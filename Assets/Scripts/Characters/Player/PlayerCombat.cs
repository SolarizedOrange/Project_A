using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat: PlayerComponent
{
    bool hasJustAttacked;
    public void OnWeaponSwap()
    {
        PlayerCtrl.IsAiming = false;
    }

    public void TryAttack()
    {
        if (PlayerCtrl.IsAiming)
        {
            PlayerCtrl.CurrentWeapon.Attack(hasJustAttacked);        
        }
        else
        {
            // Do Melee
        }
        hasJustAttacked = false;
    }

    public void OnAttack(InputValue value)
    {
        PlayerCtrl.IsAttacking = value.isPressed;
        hasJustAttacked = value.isPressed;
        TryAttack();
    }

    public void OnReload()
    {
        if (PlayerCtrl.CurrentWeapon != null && PlayerCtrl.CurrentWeapon.GetWeaponType() != WeaponType.Melee)
        {
            (PlayerCtrl.CurrentWeapon as RangedWeapon).Reload();
        }
    }

}
