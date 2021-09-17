using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to play SFX via animation clip events for the player model on the main menu
public class Model : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _audioClips;

    private void PlaySFX(int clip)
    {
        AudioManager.Instance.PlayOneShot(_audioClips[clip], 1f);
    }
}
