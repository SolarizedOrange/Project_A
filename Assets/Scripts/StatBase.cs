using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatBase<T>
{
    [SerializeField] private T val;
    public T Val
    { 
        get
        {
            return val;
        }

        set
        {
            val = value;
            OnChangedValue?.Invoke(val);
        }
    }
    public T MaxVal;
    // public Dictionary<BuffType, Buff> BuffMulDic;
    public float BuffMul;

    public UnityEvent<T> OnChangedValue;
}
