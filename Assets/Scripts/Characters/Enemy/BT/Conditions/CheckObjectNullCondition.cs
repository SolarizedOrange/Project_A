using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Object Null", story: "Check if [TargetObject] is null or has been destroyed.", category: "Conditions", id: "0e31b802b3d63244611c09446d4171d8")]
public partial class CheckObjectNullCondition : Condition
{
    [SerializeReference] public BlackboardVariable<CharacterBase> TargetObject;

    public override bool IsTrue()
    {
        if (TargetObject == null || TargetObject.Value == null) return true;
        var raw = TargetObject.Value.gameObject;
        return ReferenceEquals(raw, null) || raw == null;
    }
}
