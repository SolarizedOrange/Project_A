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
    }

    public void OnMove(InputValue value)
    {
        MoveDirection = value.Get<Vector2>();

        if (MoveDirection == null || MoveDirection.x == 0) // Short circuit evaluation
        {
            Machine.ChangeState(PlayerStateType.Idle);
        }
        else
        {
            Machine.ChangeState(PlayerStateType.Move);
        }
    }

    public void OnAttack()
    {

    }

    public void OnCover()
    {

    }

    public bool TryAttack()
    {
        return true;
    }
}
