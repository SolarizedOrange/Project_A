using UnityEngine;

public class SetLayerWeight : StateMachineBehaviour
{
   [SerializeField] float startWeight = 1f;
   [SerializeField] float exitWeight = 1f;
   [SerializeField] bool isExitMode = true;

   // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      animator.SetLayerWeight(layerIndex,startWeight);
   }

   // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      if (isExitMode == false && stateInfo.normalizedTime >= 1f)
      {
         animator.SetLayerWeight(layerIndex,exitWeight);
      }
   }

   // OnStateExit is called before OnStateExit is called on any state inside this state machine
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      if (isExitMode)
		{
         animator.SetLayerWeight(layerIndex,exitWeight);
		}
   }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
