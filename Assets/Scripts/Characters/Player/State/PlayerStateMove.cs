using System;
using UnityEngine;

[CreateAssetMenu(menuName="State/Move")]
public class PlayerStateMove : PlayerStateBase
{
    // int count = 0;
    public override void OnEnter()
    {
        // throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        PlayerCtrl.MoveCtrl.TargetVelocity = Vector3.right * PlayerCtrl.MoveDirection.x * PlayerCtrl.Stat.MoveSpeed.Val;
        // if (PlayerCtrl.MoveDirection.x > 0 && isFacingRight)
        // {
            
        // }
        if (!PlayerCtrl.isAiming)
        {
            PlayerCtrl.MoveCtrl.UpdateRotation(Vector3.right * Math.Sign(PlayerCtrl.MoveDirection.x));
            // PlayerCtrl.MoveCtrl.transform.LookAt(PlayerCtrl.MoveCtrl.transform.position + Vector3.right * PlayerCtrl.MoveDirection.x);
        }
        else
        {
            PlayerCtrl.MoveCtrl.UpdateRotation(Vector3.right * Math.Sign(PlayerCtrl.MoveDirection.x));
            // PlayerCtrl.MoveCtrl.transform.LookAt(PlayerCtrl.MoveCtrl.transform.position + Vector3.right);
        }
    }

    public override void OnExit()
    {
        PlayerCtrl.MoveCtrl.TargetVelocity = Vector3.zero;
    }

    public override PlayerStateType GetStateType()
    {
        return PlayerStateType.Move;
    }
}
