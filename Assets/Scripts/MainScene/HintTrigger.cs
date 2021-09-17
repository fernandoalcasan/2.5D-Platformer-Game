using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class attached to world canvases for the tutorial hints
public class HintTrigger : MonoBehaviour
{
    private Canvas _canvas;

    //var initialization
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        if (_canvas is null)
            Debug.LogError("Canvas is NULL");
    }

    //Method to enable canvas component when player is near the Canvas box collider
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            _canvas.enabled = true;
    }

    //Method to disable canvas component when player leaves the Canvas box collider

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            _canvas.enabled = false;
    }
}
