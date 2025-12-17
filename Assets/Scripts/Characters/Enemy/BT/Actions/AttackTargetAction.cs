using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "[Agent] attacks [Target]", category: "Action", id: "85cd35e47cd82a5f68c4924ded1af75a")]
public partial class AttackTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<CharacterBase> Target;

    EnemyController enemy;
	protected override Status OnStart()
	{
        enemy = Agent.Value;
        if (enemy.CurrentWeapon.Attack(true))
        {
            enemy.Recoil.x += enemy.CurrentWeapon.Stat.Recoil * UnityEngine.Random.Range(-0.1f, 0.1f);
            enemy.Recoil.y += enemy.CurrentWeapon.Stat.Recoil * UnityEngine.Random.Range(-0.2f, 0.3f);
            enemy.Recoil.x = Mathf.Clamp(enemy.Recoil.x, -0.3f, 0.3f);    
            return Status.Success;
        }
        else return Status.Failure;
	}
}

