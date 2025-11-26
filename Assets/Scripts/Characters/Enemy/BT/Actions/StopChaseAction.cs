using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopChaseAction", story: "[Agent] Stop chasing", category: "Action", id: "1fcb49870df57ce59bb8ae7e5eb40a2f")]
public partial class StopChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;

    protected override Status OnStart()
    {
        Agent.Value.MoveCtrl.SetTargetVelocity(Vector3.zero);
        return Status.Success;
    }
}

