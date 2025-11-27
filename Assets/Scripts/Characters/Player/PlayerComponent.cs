using UnityEngine;

public class PlayerComponent: MonoBehaviour
{
    protected PlayerController PlayerCtrl;
    void Awake()
    {
        PlayerCtrl = GetComponent<PlayerController>();
    }
}
