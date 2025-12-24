using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetCoverValue", story: "Set [Agent] [IsCover] to [Value]", category: "Action", id: "87a3e2c9c99ef38d3c08120bc950cba8")]
public partial class SetCoverValueAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<bool> IsCover;
    [SerializeReference] public BlackboardVariable<bool> Value;

    protected override Status OnStart()
    {
        var ctrl = Agent.Value;
        if (ctrl == null || IsCover == null || Value == null)
        {
            return Status.Failure;
        }
        ctrl.IsCover = Value.Value;
        IsCover.Value = Value.Value;
        return Status.Success;
    }

}

