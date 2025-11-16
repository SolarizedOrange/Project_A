using UnityEngine;

[System.Serializable]
public class WeaponStat
{
    public StatBase<float> Accuracy;
    public StatBase<float> AttackRate;// = new();
    public StatBase<int> Capacity;
    public StatBase<float> Damage;
    public StatBase<int> ShotCount;
    public StatBase<float> AttackRange;
}
