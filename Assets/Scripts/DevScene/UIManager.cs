using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevScene
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Text _coinsTxt, _livesTxt;

        [SerializeField]
        private Player _playerRef;

        void Start()
        {
            Collectable.OnCoinCollected += AddCoin;
            DeadZone.OnPlayerFall += UpdateLives;
        }

        private void AddCoin()
        {
            _coinsTxt.text = "Coins: " + _playerRef.Coins;
        }

        private void UpdateLives()
        {
            _livesTxt.text = "Lives: " + _playerRef.Lives;
        }

        private void OnDestroy()
        {
            Collectable.OnCoinCollected -= AddCoin;
            DeadZone.OnPlayerFall -= UpdateLives;
        }
    }
}