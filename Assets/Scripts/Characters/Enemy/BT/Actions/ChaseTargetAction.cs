using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseTarget", story: "[Agent] Chases [Target] until [MinDistance]", category: "Action", id: "0fcdb71c1f737236de708016bdbc8318")]
public partial class ChaseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<CharacterBase> Target;
    [SerializeReference] public BlackboardVariable<float> MinDistance;
    [SerializeReference] public BlackboardVariable<bool> IsHit;

    protected override Status OnUpdate()
    {
        if (IsHit.Value)
            return Status.Failure;

        var movement = Target.Value.transform.position - Agent.Value.transform.position;

        var dir = movement.normalized;
        if (movement.magnitude > MinDistance.Value)
        {
            Agent.Value.MoveCtrl.SetTargetVelocity(Vector3.right * dir.x * Agent.Value.Stat.MoveSpeed.BaseVal);
        }
        else 
        {
            Agent.Value.MoveCtrl.SetTargetVelocity(Vector3.zero);
        }
        Agent.Value.MoveCtrl.SetTargetRotation(Vector3.right * dir.x);
        return Status.Running;

    }

	protected override void OnEnd()
	{
        Agent.Value.MoveCtrl.SetTargetVelocity(Vector3.zero);
	}
}

