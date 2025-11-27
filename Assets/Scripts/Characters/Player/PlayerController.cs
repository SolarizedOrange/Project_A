using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    [Header("Player Controller")]
    // public PlayerStateMachine Machine;
    public Vector2 MoveDirection;
    public bool IsAiming;
    public bool IsAttacking;
    readonly int SpeedHash = Animator.StringToHash("Speed");
    readonly int AimHash = Animator.StringToHash("IsAiming");
    readonly int AttackHash = Animator.StringToHash("IsAttacking");
    readonly int CoverHash = Animator.StringToHash("IsCover");

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        UpdateMove();
        UpdateAnim();
    }

    public void UpdateMove()
    {
        if (IsCover || MoveDirection == null || MoveDirection.x == 0) // Short circuit evaluation
        {
            MoveCtrl.SetTargetVelocity(Vector3.zero * Stat.MoveSpeed.Val);
        }
        else
        {
            MoveCtrl.SetTargetVelocity(Vector3.right * MoveDirection.x * Stat.MoveSpeed.Val);
            if (!IsAiming)
            {
                MoveCtrl.SetTargetRotation(Vector3.right * MoveDirection.x);
            }
            else
            {
                MoveCtrl.SetTargetRotation(Vector3.right * MoveDirection.x);
            }
        }
    }

    public void UpdateAim()
    {
        if (IsAiming)
        {
            // TODO implement aim anim and etc.
        }
    }

    public void UpdateAnim()
    {
        Animator.SetFloat(SpeedHash, Mathf.Clamp01(MoveCtrl.Ctrl.linearVelocity.magnitude));
        Animator.SetBool(AimHash, IsAiming);
        Animator.SetBool(AttackHash, IsAiming && IsAttacking);
    }


    public void OnMove(InputValue value)
    {
        MoveDirection = value.Get<Vector2>();
    }

    public void OnAim(InputValue value)
    {
        IsAiming = value.isPressed;
    }

}
