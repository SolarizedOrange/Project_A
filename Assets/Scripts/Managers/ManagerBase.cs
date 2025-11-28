using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    public bool DontDestroyOnLoadCheck = false;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            if (DontDestroyOnLoadCheck)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
