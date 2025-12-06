using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateToTarget", story: "[Agent] rotate to [Target] [TargetPosition]", category: "Action", id: "df05907a588d4c845f3669f3597c1942")]
public partial class RotateToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<CharacterBase> Target;
    [SerializeReference] public BlackboardVariable<Transform> TargetPosition;

    protected override Status OnStart()
    {
        var dir = Target.Value.transform.position - Agent.Value.transform.position;
        TargetPosition.Value.position = Target.Value.transform.position;
        Agent.Value.MoveCtrl.SetTargetRotation(dir.x * Vector3.right);
        return Status.Success;
    }
}

