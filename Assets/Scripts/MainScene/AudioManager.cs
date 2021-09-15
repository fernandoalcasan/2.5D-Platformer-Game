using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogError("Audio Manager instance is NULL");
            return _instance;
        }
    }

    [SerializeField]
    private AudioSource _sfxAudioSource;
    private AudioSource _musicAudioSource;

    private void Awake()
    {
        _instance = this;
        _sfxAudioSource.ignoreListenerPause = true;
        _musicAudioSource = GetComponent<AudioSource>();

        if (_musicAudioSource is null)
            Debug.LogError("Music audio source is NULL");
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        _sfxAudioSource.PlayOneShot(clip, volume);
    }

    public void PlayOneShotFullVolume(AudioClip clip)
    {
        _sfxAudioSource.PlayOneShot(clip, 1f);
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicAudioSource.clip = clip;
        _musicAudioSource.Play();
    }
}
