using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Composite = Unity.Behavior.Composite;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WeightRandom", story: "[RandomWeightList]", category: "Flow", id: "cf7ddde7e76e100370730442333d394b")]
public partial class WeightRandomSequence : Composite
{
    [SerializeReference] public BlackboardVariable<List<float>> RandomWeightList;
    int randomIndex = 0;

    protected override Status OnStart()
    {
        var pick = UnityEngine.Random.Range(0f, 1f);
        var sum = 0f;
        var list = RandomWeightList.Value;
        for (int i = 0; i< list.Count; i++)
        {
            sum += list[i];
            if (pick < sum) 
            { 
                randomIndex = i;
                break;
            }
        }
        
        if (randomIndex < Children.Count)
        {
            var status = StartNode(Children[randomIndex]);
            if (status == Status.Success || status == Status.Failure)
                return status;

            return Status.Waiting;
        }

        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        var status = Children[randomIndex].CurrentStatus;
        if (status == Status.Success || status == Status.Failure)
            return status;

        return Status.Waiting;
    }
}

