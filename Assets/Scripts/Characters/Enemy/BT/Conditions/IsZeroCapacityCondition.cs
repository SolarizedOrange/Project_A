using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsZeroCapacity", story: "Is zero [Agent] Capacity", category: "Conditions", id: "dd7e31801888496545960557e7a1e9ce")]
public partial class IsZeroCapacityCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;

    public override bool IsTrue()
    {
        var weapon = Agent.Value.CurrentWeapon;
        if (weapon != null && weapon.Stat.Capacity.Val > 0)
            return false;
        return true;
    }
}
