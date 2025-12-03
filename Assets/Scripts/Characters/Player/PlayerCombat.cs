using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat: PlayerComponent
{
    readonly int AttackHash = Animator.StringToHash("IsAttacking");
    readonly int WeaponTypeHash = Animator.StringToHash("WeaponType");
    bool hasJustAttacked;

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
            }
        }
        else
        {
            // Do Melee
        }
        hasJustAttacked = false;
    }

    IEnumerator DoRecoilRoutine()
    {
        PlayerCtrl.Animator.SetBool(AttackHash, true);
        yield return new WaitForSeconds(PlayerCtrl.CurrentWeapon.Stat.AttackRate.Val);
        PlayerCtrl.Animator.SetBool(AttackHash, false);
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
    }

    public void OnWeaponSwap()
    {
        PlayerCtrl.IsAiming = false;
        PlayerCtrl.Animator.SetInteger(
            WeaponTypeHash, PlayerCtrl.CurrentWeapon != null
            ? (int)PlayerCtrl.CurrentWeapon.GetWeaponType()
            : (int)WeaponType.None
        );
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
