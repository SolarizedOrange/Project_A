using Unity.Behavior;
using UnityEngine;

public class EnemyController: CharacterBase
{
	[Header("Enemy Controller")]
    public BehaviorGraphAgent Agent;

	void Update()
	{
		// Update AttackDistance in Blackboard
		Agent.BlackboardReference.GetVariable<float>("AttackDistnace", out var range);
		if (CurrentWeapon != null)
			range.Value = CurrentWeapon.Stat.AttackRange.Val;
		else
			range.Value = 0f;
	}

	public override void OnDamage(HitBoxType hitBoxType)
	{
		base.OnDamage(hitBoxType);
		Agent.BlackboardReference.GetVariable<bool>("IsHit", out var isHit);
		isHit.Value = true;
	}
}
