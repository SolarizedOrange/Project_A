using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindCoverObject", story: "[Agent] covers [CoverObject] within [FindDistance]", category: "Action", id: "4dda05289653fa594013b18a7433b03f")]
public partial class FindCoverObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<Transform> CoverObject;
    [SerializeReference] public BlackboardVariable<Vector3> FindDistance;
    // [SerializeReference] public BlackboardVariable<float> coverMinDistance;

    protected override Status OnStart()
    {
        if (TryFindCoverObject())
		{
            Debug.Log("Find");
			return Status.Success;
		}
        else return Status.Failure;
    }

    bool TryFindCoverObject()
    {
        float minDist = float.MaxValue;
        var curPos = Agent.Value.transform.position;
        // var maxDist = FindDistance.Value.magnitude;

        // old Ovelap Search
        // Collider[] colliders = Physics.OverlapSphere(curPos, maxDist, (int)Layers.EnemyCoverable);
        // new Ovelap Search
        Collider[] colliders = Physics.OverlapBox(curPos,FindDistance.Value,Quaternion.identity,(int)Layers.EnemyCoverable);
        Transform findMinObject = null;
        foreach (var cover in colliders)
        {
            var dif = curPos - cover.transform.position;
            // var dif_x = Mathf.Abs(dif.x);
            // var dif_z = Mathf.Abs(dif.z);
            var dist = dif.sqrMagnitude;
            
            // if (dif_x < FindDistance.Value.x && dif_z < FindDistance.Value.z && minDist > dist)
            if (minDist > dist)
            {
                findMinObject = cover.transform;
                minDist = dist;
            }
        }
        CoverObject.Value = findMinObject;
        if (CoverObject.Value == null)
        {
            // Debug.Log("Fallback to BattleIdle");
            return false;
        }
        else return true;
    }
}

