using System.Collections;
using Unity.Behavior;
using UnityEngine;

public class EnemyController: CharacterBase
{
	readonly int SpeedHash = Animator.StringToHash("Speed");
	readonly int LastHitTypeHash = Animator.StringToHash("LastHitType");

	[Header("Enemy Controller")]
	[SerializeField] Collider characterCollider;
    public BehaviorGraphAgent Agent;
    public Vector3 Recoil;
	bool isDebuffApplied;
	protected override void Start()
	{
		base.Start();
		PlayerDamageRoutineSetup();
		EquipWeapon(GetComponentInChildren<WeaponBase>());

		foreach (var item in BulletAmmo.Values)
		{
			item.Value = 999999;
		}
	}

	void OnDestroy()
	{
		PlayerDamageRoutineCleanup();
	}

	void Update()
	{
		if (Agent.enabled == false)
		{
			if (Agent.BlackboardReference.GetVariable<CharacterBase>("Target", out var target) && target != null)
			{
				Agent.enabled = true;
				Agent.Restart();
			}
			else if (GameManager.Instance && GameManager.Instance.Player)
				Agent.BlackboardReference.SetVariableValue("Target", GameManager.Instance.Player);
		}

		SyncSpeedAnimator();
	}
	
	public override void OnDamage(HitBoxType hitBoxType, float damage)
	{
		HP.Value -= damage;

		Animator.SetInteger(LastHitTypeHash, (int)hitBoxType);
		
		if (HP.Value < 0.001f)
		{
			characterCollider.excludeLayers = (int)Layers.Player;
			Die();
			return;
		}


		if (isDebuffApplied) return;
		
		// Debuff
		if (hitBoxType == HitBoxType.Head)
		{
			SetEnemyAction(EnemyActionType.Hit);
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
	}

	public void SetEnemyAction(EnemyActionType actionType)
	{
		Agent.BlackboardReference.GetVariable<EnemyActionType>("CurrentAction", out var curAction);
		curAction.Value = actionType;
	}

	#region Player Damage Routine
	public bool IsPlayPlayerDamageRoutine = false;
	void PlayerDamageRoutineSetup()
	{
		PlayerDamage.EndureStartEvent.AddListener(OnPlayerDamageEndureStart);
        PlayerDamage.EndureEndEvent.AddListener(OnPlayerDamageEndureEnd);
	}

	void PlayerDamageRoutineCleanup()
	{
		PlayerDamage.EndureStartEvent.RemoveListener(OnPlayerDamageEndureStart);
		PlayerDamage.EndureEndEvent.RemoveListener(OnPlayerDamageEndureEnd);
	}

    void OnPlayerDamageEndureStart()
    {
        IsPlayPlayerDamageRoutine = true;
    }
    void OnPlayerDamageEndureEnd(bool success)
    {
        IsPlayPlayerDamageRoutine = false;
    }
	#endregion
}
