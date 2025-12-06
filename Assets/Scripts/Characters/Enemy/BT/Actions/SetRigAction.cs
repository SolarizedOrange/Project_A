using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetRig", story: "Set [RigBuilder] [Layer] [isActive]", category: "Action", id: "d21941ebfe12d715d121a2c1580fed3d")]
public partial class SetRigAction : Action
{
    [SerializeReference] public BlackboardVariable<RigBuilder> RigBuilder;
    [SerializeReference] public BlackboardVariable<int> Layer;
    [SerializeReference] public BlackboardVariable<bool> IsActive;
    protected override Status OnStart()
    {
        RigBuilder.Value.layers[Layer.Value].active = IsActive.Value;
        return Status.Success;
    }
}

