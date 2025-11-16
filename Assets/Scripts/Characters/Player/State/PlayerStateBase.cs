using UnityEngine;

[System.Serializable]
public class PlayerStateBase: ScriptableObject
{
    public PlayerController PlayerCtrl;

    public void Init(PlayerController ctrl)
    {
        PlayerCtrl = ctrl;
    }
    public virtual void OnEnter() { throw new System.NotImplementedException(); }
    public virtual void OnUpdate() { throw new System.NotImplementedException(); }
    public virtual void OnExit() { throw new System.NotImplementedException(); }
    public virtual PlayerStateType GetStateType() { throw new System.NotImplementedException(); }
}
