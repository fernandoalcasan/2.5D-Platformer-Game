using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStateBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("NextState", Random.Range(0, 2));
    }
}
