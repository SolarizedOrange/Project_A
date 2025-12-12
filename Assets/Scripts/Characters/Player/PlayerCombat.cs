using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    void Update()
    {
        UpdateAttack();
        UpdateReload();
    }

    public void UpdateAttack()
    {
        if (PlayerCtrl.IsAiming && PlayerCtrl.IsAttacking)
        {
            if (PlayerCtrl.CurrentWeapon.Attack(hasJustAttacked))
            {
                Debug.Log("RECOIL");
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
        // PlayerCtrl.Animator.SetBool(AttackHash, true);
        yield return new WaitForSeconds(PlayerCtrl.CurrentWeapon.Stat.AttackRate);
        // PlayerCtrl.Animator.SetBool(AttackHash, false);
        recoverRecoil = true;
        Debug.Log("Do Recoil");
    }

    public void UpdateReload()
    {
        if (PlayerCtrl.IsReloading)
        {
            var ranged = PlayerCtrl.CurrentWeapon as RangedWeapon;
            ranged.Reload();
            PlayerCtrl.IsReloading = ranged.IsReloading;
        }
        PlayerCtrl.Animator.SetBool(ReloadHash, PlayerCtrl.IsReloading);
    }

    public void OnWeaponSwap()
    {
        PlayerCtrl.IsAiming = false;
        PlayerCtrl.IsAttacking = false;
        PlayerCtrl.IsReloading = false;
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
        var ranged = PlayerCtrl.CurrentWeapon as RangedWeapon;
        if (ranged != null)
        {
            PlayerCtrl.IsReloading = true;
        }
    }

}
