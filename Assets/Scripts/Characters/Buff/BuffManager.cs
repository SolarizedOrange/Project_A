using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{
    Dictionary<Enum,(HashSet<Buff> hashSet,float buffMul)> buffDic;

	Coroutine buffRoutine;
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
		if (buff.Duration > 0f)
			buffRoutine = GameManager.Instance.StartCoroutine(BuffRoutine(buff));
        UpdateBuff(statType);
	}

    public void RemoveBuff(Buff buff)
	{
        var statType = buff.StatType;
        if (buffDic.TryGetValue(statType,out var tuple)
			&& tuple.hashSet.Remove(buff))
		{
			if (buffRoutine != null) GameManager.Instance.StopCoroutine(buffRoutine);
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
		buffDic[statType] = tuple;
		Debug.Log(tuple.buffMul);
	}

    public float GetBuffMul(Enum statType)
	{
        if (buffDic.TryGetValue(statType, out var tuple))
		    return tuple.buffMul;
        return 1f;
	}
	IEnumerator BuffRoutine(Buff buff)
	{
		yield return new WaitForSeconds(buff.Duration);
		buffRoutine = null;
		RemoveBuff(buff);
	}
}
