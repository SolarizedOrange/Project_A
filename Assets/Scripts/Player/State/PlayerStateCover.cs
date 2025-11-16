using UnityEngine;

[CreateAssetMenu(menuName="State/Cover")]
public class PlayerStateCover : PlayerStateBase
{    public override void OnEnter()
    {
        float minDist = float.MaxValue;
        Collider closestCover = null;

        Collider[] colliders = Physics.OverlapSphere(PlayerCtrl.transform.position, 3f, (int)Layers.Coverable);

        foreach (var cover in colliders)
        {
            float dist = Vector3.SqrMagnitude(PlayerCtrl.transform.position - cover.transform.position);

            if (minDist > dist)
            {
                closestCover = cover;
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
    }

    public override PlayerStateType GetStateType()
    {
        return PlayerStateType.Cover;
    }
}
