using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerkItem",menuName ="Shop/PerkItem")]
public class PerkItem : ItemBase
{
	[Serializable]
	public class PerkItemContainer
	{
		public bool IsCharacterBuff;
		[SerializeReference] public Buff Buff;

		public PerkItemContainer()
		{
			IsCharacterBuff = false;
		}
	}
	public PerkGroup PerkGroup;
	public List<PerkItemContainer> BuffList;	

	void OnValidate()
	{
		for (int i = 0; i < BuffList.Count; i++)
		{
			var container = BuffList[i];
			if (container.IsCharacterBuff && container.Buff is not CharacterBuff)
			{
				container.Buff = new CharacterBuff();
			}
			else if (container.IsCharacterBuff == false && container.Buff is not WeaponBuff)
			{
				container.Buff = new WeaponBuff();
			}
		}
	}
}