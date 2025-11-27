using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat: PlayerComponent
{
    bool hasJustAttacked;
    public void OnWeaponSwap()
    {
        
    }

    public void OnAttack(InputValue value)
    {
        PlayerCtrl.IsAttacking = value.isPressed;
        hasJustAttacked = value.isPressed;
        TryAttack();
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
}
