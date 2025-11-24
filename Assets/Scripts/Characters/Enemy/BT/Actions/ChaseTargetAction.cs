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
        if (Vector3.Distance(Agent.Value.transform.position, Target.Value.transform.position) > MinDistance.Value)
        {
            Vector3 direction = (Target.Value.transform.position - Agent.Value.transform.position).normalized;
            Agent.Value.MoveCtrl.Ctrl.Move(direction * Time.deltaTime * Agent.Value.Stat.MoveSpeed.Val);
            return Status.Running;
        }
        else return Status.Success;
    }
}

