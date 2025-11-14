using UnityEngine;

[CreateAssetMenu(menuName="State/Cover")]
public class PlayerStateCover : PlayerStateBase
{    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public override PlayerStateType GetStateType()
    {
        return PlayerStateType.Cover;
    }
}
