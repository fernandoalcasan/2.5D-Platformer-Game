using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//Class to manage the UI
public class UIManager : MonoBehaviour
{
    //Action delegates to indicate when mission has been displayed and game has been paused
    public static Action OnMissionDisplayed;
    public static Action OnGamePause;

    [Header("UI refs")]
    [SerializeField]
    private Text _collectablesText;
    [SerializeField]
    private Text _livesTxt;
    [SerializeField]
    private Canvas _pauseCanvas;
    [SerializeField]
    private Canvas _resultCanvas;
    [SerializeField]
    private Text _resultText;
    
    [Header("UI SFX")]
    [SerializeField]
    private AudioClip _missionAudio;
    [SerializeField]
    private AudioClip _pauseAudio;
    [SerializeField]
    private AudioClip _successAudio;
    [SerializeField]
    private AudioClip _failAudio;

    //Method to subscribe to the respective action delegates and hide cursor
    private void Start()
    {
        Cursor.visible = false;
        Collectable.OnCollection += UpdateCollectables;
        DeadZone.OnPlayerFall += UpdateLives;
        Player.OnMissionEnd += DisplayResultMenu;
    }

    //Method to chek for player's input to pause the game
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!_pauseCanvas.enabled)
                PauseGame();
            else
                ResumeGame();
        }
    }

    //Method to update the collectables UI text
    private void UpdateCollectables()
    {
        _collectablesText.text = Player.Power + "/20";
    }

    //Method to update the lives UI text
    private void UpdateLives()
    {
        _livesTxt.text = Player.Lives + "/3";
    }

    //Method called from UI animation to play SFX of mission
    private void MissionAlert()
    {
        AudioManager.Instance.PlayOneShotHalfVolume(_missionAudio);
    }

    //Method called from UI animation to indicate that mission was displayed
    private void MissionDisplayed()
    {
        if (!(OnMissionDisplayed is null))
            OnMissionDisplayed();
    }

    //Method to display the respective UI when player dies or wins the game
    private void DisplayResultMenu(bool isWin)
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        AudioListener.pause = true;

        if (isWin)
        {
            _resultText.text = "Mission\nComplete";
            AudioManager.Instance.PlayOneShotFullVolume(_successAudio);
        }
        else
        {
            _resultText.text = "Mission\nFailed";
            AudioManager.Instance.PlayOneShotFullVolume(_failAudio);
        }
        _resultCanvas.enabled = true;
    }

    //Method to pause the game and handle UI, cursor, time and audio.
    private void PauseGame()
    {
        if (_resultCanvas.enabled)
            return;

        AudioListener.pause = true;
        AudioManager.Instance.PlayOneShotHalfVolume(_pauseAudio);

        if (!(OnGamePause is null))
            OnGamePause();

        Time.timeScale = 0f;
        _pauseCanvas.enabled = true;
        Cursor.visible = true;
    }

    //Method to resume the game and handle UI, cursor, time and audio.
    public void ResumeGame()
    {
        AudioListener.pause = false;
        _pauseCanvas.enabled = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    //Method to restart the game and handle time and audio.
    public void RestartLevel()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    //Method to load main menu and handle time and audio.
    public void GoToMainMenu()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    //Unsubscribe from Action delegates as best practice
    private void OnDisable()
    {
        Collectable.OnCollection -= UpdateCollectables;
        DeadZone.OnPlayerFall -= UpdateLives;
        Player.OnMissionEnd -= DisplayResultMenu;
    }
}
