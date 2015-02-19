using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Botter.States;


namespace Botter
{
	public class AppEntry : MonoBehaviour
	{
		private GameStateManager _rootManager;

		void Awake()
		{
			DontDestroyOnLoad(gameObject);

			_rootManager = new GameStateManager("Root", null);
			_rootManager.SetState(new ApplicationStartingState(_rootManager));

			StartCoroutine(InitializeNetwork());
		}

		private IEnumerator InitializeNetwork()
		{
			// show connecting GUI until connected
			_rootManager.SetState(new ConnectingToServerState(_rootManager));

			// nothing yet for network code
			yield return new WaitForSeconds(0.5f);

			// show main menu when connected
			_rootManager.SetState(new MainMenuState(_rootManager));
		}
	}
}