using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    [Header("Player Controller")]
    public PlayerStateMachine Machine;
    public Vector2 MoveDirection;
    public bool isAiming;
    public bool isAttacking;
    bool hasJustAttacked;

    readonly int SpeedHash = Animator.StringToHash("Speed");
    readonly int AimHash = Animator.StringToHash("IsAiming");
    readonly int AttackHash = Animator.StringToHash("IsAttacking");

    protected override void Awake()
    {
        base.Awake();
        Machine = GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        UpdateMove();
        UpdateAim();
        UpdateAttack();

        hasJustAttacked = false;
    }

    public void UpdateMove()
    {
        if (Machine.CurrentState.GetStateType() == PlayerStateType.Cover) return;
    
        if (MoveDirection == null || MoveDirection.x == 0) // Short circuit evaluation
        {
            Machine.ChangeState(PlayerStateType.Idle);
        }
        else
        {
            Machine.ChangeState(PlayerStateType.Move);
        }
        Animator.SetFloat(SpeedHash, Mathf.Clamp01(MoveCtrl.Ctrl.velocity.magnitude));
    }

    public void UpdateAim()
    {
        if (isAiming)
        {
            // TODO implement aim anim and etc.
            Animator.SetBool(AimHash, true);
        }
        else
        {
            Animator.SetBool(AimHash, false);
        }
    }

    public void UpdateAttack()
    {
        if (isAiming && isAttacking)
        {
            CurrentWeapon.Attack(hasJustAttacked);
            Animator.SetBool(AttackHash, true);
        }
        else
        {
            Animator.SetBool(AttackHash, false);
        }
    }

    public void OnMove(InputValue value)
    {
        MoveDirection = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        isAttacking = value.isPressed;
        hasJustAttacked = value.isPressed;
    }

    public void OnAim(InputValue value)
    {
        isAiming = value.isPressed;
    }

    public void OnCover()
    {
        if (Machine.CurrentState.GetStateType() != PlayerStateType.Cover)
        {
            Debug.Log("Enter Cover State");
            Machine.ChangeState(PlayerStateType.Cover);
        }
        else
        {
            Debug.Log("Exit Cover State");
            Machine.ChangeState(PlayerStateType.Idle);
        }
    }

    public bool TryAttack()
    {
        return true;
    }
}
