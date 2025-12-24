using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Cover", story: "Agent [isCover] in [CoverObject]", category: "Action", id: "fe243adc61bdf0bf1602db6de6f3570d")]
public partial class CoverAction : Action
{
	readonly int IsEnterHash = UnityEngine.Animator.StringToHash("IsEnter");
	readonly int IsCoverLeftHash = UnityEngine.Animator.StringToHash("IsCoverLeft");
    [SerializeReference] public BlackboardVariable<bool> IsCover;
    [SerializeReference] public BlackboardVariable<bool> InEnter;
    [SerializeReference] public BlackboardVariable<CoverObject> CoverObject;
    [SerializeReference] public BlackboardVariable<EnemyController> Agent;
    [SerializeReference] public BlackboardVariable<Vector3> AgentReturnPos;
    [SerializeReference] public BlackboardVariable<Animator> Animator;

    MovementController ctrl;
    float timer;
    bool isMoveToCover;

    protected override Status OnStart()
    {
        // Debug.Log("CoverStart");
        isMoveToCover = false;
        ctrl = Agent.Value.MoveCtrl;
        timer = Mathf.Max(ctrl.PositionTransitionTime, ctrl.RotateTransitionTime);
        if (CoverObject.Value == null) return Status.Success;

        if (IsCover.Value)
		{
            EnterCover();
		}
        else if (InEnter.Value)
		{
			ExitCover();
		}
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (timer < 0 || isMoveToCover == false) 
        {
            if (InEnter.Value == false) 
                CoverObject.Value.ExitCover(Agent.Value);

            return Status.Success;
        }
        timer -= Time.deltaTime;
        return Status.Running;
    }

    void EnterCover()
    {
        if (CoverObject.Value.TryEnterCover(Agent.Value) == false) return;
        var transform = CoverObject.Value.transform;
        isMoveToCover = true;
        ctrl.SetTargetPositionXZ(transform.position,false);
        InEnter.Value = true;
        Animator.Value.SetBool(IsEnterHash, InEnter);
        Animator.Value.SetBool(IsCoverLeftHash, Vector3.Dot(transform.right,transform.position - Agent.Value.transform.position) > 0f );
        // Debug.Log($"Enter Cover c:{IsCover.Value}");
    }

    void ExitCover()
    {
        isMoveToCover = true;
        ctrl.SetTargetPositionXZ(AgentReturnPos.Value, false);
        InEnter.Value = false;
        Animator.Value.SetBool(IsEnterHash, InEnter);
        // Debug.Log($"Exit Cover c:{IsCover.Value}");
    }
}

