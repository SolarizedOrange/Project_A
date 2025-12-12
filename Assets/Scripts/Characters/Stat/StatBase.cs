using UnityEngine;

[System.Serializable]
public class StatBase
{
    [Header("Stat")]
    [SerializeField] float baseVal;
    public float BaseVal
    {
        get
        {
            return baseVal;
        }
    }

    [SerializeField] float minVal;
    public float MinVal
    {
        get
        {
            return minVal;
        }    
    }

    [SerializeField] float maxVal;
    public float MaxVal
    {
        get
        {
            return maxVal;
        }
    }
}
