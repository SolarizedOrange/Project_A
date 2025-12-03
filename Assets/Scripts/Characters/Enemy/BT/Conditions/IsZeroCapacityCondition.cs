using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsZeroCapacity", story: "[Agent] Capacity is Zero?  [IsZero]", category: "Conditions", id: "dd7e31801888496545960557e7a1e9ce")]
public partial class IsZeroCapacityCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<bool> IsZero;

    public override bool IsTrue()
    {
        var weapon = Agent.Value.CurrentWeapon;
        if (weapon != null && weapon.Stat.Capacity.Val > 0)
            return false == IsZero.Value;
        return true == IsZero.Value;
    }
}
