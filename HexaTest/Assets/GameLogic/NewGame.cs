using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour 
{
	public void ReloadLevel()
	{
		var controller = GameObject.FindObjectOfType<GameController>();
		controller.OnApplicationQuit();

		Application.LoadLevel(Application.loadedLevel);
	}
}
