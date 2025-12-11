using System;
using System.Collections.Generic;

public class BuffManager
{
    Dictionary<Enum,(HashSet<Buff> hashSet,float buffMul)> buffDic;

	public BuffManager()
	{
        buffDic = new ();
	}

	public void AddBuff(Buff buff)
	{
        var statType = buff.StatType;
		if (buffDic.ContainsKey(statType) == false)
			buffDic.Add(statType, (new (),1f));

		buffDic[statType].hashSet.Add(buff);
        UpdateBuff(statType);
	}

    public void RemoveBuff(Buff buff)
	{
        var statType = buff.StatType;
        if (buffDic.TryGetValue(statType,out var tuple)
			&& tuple.hashSet.Remove(buff))
		{
			UpdateBuff(statType);
		} 
	}

    void UpdateBuff(Enum statType)
	{
		var tuple = buffDic[statType];
		var updateVal = 1f;
        foreach (var buff in tuple.hashSet)
        {
            updateVal *= buff.Value;
        }
        tuple.buffMul = updateVal;
	}

    public float GetBuffMul(Enum statType)
	{
        if (buffDic.TryGetValue(statType, out var tuple))
		    return tuple.buffMul;
        return 1f;
	}
}
