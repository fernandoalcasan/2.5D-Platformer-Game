using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    public static Action OnMissionDisplayed;
    public static Action OnGamePause;

    [SerializeField]
    private AudioClip _missionAudio;
    [SerializeField]
    private Text _collectablesText;
    [SerializeField]
    private Text _livesTxt;
    [SerializeField]
    private Canvas _pauseCanvas;
    [SerializeField]
    private AudioClip _pauseAudio;
    [SerializeField]
    private Canvas _resultCanvas;
    [SerializeField]
    private Text _resultText;
    [SerializeField]
    private AudioClip _successAudio;
    [SerializeField]
    private AudioClip _failAudio;

    private void Start()
    {
        Cursor.visible = false;
        Collectable.OnCollection += UpdateCollectables;
        DeadZone.OnPlayerFall += UpdateLives;
        Player.OnMissionEnd += DisplayResultMenu;
    }

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

    private void UpdateCollectables()
    {
        _collectablesText.text = Player.Power + "/20";
    }

    private void UpdateLives()
    {
        _livesTxt.text = Player.Lives + "/3";
    }

    private void MissionAlert()
    {
        AudioManager.Instance.PlayOneShotHalfVolume(_missionAudio);
    }

    private void MissionDisplayed()
    {
        if (!(OnMissionDisplayed is null))
            OnMissionDisplayed();
    }

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

    public void ResumeGame()
    {
        AudioListener.pause = false;
        _pauseCanvas.enabled = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void RestartLevel()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void GoToMainMenu()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        Collectable.OnCollection -= UpdateCollectables;
        DeadZone.OnPlayerFall -= UpdateLives;
        Player.OnMissionEnd -= DisplayResultMenu;
    }
}
