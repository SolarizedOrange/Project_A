using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Reload", story: "[Agent] reload", category: "Action", id: "c9565148f238d526e9d3bbff81e046e4")]
public partial class ReloadAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;

    float timer;
    RangedWeapon weapon;
    protected override Status OnStart()
    {
        weapon = Agent.Value.CurrentWeapon as RangedWeapon;
        timer = weapon.Stat.ReloadTime;
        if (weapon != null)
        {
            weapon.Reload(Agent.Value.BulletAmmo[weapon.GetWeaponType()]);
            return Status.Running;
        }
        return Status.Failure;
    }

	protected override Status OnUpdate()
	{
		if (timer < 0f || weapon == null) 
        { 
            weapon.IsReloading = false;
            return Status.Success;
        }
        timer -= Time.deltaTime;
        return Status.Running;
    }
}

