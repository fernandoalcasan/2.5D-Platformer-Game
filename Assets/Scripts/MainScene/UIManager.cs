using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _coinsTxt;

    [SerializeField]
    private Player _playerRef;

    void Start()
    {
        Collectable.OnCoinCollected += AddCoin;
    }

    private void AddCoin()
    {
        _coinsTxt.text = "Coins: " + _playerRef.Coins;
    }

    private void OnDestroy()
    {
        Collectable.OnCoinCollected -= AddCoin;
    }
}
