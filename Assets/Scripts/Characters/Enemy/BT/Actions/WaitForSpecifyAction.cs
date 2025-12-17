using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForSpecifyAction", story: "Wait for [ActionType] [IsEqual] [EqualType]", category: "Action", id: "1d8cb2b8dce5ead6771703a8e454bee7")]
public partial class WaitForSpecifyAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyActionType> ActionType;
    [SerializeReference] public BlackboardVariable<EnemyActionType> EqualType;
    [SerializeReference] public BlackboardVariable<bool> IsEqual;
    private int currentFrame;
    [CreateProperty] private int frameDelta;
    

    protected override Status OnStart()
    {
        currentFrame = Time.frameCount;

        if (ConditionCheck() == false)
        {
            return Status.Success;
        }
        else return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (currentFrame == Time.frameCount)
        {
            return Status.Running;
        }
        currentFrame = Time.frameCount;

        if (ConditionCheck() == false)
        {
            return Status.Success;
        }
        else return Status.Running;
    }

    bool ConditionCheck()
    {
        if (IsEqual.Value)
        {
            return ActionType.Value == EqualType.Value;
        }
        else
        {
            return ActionType.Value != EqualType.Value;
        }
    }
}

