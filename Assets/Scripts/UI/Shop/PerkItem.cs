using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatPerk",menuName ="Perk/CharacterStatPerk")]
public class CharacterStatPerkItem : ItemBase
{
	public List<Buff<CharacterStatType>> BuffList;	
}
[CreateAssetMenu(fileName = "WeaponStatPerk",menuName ="Perk/WeaponStatPerk")]
public class WeaponStatPerkItem : ItemBase
{
	[Serializable]
	public struct WeaponPerkContainer
	{
		public WeaponType WeaponType;
		public Buff<WeaponStatType> Buff;
	}

	public List<WeaponPerkContainer> BuffList;
}