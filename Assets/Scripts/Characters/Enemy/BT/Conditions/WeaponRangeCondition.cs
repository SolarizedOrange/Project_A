using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "WeaponRangeCondition", story: "distance between [SelfEnemy] and [Target] [Operator] WeaponRange", category: "Conditions", id: "2b0cb1b7d3bc4c9ad1834f72e300220f")]
public partial class WeaponRangeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyController> SelfEnemy;
    [SerializeReference] public BlackboardVariable<CharacterBase> Target;
    [Comparison(comparisonType: ComparisonType.All)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> Operator;

    public override bool IsTrue()
    {
        if (SelfEnemy.Value == null || Target.Value == null)
        {
            return false;
        }

        float distance = Vector3.Distance(SelfEnemy.Value.transform.position, Target.Value.transform.position);
        return ConditionUtils.Evaluate(distance, Operator, SelfEnemy.Value.CurrentWeapon ? SelfEnemy.Value.CurrentWeapon.Stat.AttackRange:2f);
    }
}
