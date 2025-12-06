using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitPositionTransition", story: "[Agent] wait MoveController transition", category: "Action", id: "20c743f1b9e18eae33f9604b845ecc27")]
public partial class WaitPositionTransitionAction : Action
{
    [SerializeReference] public BlackboardVariable<CharacterBase> Agent;
    float timer;
    protected override Status OnStart()
    {
        timer = 0f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var ctrl = Agent.Value.MoveCtrl;
        var delay = Mathf.Max(ctrl.PositionTransitionTime, ctrl.RotateTransitionTime);
        if (timer > delay) return Status.Success;

        timer += Time.deltaTime;
        return Status.Running;
    }
}

