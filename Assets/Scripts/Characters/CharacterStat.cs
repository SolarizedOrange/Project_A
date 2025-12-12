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

	public float Hp
    {
        get { return Mathf.Clamp(stat.Hp.BaseVal * owner.CharacterBuff.GetBuffMul(CharacterStatType.HP), stat.Hp.MinVal, stat.Hp.MaxVal); }
    }

    public float MoveSpeed
    {
        get { return Mathf.Clamp(stat.MoveSpeed.BaseVal * owner.CharacterBuff.GetBuffMul(CharacterStatType.MoveSpeed), stat.MoveSpeed.MinVal, stat.MoveSpeed.MaxVal); }
    }

    public float Precision
    {
        get { return Mathf.Clamp(stat.Precision.BaseVal * owner.CharacterBuff.GetBuffMul(CharacterStatType.Precision), stat.Precision.MinVal, stat.Precision.MaxVal); }
    }

}
