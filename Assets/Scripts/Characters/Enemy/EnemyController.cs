using Unity.Behavior;
using UnityEngine;

public class EnemyController: CharacterBase
{
	readonly int SpeedHash = Animator.StringToHash("Speed");
	readonly int IsEnterHash = Animator.StringToHash("IsEnter");

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
		Agent.BlackboardReference.GetVariable<EnemyActionType>("CurrentAction", out var curAction);
		curAction.Value = EnemyActionType.Hit;
	}

	void SyncSpeedAnimator()
	{
		Animator.SetFloat(SpeedHash, MoveCtrl.Ctrl.linearVelocity.magnitude);
		Animator.SetBool(IsEnterHash, Agent.BlackboardReference.GetVariable<bool>("IsEnter", out var isEnter) && isEnter.Value);
	}

	void SyncBlackboard()
	{
		// Update AttackDistance in Blackboard
		Agent.BlackboardReference.GetVariable<float>("AttackDistnace", out var range);
		if (CurrentWeapon != null)
			range.Value = CurrentWeapon.Stat.AttackRange.BaseVal;
		else
			range.Value = 0f;

		Agent.BlackboardReference.GetVariable<bool>("IsCover", out var isCover);
		IsCover = isCover.Value;
	}
}
