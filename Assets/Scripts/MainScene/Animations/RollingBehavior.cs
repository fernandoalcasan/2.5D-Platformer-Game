using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script called after player executes the rolling animation to reset X velocity.
public class RollingBehavior : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent(out Player player))
        {
            player.StopRolling();
        }
        else
            Debug.LogError("Player component is NULL");
    }
}
