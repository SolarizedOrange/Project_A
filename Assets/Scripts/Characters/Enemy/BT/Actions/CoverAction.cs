using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Rendering;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Cover", story: "[Agent] covers CoverObject within [FindDistance]", category: "Action", id: "4dda05289653fa594013b18a7433b03f")]
public partial class CoverAction : Action
{
    [SerializeReference] public BlackboardVariable<CharacterBase> Agent;
    [SerializeReference] public BlackboardVariable<float> FindDistance;
    private Collider m_closestCover = null;

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

    bool TryFindCoverObject()
    {
        var Ctrl = Agent.Value;
        float minDist = float.MaxValue;

        Collider[] colliders = Physics.OverlapSphere(Ctrl.transform.position, FindDistance, (int)Layers.Coverable);

        foreach (var cover in colliders)
        {
            float dist = Vector3.SqrMagnitude(Ctrl.transform.position - cover.transform.position);

            if (minDist > dist)
            {
                m_closestCover = cover;
                minDist = dist;
            }
        }
        if (m_closestCover == null)
        {
            Debug.Log("Fallback to BattleIdle");
            return false;
        }
        else return true;
    }

    void EnterCover()
    {
        //Test Logic
        Agent.Value.transform.localScale = Vector3.one * 0.5f;
        Debug.Log($"Enter Cover {m_closestCover.name}");
    }

    void ExitCover()
    {
        Agent.Value.transform.localScale = Vector3.one;
        Debug.Log("Exit Cover");
    }
}

