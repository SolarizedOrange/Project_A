using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStat", menuName = "Character/CharacterStat")]
public class CharacterStat : ScriptableObject
{
	public StatBase Hp;
	public StatBase MoveSpeed;
	public StatBase Precision;
	// public List<Armor> Armors;
}

public class CharacterStatWrapper
{
	CharacterStat stat;
	CharacterBase owner;
	public CharacterStatWrapper(CharacterBase owner, CharacterStat stat)
	{
		this.owner = owner;
		this.stat = stat;
	}

	public float GetApplyBuffStatBase(CharacterStatType statType, float buffMul)
	{
		StatBase statBase;
		switch (statType)
        {
            case CharacterStatType.HP:
                statBase = stat.Hp;
                break;
            case CharacterStatType.MoveSpeed:
                statBase = stat.MoveSpeed;
                break;
            case CharacterStatType.Precision:
                statBase = stat.Precision;
                break;
            default:
                return 0f;
        }

        if (statBase.IsFloating)
		{
			return Mathf.Clamp(statBase.BaseVal * buffMul, statBase.MinVal, statBase.MaxVal);
		}
        else
		{
			return (int)Mathf.Clamp(statBase.BaseVal * buffMul, statBase.MinVal, statBase.MaxVal);
		}
	}

	public float Hp
    {
        get
        {
            return GetApplyBuffStatBase(
                CharacterStatType.HP,
                owner.CharacterBuff.GetBuffMul(CharacterStatType.HP)
            );
        }
    }

    public float MoveSpeed
    {
        get
        {
            return GetApplyBuffStatBase(
                CharacterStatType.MoveSpeed,
                owner.CharacterBuff.GetBuffMul(CharacterStatType.MoveSpeed)
            );
        }
    }

    public float Precision
    {
        get
        {
            return GetApplyBuffStatBase(
                CharacterStatType.Precision,
                owner.CharacterBuff.GetBuffMul(CharacterStatType.Precision)
            );
        }
    }

}
