using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script called after player executes the climb up animation (Ledge) to change position due to animation baking.
public class ClimbUpBehavior : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent(out Player player))
        {
            player.StandFromLedge();
        }
        else
            Debug.LogError("Player component is NULL");
    }
}
