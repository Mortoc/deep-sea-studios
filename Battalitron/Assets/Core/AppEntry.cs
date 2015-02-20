using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using BackstreetBots.States;
using BackstreetBots.Networking.States;

namespace BackstreetBots
{
	public class AppEntry : MonoBehaviour
	{
		private GameStateManager _rootManager;

		void Awake()
		{
			DontDestroyOnLoad(gameObject);

            _rootManager = gameObject.AddComponent<GameStateManager>();
			_rootManager.Init("Root", null);
            var appLoading = _rootManager.CreateSubState<AppLoadingState>(true);
            appLoading.LoadingComplete += LoadingComplete;

            _rootManager.CreateSubManager<NetworkingStateManager>("NetworkManager");
		}

        private void LoadingComplete()
        {
            _rootManager.CreateSubState<MainMenuState>(true);
        }
	}
}