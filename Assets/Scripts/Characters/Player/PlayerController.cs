using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    [Header("Player Controller")]
    [SerializeField] public Rig AimRig;
    [SerializeField] Transform AimOrigin;
    [SerializeField] Transform AimTarget;
    public Vector2 MoveDirection;
    public Vector3 AimPos;
    public Vector3 Recoil;
    public bool IsAiming;
    public bool IsReloading;
    public bool IsAttacking;
    readonly int SpeedHash = Animator.StringToHash("Speed");
    readonly int AimHash = Animator.StringToHash("IsAiming");
    PlayerCombat playerCombat;
    PlayerCover playerCover;
    PlayerDamage playerDamage;
    InputAction mouseInputAction;

    protected override void Awake()
    {
        base.Awake();
        mouseInputAction = GetComponent<PlayerInput>().actions["Pointer"];

        playerCombat = GetComponent<PlayerCombat>();
        playerCover = GetComponent<PlayerCover>();
        playerDamage = GetComponent<PlayerDamage>();
    }

    void Update()
    {
        UpdateMove();
        UpdateAim();
        playerCover.UpdateCover();
        playerCombat.UpdateAttack();
        playerDamage.UpdateDamage();
    }

    public void UpdateMove()
    {
        if (IsCover || MoveDirection.x == 0)
        {
            MoveCtrl.SetTargetVelocity(Vector3.zero * Stat.MoveSpeed);
        }
        else
        {
            MoveCtrl.SetTargetVelocity(Vector3.right * MoveDirection.x * Stat.MoveSpeed);
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
            AimPos.z = Vector3.Dot(transform.position - Camera.main.transform.position, Camera.main.transform.forward);
            AimPos = Camera.main.ScreenToWorldPoint(AimPos);
            MoveCtrl.SetTargetRotation(AimPos - transform.position);
            if ((AimPos - AimOrigin.position).sqrMagnitude > 0.1f)
            {
                AimOrigin.LookAt(AimPos);                
            }
            AimTarget.localPosition = Vector3.forward * 3 + Recoil;
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

    public override void OnDamage(HitBoxType hitboxType, float damage)
    {
        playerDamage.DoDamage(hitboxType, damage);
    }
}
