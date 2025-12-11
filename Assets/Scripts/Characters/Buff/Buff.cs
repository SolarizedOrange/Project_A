using System;
using UnityEngine;

[Serializable]
public class Buff
{
	public virtual Enum StatType { get; } 
    [SerializeField] float value;
	public float Value => value;
	[SerializeField] float duration;
	public float Duration => duration;
}

[Serializable]
public class CharacterBuff: Buff
{
	[SerializeField] CharacterStatType statType;
	public override Enum StatType => statType;
}

[Serializable]
public class WeaponBuff: Buff
{
	[SerializeField] WeaponType weaponType;
	public WeaponType WeaponType => weaponType;

	[SerializeField] WeaponStatType statType;
	public override Enum StatType => statType;
}