using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Botter.States;
using Botter.Networking.States;

namespace Botter
{
	public class AppEntry : MonoBehaviour
	{
		private GameStateManager _rootManager;
        private NetworkingStateManager _networkConnectionManager;
        private GUIStateManager _guiStateManager;

		void Awake()
		{
			DontDestroyOnLoad(gameObject);

            _rootManager = gameObject.AddComponent<GameStateManager>();
			_rootManager.Init("Root", null);
            _rootManager.CreateSubState<ApplicationStartingState>(true);
            
            _guiStateManager = _rootManager.CreateSubManager<GUIStateManager>("GUIStateManager");
            _networkConnectionManager = _rootManager.CreateSubManager<NetworkingStateManager>("NetworkManager");
		}
	}
}