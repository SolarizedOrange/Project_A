using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetAllRig", story: "Set [RigBuilder] Layers [isActive]", category: "Action", id: "461dc9cc101c94d596267a8ba7407bc3")]
public partial class SetAllRigAction : Action
{
    [SerializeReference] public BlackboardVariable<RigBuilder> RigBuilder;
    [SerializeReference] public BlackboardVariable<bool> IsActive;
    protected override Status OnStart()
    {
        var layers = RigBuilder.Value.layers;
        foreach(var layer in layers)
		{
			layer.active = IsActive.Value;
		}
        return Status.Success;
    }

}

