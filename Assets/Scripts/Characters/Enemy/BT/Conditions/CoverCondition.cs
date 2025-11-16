using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CoverCondition", story: "[Agent] isCover?", category: "Conditions", id: "38002e8208067719b844a45cc509c60c")]
public partial class CoverCondition : Condition
{
    [SerializeReference] public BlackboardVariable<CharacterBase> Agent;

    public override bool IsTrue()
    {
        if (Agent.Value == null)
        {
            return false;
        }
        return Agent.Value.IsCover;
    }
}
