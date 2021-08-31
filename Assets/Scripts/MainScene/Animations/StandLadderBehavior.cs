using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandLadderBehavior : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.transform.parent.TryGetComponent(out Player player))
        {
            player.StandFromLadder();
        }
        else
            Debug.LogError("Player component is NULL");
    }
}
