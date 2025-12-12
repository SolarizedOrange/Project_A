using System;
using UnityEngine;

[Serializable]
public class Buff
{
	public virtual Enum StatType { get; set;} 
    [SerializeField] float value;
	public float Value { get => value; set => this.value = value; }
	[SerializeField] float duration;
	public float Duration { get => duration; set => duration = value; }
}

[Serializable]
public class CharacterBuff: Buff
{
	[SerializeField] CharacterStatType statType;
	public override Enum StatType { get => statType; set => statType = (CharacterStatType)value; }
}

[Serializable]
public class WeaponBuff: Buff
{
	[SerializeField] WeaponType weaponType;
	public WeaponType WeaponType { get => weaponType; set => weaponType = value; }

	[SerializeField] WeaponStatType statType;
	public override Enum StatType { get => statType; set => statType = (WeaponStatType)value; }
}