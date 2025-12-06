using System;
using System.Collections.Generic;

public class BuffManager<T> where T:Enum
{
    Dictionary<T,HashSet<Buff<T>>> buffDic;
    Dictionary<T,float> buffMulDic;

	public BuffManager()
	{
        buffDic = new ();
        buffMulDic = new ();
		foreach(T statType in Enum.GetValues(typeof(T)))
		{
			buffDic.Add(statType,new HashSet<Buff<T>>());
            buffMulDic.Add(statType, 1f);
		}
	}

	public void AddBuff(Buff<T> buff)
	{
		buffDic[buff.StatType].Add(buff);
        UpdateBuff(buff.StatType);
	}

    public void RemoveBuff(Buff<T> buff)
	{
        var statType = buff.StatType;
        if (buffDic[statType].Remove(buff))
		{
			UpdateBuff(statType);
		} 
	}

    void UpdateBuff(T statType)
	{
		var updateVal = 1f;
        foreach (var buff in buffDic[statType])
        {
            updateVal *= buff.Value;
        }
        buffMulDic[statType] = updateVal;
	}

    public float GetBuffMul(T statType)
	{
        if (buffMulDic.TryGetValue(statType, out var buffMul))
		    return buffMul;
        return 1f;
	}
}
