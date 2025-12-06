using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStat", menuName = "Character/CharacterStat")]
public class CharacterStat : ScriptableObject, IStatField<CharacterStatType>
{
	public StatBase Hp;
	public StatBase MoveSpeed;
	public StatBase Precision;

	public float GetApplyBuffStatBase(CharacterStatType statType, float buffMul)
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

	// public List<Armor> Armors;

	public StatBase GetStatBase(CharacterStatType statType)
	{
		switch (statType)
        {
            case CharacterStatType.HP:
                return Hp;
            case CharacterStatType.MoveSpeed:
                return MoveSpeed;
            case CharacterStatType.Precision:
                return Precision;
            default:
                return null;
        }
	}
}
