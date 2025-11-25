using UnityEngine;

[CreateAssetMenu(menuName="State/Cover")]
public class PlayerStateCover : PlayerStateBase
{    
    private Vector3 m_originPos;
    public override void OnEnter()
    {
        float minDist = float.MaxValue;
        Collider closestCover = null;

        Collider[] colliders = Physics.OverlapSphere(PlayerCtrl.transform.position, 3f, (int)Layers.PlayerCoverable);

        foreach (var cover in colliders)
        {
            float dist = Vector3.SqrMagnitude(PlayerCtrl.transform.position - cover.transform.position);

            if (minDist > dist)
            {
                closestCover = cover;
                m_originPos = Vector3.Scale(PlayerCtrl.transform.position,new Vector3(1,1,0));
                PlayerCtrl.MoveCtrl.SetTargetPositionXZ(cover.transform.position);
                // PlayerCtrl.MoveCtrl.SetTargetRotation(Vector3.right);
                minDist = dist;
            }
        }
        if (closestCover == null)
        {
            Debug.Log("Fallback to Idle");
            PlayerCtrl.Machine.ChangeState(PlayerStateType.Idle);
            
            return;
        }
        Debug.Log("Initiate Cover");

        // TODO: handle cover sqeuence with closestCover 
    }

    public override void OnUpdate()
    {
        // throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        // throw new System.NotImplementedException();
        PlayerCtrl.MoveCtrl.SetTargetPositionXZ(m_originPos);

    }

    public override PlayerStateType GetStateType()
    {
        return PlayerStateType.Cover;
    }
}
