using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script to handle the main menu mechanics (UI, lighting, model animations)
public class MainMenu : MonoBehaviour
{
    [Header("Lights on Scene")]
    [SerializeField]
    private Light[] _lights;
    private Color _lightsColor;
    private Renderer _lightsRenderer;
    private Color _lightsEmissionColor;

    //Time to wait before executing start or exit from game
    [SerializeField]
    private float _timeAfterBtnPressed;
    [SerializeField]
    private int _timesLightChange;
    private WaitForSeconds _wait;
    private AsyncOperation _asyncLoad;
    
    [Header("Humanoid Animator")]
    [SerializeField]
    private Animator _modelAnimator;  
    
    [Header("Credits")]
    [SerializeField]
    private Canvas _creditsCanvas;
    private CanvasScaler _creditsScaler;

    //Initialize variables
    private void Awake()
    {
        _wait = new WaitForSeconds(_timeAfterBtnPressed / _timesLightChange);

        _creditsScaler = _creditsCanvas.GetComponent<CanvasScaler>();

        if (_creditsScaler is null)
            Debug.LogError("Canvas Scaler is NULL");

        if (_lights.Length < 1)
        {
            Debug.LogError("Please add the respective lights through the inspector");
            return;
        }

        _lightsColor = _lights[0].color;
        _lightsRenderer = _lights[0].transform.parent.GetComponent<Renderer>();

        if (_lightsRenderer is null)
            Debug.LogError("Renderer in parent light is NULL");

        _lightsEmissionColor = _lightsRenderer.sharedMaterial.GetColor("_EmissionColor");
    }

    //Method called from UI to start the game using a coroutine
    public void StartGame()
    {
        StartCoroutine(StartOrExitCoroutine(true));
    }

    //Method called from UI to exit the game using a coroutine
    public void ExitGame()
    {
        StartCoroutine(StartOrExitCoroutine(false));
    }

    //Method called from UI to display the credits canvas
    public void DisplayCredits()
    {
        _creditsCanvas.enabled = true;
        _creditsScaler.enabled = true;
    }

    //Method called from UI to hide the credits canvas
    public void CloseCredits()
    {
        _creditsCanvas.enabled = false;
        _creditsScaler.enabled = false;
    }

    //Coroutine to change lighting, execute animation, load game scene async and exit game
    private IEnumerator StartOrExitCoroutine(bool isStart)
    {
        if (isStart)
        {
            _modelAnimator.SetTrigger("GameStart");
            _asyncLoad = SceneManager.LoadSceneAsync(1);
            _asyncLoad.allowSceneActivation = false;

            for (int i = 0; i < _timesLightChange; i++)
            {
                if (i % 2 == 0)
                    ChangeLightsColor(Color.green);
                else
                    ReturnLightsColor();
                yield return _wait;
            }
            
            ReturnLightsColor();
            _asyncLoad.allowSceneActivation = true;
        }
        else
        {
            _modelAnimator.SetTrigger("GameExit");

            for (int i = 0; i < _timesLightChange; i++)
            {
                if (i % 2 == 0)
                    ChangeLightsColor(Color.red);
                else
                    ReturnLightsColor();
                yield return _wait;
            }

            ReturnLightsColor();
            Application.Quit();
        }
    }

    //Method to change the color and material from the lights
    private void ChangeLightsColor(Color color)
    {
        _lightsRenderer.sharedMaterial.SetColor("_EmissionColor", color);
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].color = color;
        }
    }

    //Method to reset the color and material from the lights
    private void ReturnLightsColor()
    {
        _lightsRenderer.sharedMaterial.SetColor("_EmissionColor", _lightsEmissionColor);
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].color = _lightsColor;
        }
    }
}
