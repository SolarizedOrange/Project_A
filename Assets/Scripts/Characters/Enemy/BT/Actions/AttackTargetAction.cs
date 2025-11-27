using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "[Agent] attacks [Target] within [AttackDistance]", category: "Action", id: "85cd35e47cd82a5f68c4924ded1af75a")]
public partial class AttackTargetAction : Action
{
    readonly int StateHash = Animator.StringToHash("CurState");
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<CharacterBase> Target;
    [SerializeReference] public BlackboardVariable<float> AttackDistance;

	protected override Status OnStart()
	{
        var dir = Target.Value.transform.position - Agent.Value.transform.position;
        Agent.Value.MoveCtrl.SetTargetRotation(dir);
        Agent.Value.CurrentWeapon.Attack(true);
        return Status.Success;
	}
}

