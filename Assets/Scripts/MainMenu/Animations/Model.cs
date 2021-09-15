using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _audioClips;

    private void PlaySFX(int clip)
    {
        AudioManager.Instance.PlayOneShot(_audioClips[clip], 1f);
    }
}
