using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	[SerializeField]
	private ControllerManager _manager;

	[SerializeField]
	private Color _p1Color;

	[SerializeField]
	private Image _p1background;
	
	[SerializeField]
	private Text _p1ReadyText;


	[SerializeField]
	private Color _p2Color;

	
	[SerializeField]
	private Image _p2background;
	
	[SerializeField]
	private Text _p2ReadyText;

	private bool _p1Ready = false;
	private bool _p2Ready = false;


	void OnEnable()
	{
		_manager.OnButtonPress += HandleOnButtonPress;
	}

	
	void OnDisable()
	{	
		_manager.OnButtonPress -= HandleOnButtonPress;
	}
	
	void HandleOnButtonPress (ControllerManager.ButtonLabel arg1, ControllerManager.PlayerNumber arg2)
	{
		if( arg1 == ControllerManager.ButtonLabel.LeftButton )
		{
			if( arg2 == ControllerManager.PlayerNumber.P1 )
			{
				_p1background.color = _p1Color;
				_p1ReadyText.text = "READY!";
				_p1Ready = true;
			}
			else
			{
				_p2background.color = _p2Color;
				_p2ReadyText.text = "READY!";
				_p2Ready = true;
			}


			if( _p1Ready && _p2Ready )
			{
				Application.LoadLevel ("Level1");
			}
		}
	}
}
