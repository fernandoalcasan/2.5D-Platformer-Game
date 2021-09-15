using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        if (_canvas is null)
            Debug.LogError("Canvas is NULL");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            _canvas.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            _canvas.enabled = false;
    }
}
