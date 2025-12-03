using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "[Agent] attacks [Target] within [AttackDistance]", category: "Action", id: "85cd35e47cd82a5f68c4924ded1af75a")]
public partial class AttackTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<CharacterBase> Target;
    [SerializeReference] public BlackboardVariable<float> AttackDistance;

	protected override Status OnStart()
	{
        Agent.Value.CurrentWeapon.Attack(true);
        return Status.Success;
	}
}

