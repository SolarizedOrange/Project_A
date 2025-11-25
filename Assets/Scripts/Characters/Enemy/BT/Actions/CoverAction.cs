using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Rendering;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Cover", story: "[Agent] covers [CoverObject] within [FindDistance]", category: "Action", id: "4dda05289653fa594013b18a7433b03f")]
public partial class CoverAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<Transform> CoverObject;
    [SerializeReference] public BlackboardVariable<float> FindDistance;
    [SerializeReference] public BlackboardVariable<float> CoverTime;

    private Vector3 m_origin = Vector3.zero;
    private bool m_isPlayAnim = false;

    protected override Status OnStart()
    {
        if (Agent.Value.IsCover)
		{
            Agent.Value.IsCover = false;
			ExitCover();
            return Status.Running;
		}
        else
		{
			if (TryFindCoverObject())
			{
                Agent.Value.IsCover = true;
                EnterCover();
                return Status.Running;
			}
            return Status.Success;
		}
    }

	protected override Status OnUpdate()
	{
        
		if (m_isPlayAnim)
		{
			
            return Status.Running;
		}
        else return Status.Success;
	}

    bool TryFindCoverObject()
    {
        var Ctrl = Agent.Value;
        float minDist = float.MaxValue;

        Collider[] colliders = Physics.OverlapSphere(Ctrl.transform.position, FindDistance, (int)Layers.EnemyCoverable);

        foreach (var cover in colliders)
        {
            float dist = Vector3.SqrMagnitude(Ctrl.transform.position - cover.transform.position);

            if (minDist > dist)
            {
                CoverObject.Value = cover.transform;
                minDist = dist;
            }
        }
        if (CoverObject.Value == null)
        {
            // Debug.Log("Fallback to BattleIdle");
            return false;
        }
        else return true;
    }

    void EnterCover()
    {
        m_isPlayAnim = true;
        m_origin = Agent.Value.transform.position;
        var dir = (CoverObject.Value.position - Agent.Value.transform.position).normalized;
        Agent.Value.MoveCtrl.SetTargetVelocity(dir * Agent.Value.Stat.MoveSpeed.Val);
        Debug.Log($"Enter Cover {CoverObject.Value.name}");
    }

    void ExitCover()
    {
        m_isPlayAnim = true;
        var dir = (CoverObject.Value.position - Agent.Value.transform.position).normalized;

        Agent.Value.MoveCtrl.SetTargetVelocity(dir * Agent.Value.Stat.MoveSpeed.Val);
        Debug.Log("Exit Cover");
    }


}

