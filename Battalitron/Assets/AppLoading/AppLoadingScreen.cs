using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using BackstreetBots.States;
using BackstreetBots.Networking.States;

namespace BackstreetBots
{
    public class AppLoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private Image _readyButton;
        [SerializeField]
        private Text _loadingMessage;

        private NetworkingStateManager _networkManager;

        public IEnumerator Start()
        {
            _readyButton.gameObject.SetActive(false);

            while (!_networkManager)
            {
                _networkManager = FindObjectOfType<NetworkingStateManager>();
                yield return 0;
            }
            _networkManager.StateTransition += NetworkingStateChange;
        }

        public void OnDisable()
        {
            if (_networkManager)
            {
                _networkManager.StateTransition -= NetworkingStateChange;
            }
        }

        private void NetworkingStateChange(GameState newState)
        {
            if (newState is InLobbyState || newState is CannotConnectState)
            {
                _readyButton.gameObject.SetActive(true);
                _loadingMessage.text = "Ready";
            }
        }

        public void ReadyPressed()
        {
            var appLoadingState = FindObjectOfType<AppLoadingState>();
            appLoadingState.LoadingIsComplete();
        }

    }
}
