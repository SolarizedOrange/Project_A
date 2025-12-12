using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Animations.Rigging;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseTarget", story: "[Agent] Chases [Target] until [MinDistance]", category: "Action", id: "0fcdb71c1f737236de708016bdbc8318")]
public partial class ChaseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<CharacterBase> Target;
    [SerializeReference] public BlackboardVariable<float> MinDistance;
    [SerializeReference] public BlackboardVariable<bool> IsHit;
    [SerializeReference] public BlackboardVariable<Transform> TargetPosition;

    protected override Status OnUpdate()
    {
        if (IsHit.Value)
            return Status.Failure;

        AimToTarget();
        MoveToTarget();
        return Status.Running;
    }

    void AimToTarget()
	{
        TargetPosition.Value.position = Target.Value.transform.position;
	}

    void MoveToTarget()
	{
        var p = Agent.Value;
        var movement = Target.Value.transform.position - p.transform.position;

        var dir = movement.normalized;
        if (movement.magnitude > MinDistance.Value)
        {
            var spd = p.Stat.MoveSpeed;
            p.MoveCtrl.SetTargetVelocity(Vector3.right * dir.x * spd);
        }
        else 
        {
            p.MoveCtrl.SetTargetVelocity(Vector3.zero);
        }
        p.MoveCtrl.SetTargetRotation(Vector3.right * dir.x);
	}

	protected override void OnEnd()
	{
        Agent.Value.MoveCtrl.SetTargetVelocity(Vector3.zero);
	}
}

