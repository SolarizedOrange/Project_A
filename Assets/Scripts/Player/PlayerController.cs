using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{

    public PlayerStateMachine Machine;
    public Vector2 MoveDirection;

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

    public void OnMove(InputValue value)
    {
        MoveDirection = value.Get<Vector2>();
    }

    public void OnAttack()
    {

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
