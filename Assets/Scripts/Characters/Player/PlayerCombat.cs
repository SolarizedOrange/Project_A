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
        if (PlayerCtrl.IsAiming && PlayerCtrl.IsAttacking && PlayerCtrl.IsMeleeAttacking == false)
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

        PlayerCtrl.Recoil = Vector3.Lerp(PlayerCtrl.Recoil, Vector3.zero, recoverRecoil ? 2f * Time.deltaTime : 0.25f * Time.deltaTime);
        hasJustAttacked = false;
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

        // Melee Attack Input
        if (PlayerCtrl.IsAiming == false && value.isPressed)
        {
            if (PlayerCtrl.IsMeleeAttacking == false)
            {
                MeleeAttackStart();
            }
            else
            {
                MeleeTouch();
            }
        }
    }

    public void OnReload()
    {
        StartCoroutine(DoReloadRoutine());
    }

    #region Melee Attack
    [SerializeField] float meleeRange = 2f;
    [SerializeField] float meleeCooltime = 10f;
    [SerializeField] int noteMinCount = 5;
    [SerializeField] int noteMaxCount = 10;
    [SerializeField] float noteInterval = 1.5f;
    [SerializeField] float marginTime = 0.3f;
    [SerializeField] float marginPositionDistance = 100f;
    [SerializeField] HitBox targetEnemyBox;
    [SerializeField] EnemyController targetEnemy;

    public static UnityEvent<EndureNote> EndureNoteEvent = new();
    public static UnityEvent<bool> EndureEndEvent = new();

    Queue<EndureNote> queue = new();
    Vector2 mousePos = Vector2.zero;
    float lastMeleeTime = -1000f;
    void MeleeAttackStart()
    {
        // Check Melee Hit
        var ray = new Ray(
            transform.position,
            transform.forward
        );
        var res = new RaycastHit[10];
        Physics.RaycastNonAlloc(ray, res, meleeRange, (int)Layers.HitCollider);
        Array.Sort(res, (a, b) => a.distance.CompareTo(b.distance));
        // MeleeAttackAction();
        
        foreach (var hit in res)
        {
            if (hit.collider != null)
            {
                targetEnemyBox = hit.collider.GetComponent<HitBox>();
                targetEnemy = targetEnemyBox?.GetComponentInParent<EnemyController>();
                var canMeleeAttack = Time.time > lastMeleeTime + meleeCooltime
                    && targetEnemyBox != null
                    && targetEnemy != null;
                
                if (canMeleeAttack)
                {
                    // Melee Action
                    MeleeAttackAction();
                    return;
                }
            }
        }
    }

    void MeleeAttackAction()
    {
        PlayerDamage.EndureEndEvent.Invoke(false);
        PlayerCtrl.IsMeleeAttacking = true;
        // Enemy Targeting and Note Generation
        targetEnemy.SetEnemyAction(EnemyActionType.MeleeTargeted);

        int noteCount = Random.Range(noteMinCount, noteMaxCount+1);
        float lastNoteTime = Time.time;

        for (int i = 0; i < noteCount; i++)
        {
            var time = lastNoteTime + Random.Range(0.0f, noteInterval);
            var note = new EndureNote{StartTime = time, Duration = 0.0f, Type = EndureNoteType.Tap, Target = targetEnemy.transform, ScreenOffset = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0)};
            queue.Enqueue(note);
            EndureNoteEvent.Invoke(note);

            lastNoteTime = time;
        }
        Debug.Log($"MeleeAttack Routine Start {queue.Count}");

        StartCoroutine(MeleeAttackRoutine());

    }
    
    IEnumerator MeleeAttackRoutine()
    {
        while(queue.Count > 0 && PlayerCtrl.IsMeleeAttacking)
        {
            var note = queue.Peek();
            if (note.StartTime + note.Duration < Time.time)
            {
                queue.Dequeue();
                MeleeAttackEnd(false);
            }
            yield return null;
        }
        // Successfully cleared all notes
        if (queue.Count == 0 && PlayerCtrl.IsMeleeAttacking) MeleeAttackEnd(true);
    }

    public void MeleeTouch()
    {
        if (queue.Count > 0)
        {
            var note = queue.Peek();
            if (Mathf.Abs(note.StartTime - Time.time) < marginTime
                && Vector2.Distance(note.Target.position, mousePos) < marginPositionDistance)
            {
                Debug.Log($"Melee Touch");
                queue.Dequeue();
            }
            // miss touching
            else
            {
                MeleeAttackEnd(false);
            }
        }
    }

    public void OnMeleePosition(InputValue value)
    {
        mousePos = value.Get<Vector2>();
    }

    public void MeleeAttackEnd(bool success = false)
    {
        Debug.Log($"Melee Attack End. Success: {success}");
        queue.Clear();
        PlayerCtrl.IsMeleeAttacking = false;
        if (success && targetEnemyBox != null)
        {
            // Deal Damage
            // targetEnemyBox.OnHit(HitBoxType.PlayerMelee, PlayerCtrl.MeleeDamage);
        }
        targetEnemy.SetEnemyAction(EnemyActionType.None);
        EndureEndEvent.Invoke(success);
        lastMeleeTime = Time.time;
    }

    #endregion
}
