using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStat", menuName = "Weapon/WeaponStat")]
public class WeaponStat : ScriptableObject, IStatField<WeaponStatType>
{
	public StatBase Accuracy;
	public StatBase AttackRate;
	public StatBase Capacity;
	public StatBase Damage;
	public StatBase ShotCount;
	public StatBase AttackRange;
	public StatBase Recoil;

	public float GetApplyBuffStatBase(WeaponStatType statType, float buffMul)
	{
		var statBase = GetStatBase(statType);
        if (statBase.IsFloating)
		{
			return Mathf.Clamp(statBase.BaseVal * buffMul, statBase.MinVal, statBase.MaxVal);
		}
        else
		{
			return (int)Mathf.Clamp(statBase.BaseVal * buffMul, statBase.MinVal, statBase.MaxVal);
		}
	}

	// public int Get(StatBase<int> val)
	// {
	//     return Mathf.Clamp(val.BaseVal, val.MinVal, val.MaxVal);
	// }

	// public float Get(StatBase<float> val)
	// {
	//     return Mathf.Clamp(val.BaseVal, val.MinVal, val.MaxVal);
	// }

	public StatBase GetStatBase(WeaponStatType statType)
	{
		switch (statType)
        {
            case WeaponStatType.Accuracy:
                return Accuracy;
            case WeaponStatType.AttackRate:
                return AttackRate;
            case WeaponStatType.Capacity:
                return Capacity;
            case WeaponStatType.Damage:
                return Damage;
            case WeaponStatType.ShotCount:
                return ShotCount;
            case WeaponStatType.AttackRange:
                return ShotCount;
			case WeaponStatType.Recoil:
				return Recoil;
            default:
                return null;
        }
	}
}

// public class BuffMul
// {
//     public float Mul = 1.0f;
//     List<float> packed;
//     Dictionary<string, int> sparse;
//     Dictionary<int, string> invertedSparse;
//     public void Insert(string name, float mul)
//     {
//         sparse[name] = packed.Count;
//         invertedSparse[packed.Count] = name;
//         packed.Add(mul);
//     }

//     public void Remove(string name)
//     {
//         if (sparse[name] != packed.Count - 1)
//         {
//             sparse[invertedSparse[packed.Count - 1]] = sparse[name];
//             invertedSparse[sparse[name]] = invertedSparse[packed.Count - 1];
//             packed[sparse[name]] = packed[packed.Count - 1];
//         }
//         packed.RemoveAt(packed.Count - 1);
//         for (int i = sparse[name]; i < packed.Count; i++)
// 		{
			
// 		}
//     }
// }
// public class WeaponStatBuff
// {
//     public WeaponStat Stat;
//     float accuracy;
//     public float Accuracy{
//         get
//         {
//             return Mathf.Clamp(accuracy * Stat.Accuracy.BaseVal, Stat.Accuracy.MinVal, Stat.Accuracy.MaxVal);
//         }
//     }
// }
