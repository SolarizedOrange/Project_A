using System;
using UnityEngine;

[System.Serializable]
public class Buff<T> where T: Enum
{
	[SerializeField] T statType;
	public T StatType => statType;
    [SerializeField] float value;
	public float Value => value;
	[SerializeField] float duration;
	public float Duration => duration;
}
