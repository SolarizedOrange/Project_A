using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdateRecoil", story: "[Agent] Update Recoil with [TargetPosition]", category: "Action", id: "9ced54b8aebfdf03dfdb8e188a909e7f")]
public partial class UpdateRecoilAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<Transform> TargetPosition;
    [SerializeReference] public BlackboardVariable<CharacterBase> Target;
    [SerializeReference] public BlackboardVariable<EnemyActionType> Type;

    protected override Status OnUpdate()
    {
        TargetPosition.Value.position = Target.Value.transform.position + Agent.Value.Recoil;
		Agent.Value.Recoil = Vector3.Lerp(Agent.Value.Recoil, Vector3.zero, Type.Value == EnemyActionType.Attack ? 2f * Time.deltaTime : 0.25f * Time.deltaTime);
        return Status.Running;
    }
}

