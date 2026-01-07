using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ApplyPlayerState", story: "[Agent] Apply [Player] (target) state", category: "Action", id: "4b1af0102c7681fb00bed79ec5d34ff2")]
public partial class ApplyPlayerStateAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<PlayerController> Player;
    [SerializeReference] public BlackboardVariable<float> DebuffAttackRate;
    [SerializeReference] public BlackboardVariable<EnemyActionType> ActionType;

    List<WeaponBuff> buffs = new ();
    protected override Status OnStart()
    {
        UpdatePlayerDamageState();
        return ActionType.Value != EnemyActionType.MeleeTargeted ? Status.Success : Status.Failure;
    }

    void UpdatePlayerDamageState()
    {
        var enemy = Agent.Value;
        // Enemy Attack Rate Debuff during Player Damage Routine
        if (enemy.IsPlayPlayerDamageRoutine && buffs.Count < 1)
        {
            foreach (var keyValue in enemy.WeaponBuff)
            {
                var buff = new WeaponBuff()
                {
                    WeaponType = keyValue.Key,
                    StatType = WeaponStatType.AttackRate,
                    Value = DebuffAttackRate.Value,
                    Duration = -1f
                };
                buffs.Add(buff);
                keyValue.Value.AddBuff(buff);
            }
        }
        else if (!enemy.IsPlayPlayerDamageRoutine && buffs.Count > 0)
        {
            foreach (var buff in buffs)
            {
                if (enemy.WeaponBuff.TryGetValue(buff.WeaponType, out var weaponBuff))
                    weaponBuff.RemoveBuff(buff);
            }
            buffs.Clear();
        };
    }
}

