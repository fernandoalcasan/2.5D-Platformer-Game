using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script called after player executes the jump animation to play landing SFX (Method to play this sfx changed btw).
public class LandingBehavior : StateMachineBehaviour
{
    [SerializeField]
    private AudioClip _landClip;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.Instance.PlayOneShot(_landClip, 1f);
    }
}
