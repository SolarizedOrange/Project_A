using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CalcDistance", story: "Check [AlertDistnace] [Self] between [Target]", category: "Action", id: "66fec97cbc730f5444a7cbfd92523c2b")]
public partial class CalcDistanceAction : Action
{
    [SerializeReference] public BlackboardVariable<float> AlertDistnace;
    [SerializeReference] public BlackboardVariable<EnemyController> Self;
    [SerializeReference] public BlackboardVariable<CharacterBase> Target;

    protected override Status OnStart()
    {
        if(Self.Value.CurrentWeapon == null) return Status.Failure;
        
        if (Vector3.Dot(Target.Value.transform.position - Self.Value.transform.position,
            Self.Value.transform.forward) > 0)
        {
            var distance = Vector3.Distance(Self.Value.transform.position, Target.Value.transform.position);
            var weaponDistance = 0f;
            if (Self.Value.CurrentWeapon != null)
            {
                weaponDistance = Self.Value.CurrentWeapon.Stat.AttackRange;
            }
            
            if (distance < weaponDistance + AlertDistnace.Value)
                return Status.Success;
            else
                return Status.Failure;
        }
        else
            return Status.Failure;
    }
}

