using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to choose a random animation for the player model on the main menu
public class NextStateBehavior : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("NextState", Random.Range(0, 2));
    }
}
