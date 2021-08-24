using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbUpBehavior : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.transform.parent.TryGetComponent(out Player player))
        {
            player.StandFromLedge();
        }
        else
            Debug.LogError("Player component is NULL");
    }
}
