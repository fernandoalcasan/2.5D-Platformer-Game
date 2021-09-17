using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script called after player executes the climb up animation (Ladder) to change position due to animation baking.
public class StandLadderBehavior : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent(out Player player))
        {
            player.StandFromLadder();
        }
        else
            Debug.LogError("Player component is NULL");
    }
}
