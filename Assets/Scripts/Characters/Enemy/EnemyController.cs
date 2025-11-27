using Unity.Behavior;
using UnityEngine;

public class EnemyController: CharacterBase
{
	readonly int SpeedHash = Animator.StringToHash("Speed");

	[Header("Enemy Controller")]
    public BehaviorGraphAgent Agent;

	void Update()
	{
		SyncSpeedAnimator();
		SyncBlackboard();
	}

	public override void OnDamage(HitBoxType hitBoxType)
	{
		base.OnDamage(hitBoxType);
		Agent.BlackboardReference.GetVariable<bool>("IsHit", out var isHit);
		isHit.Value = true;
	}

	void SyncSpeedAnimator()
	{
		Animator.SetFloat(SpeedHash, MoveCtrl.Ctrl.linearVelocity.magnitude);
	}

	void SyncBlackboard()
	{
		// Update AttackDistance in Blackboard
		Agent.BlackboardReference.GetVariable<float>("AttackDistnace", out var range);
		if (CurrentWeapon != null)
			range.Value = CurrentWeapon.Stat.AttackRange.Val;
		else
			range.Value = 0f;

		Agent.BlackboardReference.GetVariable<bool>("IsCover", out var isCover);
		IsCover = isCover.Value;
	}
}
