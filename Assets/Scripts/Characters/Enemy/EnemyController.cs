using System.Collections;
using Unity.Behavior;
using UnityEngine;

public class EnemyController: CharacterBase
{
	readonly int SpeedHash = Animator.StringToHash("Speed");
	readonly int IsEnterHash = Animator.StringToHash("IsEnter");
	readonly int LastHitTypeHash = Animator.StringToHash("LastHitType");

	[Header("Enemy Controller")]
	[SerializeField] Collider characterCollider;
    public BehaviorGraphAgent Agent;
    public Vector3 Recoil;
	bool isDebuffApplied;
	void Start()
	{
		foreach (var item in BulletAmmo.Values)
		{
			item.Value = 999999;
		}
	}
	void Update()
	{
		SyncSpeedAnimator();
		SyncBlackboard();
	}

	public override void OnDamage(HitBoxType hitBoxType, float damage)
	{
		base.OnDamage(hitBoxType, damage);

		Animator.SetInteger(LastHitTypeHash, (int)hitBoxType);
		
		if (HP.Value < 0.001f)
		{
			characterCollider.excludeLayers = (int)Layers.Player;
			return;
		}


		if (isDebuffApplied) return;
		
		// Debuff
		if (hitBoxType == HitBoxType.Head)
		{
			Agent.BlackboardReference.GetVariable<EnemyActionType>("CurrentAction", out var curAction);
			curAction.Value = EnemyActionType.Hit;
		}
		else if (hitBoxType == HitBoxType.Leg)
		{
			var debuff = new CharacterBuff();
			debuff.Value = 0.5f;
			debuff.Duration = 5f;
			debuff.StatType = CharacterStatType.MoveSpeed;
			CharacterBuff.AddBuff(debuff);
		}
		StartCoroutine(HitRoutine());

		isDebuffApplied = true;

	}

	IEnumerator HitRoutine()
	{
		yield return new WaitForSeconds(5f);
		isDebuffApplied = false;
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
			range.Value = CurrentWeapon.Stat.AttackRange;
		else
			range.Value = 0f;

		Agent.BlackboardReference.GetVariable<bool>("IsCover", out var isCover);
		IsCover = isCover.Value;
	}
}
