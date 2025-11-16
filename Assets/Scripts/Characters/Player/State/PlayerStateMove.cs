using UnityEngine;

[CreateAssetMenu(menuName="State/Move")]
public class PlayerStateMove : PlayerStateBase
{
    public override void OnEnter()
    {
        // throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        PlayerCtrl.MoveCtrl.Move(Vector3.right * PlayerCtrl.MoveDirection * Time.deltaTime);
    }

    public override void OnExit()
    {
        // throw new System.NotImplementedException();
    }

    public override PlayerStateType GetStateType()
    {
        return PlayerStateType.Move;
    }
}
