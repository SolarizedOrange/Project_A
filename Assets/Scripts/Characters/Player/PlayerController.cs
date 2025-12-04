using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    [Header("Player Controller")]
    [SerializeField] public Rig AimRig;
    [SerializeField] Transform AimTarget;
    public Vector2 MoveDirection;
    public Vector3 AimPos;
    public bool IsAiming;
    public bool IsReloading;
    public bool IsAttacking;
    readonly int SpeedHash = Animator.StringToHash("Speed");
    readonly int AimHash = Animator.StringToHash("IsAiming");
    InputAction mouseInputAction;

    protected override void Awake()
    {
        base.Awake();
        mouseInputAction = GetComponent<PlayerInput>().actions["Pointer"];
    }

    void Update()
    {
        UpdateMove();
        UpdateAim();
    }

    public void UpdateMove()
    {
        if (IsCover || MoveDirection.x == 0)
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
        }
        Animator.SetFloat(SpeedHash, Mathf.Clamp01(MoveCtrl.Ctrl.linearVelocity.magnitude));
    }

    public void UpdateAim()
    {
        if (IsAiming)
        {
            AimPos = mouseInputAction.ReadValue<Vector2>();
            AimPos.x -= Screen.width / 2;
            AimPos.y -= Screen.height / 2;
            AimPos = AimPos.normalized;
            MoveCtrl.SetTargetRotation(AimPos);
            AimTarget.position = transform.position + 3f * AimPos;
        }
        Animator.SetBool(AimHash, IsAiming);
    }

    public void OnMove(InputValue value)
    {
        MoveDirection = value.Get<Vector2>();
    }

    public void OnAim(InputValue value)
    {
        IsAiming = CurrentWeapon != null && value.isPressed;
        AimRig.weight = IsAiming ? 1 : 0;
    }
}
