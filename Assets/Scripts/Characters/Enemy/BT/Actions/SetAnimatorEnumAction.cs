using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetAnimatorEnum", story: "Set [Parameter] in [Animator] to [Value]", category: "Action", id: "a264e7e307f956ea290c967abee64ca9")]
public partial class SetAnimatorEnumAction : Action
{
    [SerializeReference] public BlackboardVariable<string> Parameter;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<EnemyAnimationActionType> Value;

    protected override Status OnStart()
    {
        if (Animator.Value == null)
        {
            LogFailure("No Animator set.");
            return Status.Failure;
        }

        Animator.Value.SetInteger(Parameter.Value, (int)Value.Value);
        return Status.Success;
    }
}

