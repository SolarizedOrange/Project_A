using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerCombat: PlayerComponent
{
    readonly int AttackHash = Animator.StringToHash("IsAttacking");
    readonly int ReloadHash = Animator.StringToHash("IsReloading");
    readonly int WeaponTypeHash = Animator.StringToHash("WeaponType");
    bool hasJustAttacked;
    bool recoverRecoil = true;

    void Start()
    {
        PlayerCtrl.Animator.SetInteger(
            WeaponTypeHash, PlayerCtrl.CurrentWeapon != null
            ? (int)PlayerCtrl.CurrentWeapon.GetWeaponType()
            : (int)WeaponType.None
        );
    }

    public void UpdateAttack()
    {
        if (PlayerCtrl.IsAiming)
        {
            RangedAttack();
        }
        else
        {
            MeleeAttack();
        }

        PlayerCtrl.Recoil = Vector3.Lerp(PlayerCtrl.Recoil, Vector3.zero, recoverRecoil ? 2f * Time.deltaTime : 0.25f * Time.deltaTime);
        hasJustAttacked = false;
    }

    void RangedAttack()
    {
        if (PlayerCtrl.IsAttacking)
        {
            if (PlayerCtrl.CurrentWeapon.Attack(hasJustAttacked))
            {
                StartCoroutine(DoRecoilRoutine());
                PlayerCtrl.IsReloading = false;
                PlayerCtrl.Recoil.x += PlayerCtrl.CurrentWeapon.Stat.Recoil * Random.Range(-0.1f, 0.1f);
                PlayerCtrl.Recoil.y += PlayerCtrl.CurrentWeapon.Stat.Recoil * Random.Range(-0.2f, 0.3f);
                PlayerCtrl.Recoil.x = Mathf.Clamp(PlayerCtrl.Recoil.x, -0.3f, 0.3f);
            }
        }
    }
    IEnumerator DoRecoilRoutine()
    {
        recoverRecoil = false;
        yield return new WaitForSeconds(PlayerCtrl.CurrentWeapon.Stat.AttackRate);
        recoverRecoil = true;
    }

    IEnumerator DoReloadRoutine()
    {
        var ranged = PlayerCtrl.CurrentWeapon as RangedWeapon;
        
        if (ranged != null)
        {
            PlayerCtrl.IsReloading = true;
            PlayerCtrl.Animator.SetBool(ReloadHash, PlayerCtrl.IsReloading);

            ranged.Reload(PlayerCtrl.BulletAmmo[ranged.GetWeaponType()]);

            yield return new WaitUntil(() => !ranged.IsReloading);

            PlayerCtrl.IsReloading = false;
            PlayerCtrl.Animator.SetBool(ReloadHash, PlayerCtrl.IsReloading);
        }
    }

    public void OnWeaponSwap()
    {
        PlayerCtrl.IsReloading = false;
        PlayerCtrl.Animator.SetBool(ReloadHash, PlayerCtrl.IsReloading);
        PlayerCtrl.Animator.SetInteger(
            WeaponTypeHash, PlayerCtrl.CurrentWeapon != null
            ? (int)PlayerCtrl.CurrentWeapon.GetWeaponType()
            : (int)WeaponType.None
        );
        PlayerCtrl.Recoil = Vector3.zero;
    }

    public void OnAttack(InputValue value)
    {
        PlayerCtrl.IsAttacking = value.isPressed;
        hasJustAttacked = value.isPressed;
    }

    public void OnReload()
    {
        StartCoroutine(DoReloadRoutine());
    }

    #region Melee Attack
    [SerializeField] float meleeRange = 2f;
    [SerializeField] float meleeCooldown = 10f;
    [SerializeField] int noteMinCount = 5;
    [SerializeField] int noteMaxCount = 10;
    [SerializeField] float noteInterval = 0.25f;
    [SerializeField] float marginTime = 0.3f;
    [SerializeField] float marginPositionDistance = 70f;
    [SerializeField] HitBox targetEnemyBox;
    [SerializeField] EnemyController targetEnemy;

    public static UnityEvent<EndureNote> MeleeNoteEvent = new();
    public static UnityEvent<bool> MeleeEndEvent = new();

    Queue<EndureNote> queue = new();
    float lastMeleeTime = -1000f;

    void MeleeAttack()
    {
        if (hasJustAttacked)
        {
            if (PlayerCtrl.IsMeleeAttacking)
            {   
                // Melee Attack Input
                MeleeTouch();
            }
            else
            {
                MeleeAttackStart();
            }
        }
    }
    void MeleeAttackStart()
    {
        // Check Melee Hit
        var ray = new Ray(
            transform.position,
            transform.forward
        );
        if (Physics.Raycast(ray, out var hit, meleeRange, (int)Layers.HitCollider))
        {
            targetEnemyBox = hit.collider.GetComponent<HitBox>();
            targetEnemy = targetEnemyBox.Character as EnemyController;

            Debug.DrawLine(
                hit.point - Camera.main.transform.up * 0.5f,
                hit.point + Camera.main.transform.up * 0.5f,
                Color.green,
                5.0f
            );
            Debug.DrawLine(
                hit.point - Camera.main.transform.right * 0.5f,
                hit.point + Camera.main.transform.right * 0.5f,
                Color.green,
                5.0f
            );

            if (targetEnemy == null) return;

            Debug.Log($"{targetEnemy} {targetEnemyBox}");
            if (Time.time > lastMeleeTime + meleeCooldown)
            {
                // Melee Action
                MeleeAttackAction();
                return;
            }
        }
    }

    void MeleeAttackAction()
    {
        PlayerCtrl.IsMeleeAttacking = true;
        // Enemy Targeting and Note Generation
        targetEnemy.SetEnemyAction(EnemyActionType.MeleeTargeted);

        int noteCount = Random.Range(noteMinCount, noteMaxCount+1);
        float lastNoteTime = Time.time + 3.0f;

        for (int i = 0; i < noteCount; i++)
        {
            var time = lastNoteTime + noteInterval * Random.Range(1, 5);
            Debug.Log(time);
            var note = new EndureNote{
                StartTime = time,
                Duration = 0.0f,
                Type = EndureNoteType.Tap,
                Target = targetEnemy.transform,
                ScreenOffset = new Vector3
                (
                    Random.Range(-1.0f, 1.0f),
                    Random.Range(-1.0f, 1.0f),
                    0
                )
            };
            queue.Enqueue(note);
            MeleeNoteEvent.Invoke(note);

            lastNoteTime = time;
        }
        Debug.Log($"MeleeAttack Routine Start {queue.Count}");

        StartCoroutine(MeleeAttackRoutine());
    }
    
    IEnumerator MeleeAttackRoutine()
    {
        bool success = true;

        while(queue.Count > 0)
        {
            var note = queue.Peek();
            if (note.StartTime + note.Duration + marginTime < Time.time)
            {
                Debug.Log($"MISS: {note.StartTime - Time.time}");
                success = false;
                break;
            }
            yield return null;
        }
        // Successfully cleared all notes
        if (PlayerCtrl.IsMeleeAttacking)
        {
            MeleeAttackEnd(success);
        }
    }

    public void MeleeTouch()
    {
        if (queue.Count > 0)
        {
            var note = queue.Peek();
            Debug.DrawLine(
                PlayerCtrl.AimPos - Camera.main.transform.up * 0.5f,
                PlayerCtrl.AimPos + Camera.main.transform.up * 0.5f,
                Color.red,
                5.0f
            );
            Debug.DrawLine(
                PlayerCtrl.AimPos - Camera.main.transform.right * 0.5f,
                PlayerCtrl.AimPos + Camera.main.transform.right * 0.5f,
                Color.red,
                5.0f
            );

            Debug.DrawLine(
                note.Target.position + note.ScreenOffset - Camera.main.transform.up * 0.5f,
                note.Target.position + note.ScreenOffset + Camera.main.transform.up * 0.5f,
                Color.cyan,
                5.0f
            );
            Debug.DrawLine(
                note.Target.position + note.ScreenOffset - Camera.main.transform.right * 0.5f,
                note.Target.position + note.ScreenOffset + Camera.main.transform.right * 0.5f,
                Color.cyan,
                5.0f
            );
            Debug.Log($"TOUCH: {note.StartTime - Time.time} {Vector2.Distance(note.Target.position + note.ScreenOffset, PlayerCtrl.AimPos)}");
            if (Mathf.Abs(note.StartTime - Time.time) < marginTime
                && Vector2.Distance(note.Target.position + note.ScreenOffset, PlayerCtrl.AimPos) < marginPositionDistance)
            {
                Debug.Log($"SUCCESS: {note.StartTime - Time.time}");
                Debug.Log($"Melee Touch");
                queue.Dequeue();
            }
            // miss touching
            else
            {
                Debug.Log($"TOUCHMISS: {note.StartTime - Time.time}");
                MeleeAttackEnd(false);
            }
        }
    }

    public void MeleeAttackEnd(bool success)
    {
        Debug.Log($"Melee Attack End. Success: {success}");
        queue.Clear();
        PlayerCtrl.IsMeleeAttacking = false;
        if (success)
        {
            // Deal Damage
            // targetEnemyBox.OnHit(HitBoxType.PlayerMelee, PlayerCtrl.MeleeDamage);
        }
        targetEnemy.SetEnemyAction(EnemyActionType.None);
        MeleeEndEvent.Invoke(success);
        lastMeleeTime = Time.time;
    }
    #endregion
}
