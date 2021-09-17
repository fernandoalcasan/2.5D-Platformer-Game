using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton class to manage the music and SFX from the game.
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

    //Singleton instance and variable initialization
    private void Awake()
    {
        _instance = this;
        _sfxAudioSource.ignoreListenerPause = true;
        _musicAudioSource = GetComponent<AudioSource>();

        if (_musicAudioSource is null)
            Debug.LogError("Music audio source is NULL");
    }

    //Method to play SFX with custom volume
    public void PlayOneShot(AudioClip clip, float volume)
    {
        _sfxAudioSource.PlayOneShot(clip, volume);
    }

    //Method to play SFX with full volume (used for UI OnClick Events)
    public void PlayOneShotFullVolume(AudioClip clip)
    {
        _sfxAudioSource.PlayOneShot(clip, 1f);
    }

    //Method to play SFX with half volume (used for UI OnClick Events)
    public void PlayOneShotHalfVolume(AudioClip clip)
    {
        _sfxAudioSource.PlayOneShot(clip, .5f);
    }

    //Method to play background music (currently unused)
    public void PlayMusic(AudioClip clip)
    {
        _musicAudioSource.clip = clip;
        _musicAudioSource.Play();
    }
}
