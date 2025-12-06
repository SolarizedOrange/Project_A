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

    protected override Status OnStart()
    {
        var weapon = Agent.Value.CurrentWeapon as RangedWeapon;
        if (weapon != null)
        {
            weapon.Reload();
            return Status.Success;
        }
        return Status.Failure;
    }
}

