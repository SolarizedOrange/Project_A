using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Reload", story: "[Agent] : Ammo < [Threshold] Flip: [True] RandomThreshold: [RandomThreshold]", category: "Conditions", id: "dd7e31801888496545960557e7a1e9ce")]
public partial class ReloadCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<bool> True;
    [SerializeReference] public BlackboardVariable<bool> RandomThreshold;
    [SerializeReference] public BlackboardVariable<int> Threshold;

    public override bool IsTrue()
    {
        var weapon = Agent.Value.CurrentWeapon as RangedWeapon;
        var reloadThreshold = RandomThreshold.Value? UnityEngine.Random.Range(0,Threshold.Value): Threshold;
        if (weapon != null && weapon.Ammo < reloadThreshold)
            return true != True.Value;
        return false != True.Value;
    }
}
