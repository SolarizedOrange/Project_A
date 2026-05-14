using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "HealthCondition", story: "[Agent] Heath < [Threshold] Flip: [Inverse]", category: "Conditions", id: "8ab5d83ec0cee649e5bc8ae233180fa3")]
public partial class HealthCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<float> Threshold;
    [SerializeReference] public BlackboardVariable<bool> Inverse;

    public override bool IsTrue()
    {
        return false;
        
        if (Agent.Value.HP.Value < Threshold.Value)
            return !Inverse.Value;
        else
            return Inverse.Value;
    }
}
