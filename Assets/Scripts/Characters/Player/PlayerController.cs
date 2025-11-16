using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    [Header("Player Controller")]
    public PlayerStateMachine Machine;
    public Vector2 MoveDirection;
    bool isAiming;
    bool isAttacking;
    bool hasJustAttacked;

    protected override void Awake()
    {
        base.Awake();
        Machine = GetComponent<PlayerStateMachine>();
    }
    void Start()
    {
        
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
    }

    public void UpdateAim()
    {
        if (isAiming)
        {
            // TODO implement aim anim and etc.
        }
    }

    public void UpdateAttack()
    {
        if (isAiming && isAttacking)
        {
            CurrentWeapon.Attack(hasJustAttacked);
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
