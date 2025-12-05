using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStat",menuName ="Weapon/WeaponStat")]
public class WeaponStat: ScriptableObject
{
    public StatBase<float> Accuracy;
    public StatBase<float> AttackRate;
    public StatBase<int> Capacity;
    public StatBase<float> Damage;
    public StatBase<int> ShotCount;
    public StatBase<float> AttackRange;

    // public int Get(StatBase<int> val)
    // {
    //     return Mathf.Clamp(val.BaseVal, val.MinVal, val.MaxVal);
    // }

    // public float Get(StatBase<float> val)
    // {
    //     return Mathf.Clamp(val.BaseVal, val.MinVal, val.MaxVal);
    // }
}

public class BuffMul
{
    public float Mul = 1.0f;
    List<float> packed;
    Dictionary<string, int> sparse;
    Dictionary<int, string> invertedSparse;
    public void Insert(string name, float mul)
    {
        sparse[name] = packed.Count;
        invertedSparse[packed.Count] = name;
        packed.Add(mul);
    }

    public void Remove(string name)
    {
        if (sparse[name] != packed.Count - 1)
        {
            sparse[invertedSparse[packed.Count - 1]] = sparse[name];
            invertedSparse[sparse[name]] = invertedSparse[packed.Count - 1];
            packed[sparse[name]] = packed[packed.Count - 1];
        }
        packed.RemoveAt(packed.Count - 1);
        for (int i = sparse[name]; i < packed.Count; i++)
        {
            
        }
    }
}
public class WeaponStatBuff
{
    public WeaponStat Stat;
    float accuracy;
    public float Accuracy{
        get
        {
            return Mathf.Clamp(accuracy * Stat.Accuracy.BaseVal, Stat.Accuracy.MinVal, Stat.Accuracy.MaxVal);
        }
    }
}
