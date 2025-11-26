using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Cover", story: "Agent [isCover] in [CoverObject]", category: "Action", id: "fe243adc61bdf0bf1602db6de6f3570d")]
public partial class CoverAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> IsCover;
    [SerializeReference] public BlackboardVariable<bool> InEnter;
    [SerializeReference] public BlackboardVariable<Transform> CoverObject;
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<Vector3> AgentReturnPos;

    MovementController ctrl;

    protected override Status OnStart()
    {
        Debug.Log("CoverStart");

        ctrl = Agent.Value.MoveCtrl;
        if (CoverObject.Value == null) return Status.Failure;

        if (IsCover.Value)
		{
            EnterCover();
		}
        else if (InEnter.Value)
		{
			ExitCover();
		}
        return Status.Success;
    }

    void EnterCover()
    {
        AgentReturnPos.Value = Vector3.Scale(Agent.Value.transform.position, new Vector3(1,1,0));
        ctrl.SetTargetPositionXZ(CoverObject.Value.position);
        InEnter.Value = true;
        // Debug.Log($"Enter Cover c:{IsCover.Value}");
    }

    void ExitCover()
    {
        ctrl.SetTargetPositionXZ(AgentReturnPos.Value);
        InEnter.Value = false;
        // Debug.Log($"Exit Cover c:{IsCover.Value}");
    }
}

