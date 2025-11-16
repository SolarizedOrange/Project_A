using UnityEngine;
using System.Collections.Generic;

public class PlayerStateMachine : MonoBehaviour
{

    public PlayerStateBase CurrentState;

    public List<PlayerStateBase> States;

    Dictionary<PlayerStateType, PlayerStateBase> StateDic;


    void Awake()
    {
        StateDic = new Dictionary<PlayerStateType, PlayerStateBase>();
        var ctrl = GetComponent<PlayerController>();

        foreach (var state in States)
        {
            StateDic.Add(state.GetStateType(), state);
            state.Init(ctrl);
        }
    }
    
    void Start()
    {
        CurrentState.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.OnUpdate();
    }

    public void ChangeState(PlayerStateType state)
    {
        CurrentState.OnExit();
        CurrentState = StateDic[state];
        CurrentState.OnEnter();
    }
}
