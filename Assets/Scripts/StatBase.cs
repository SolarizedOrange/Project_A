using UnityEngine;

[System.Serializable]
public class StatBase<T>
{
    [SerializeField] T baseVal;
    public T BaseVal
    {
        get
        {
            return baseVal;
        }
    }

    [SerializeField] T minVal;
    public T MinVal
    {
        get
        {
            return minVal;
        }    
    }

    [SerializeField] T maxVal;
    public T MaxVal
    {
        get
        {
            return maxVal;
        }
    }
}
