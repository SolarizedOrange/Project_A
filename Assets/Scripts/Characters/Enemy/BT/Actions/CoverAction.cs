using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

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
    float timer;
    bool isMoveToCover;

    protected override Status OnStart()
    {
        // Debug.Log("CoverStart");
        isMoveToCover = false;
        ctrl = Agent.Value.MoveCtrl;
        timer = Mathf.Max(ctrl.PositionTransitionTime, ctrl.RotateTransitionTime);
        if (CoverObject.Value == null) return Status.Success;

        if (IsCover.Value)
		{
            isMoveToCover = true;
            EnterCover();
		}
        else if (InEnter.Value)
		{
            isMoveToCover = true;
			ExitCover();
		}
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (timer < 0 || isMoveToCover == false) return Status.Success;

        timer -= Time.deltaTime;
        return Status.Running;
    }

    void EnterCover()
    {
        ctrl.SetTargetPositionXZ(CoverObject.Value.position,false);
        InEnter.Value = true;
        // Debug.Log($"Enter Cover c:{IsCover.Value}");
    }

    void ExitCover()
    {
        ctrl.SetTargetPositionXZ(AgentReturnPos.Value, false);
        InEnter.Value = false;
        // Debug.Log($"Exit Cover c:{IsCover.Value}");
    }
}

