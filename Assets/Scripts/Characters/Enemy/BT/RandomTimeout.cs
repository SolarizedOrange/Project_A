using System;
using Unity.Behavior;
using UnityEngine;
using Composite = Unity.Behavior.Composite;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomTimeout", story: "Execute Random [RandomDuration] Attack", category: "Flow", id: "2af17d3c6cb974f7035263c4957114e4")]
public partial class RandomTimeout : Modifier
{
    [SerializeReference] public BlackboardVariable<float> RandomDuration;
    [CreateProperty] float m_Timer = 0.0f;

    protected override Status OnStart()
    {
        if (Child == null)
        {
            LogFailure("No child node to timeout for.");
            return Status.Failure;
        }

        m_Timer = UnityEngine.Random.Range(0f, RandomDuration.Value);
        if (m_Timer <= 0.0f)
        {
            LogFailure("Duration set to zero. Child was not executed.");
            return Status.Failure;
        }

        return StartNode(Child) switch
        {
            Status.Success => Status.Running,
            Status.Failure => Status.Failure,
            _ => Status.Running
        };
    }

    protected override Status OnUpdate()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0)
        {
            EndNode(Child);
            return Status.Failure;
        }

        return Child.CurrentStatus switch
        {
            Status.Success => Status.Running,
            Status.Failure => Status.Failure,
            _ => Status.Running
        };
    }
}

