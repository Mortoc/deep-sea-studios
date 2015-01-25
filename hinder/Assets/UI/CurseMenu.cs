using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CurseMenu : MonoBehaviour 
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


	void OnEnable()
	{
		Curse.p1Selection = null;
		Curse.p2Selection = null;
		_manager.OnButtonPress += HandleOnButtonPress;
	}

	
	void OnDisable()
	{	
		_manager.OnButtonPress -= HandleOnButtonPress;
	}
	
	void HandleOnButtonPress (ControllerManager.ButtonLabel arg1, ControllerManager.PlayerNumber arg2)
	{
		System.Type selection;
		switch(arg1)
		{
		case ControllerManager.ButtonLabel.LeftButton:
			selection = typeof(CurseOfHeavyHands);
			break;
		case ControllerManager.ButtonLabel.TopButton:
			selection = typeof(CurseOfBlindness);
			break;
		case ControllerManager.ButtonLabel.RightButton:
			selection = typeof(CurseOfMimicry);
			break;
		default:
			return;
		}


		if( arg2 == ControllerManager.PlayerNumber.P1 )
		{
			_p1background.color = _p1Color;
			_p1ReadyText.text = "READY!";
			Curse.p1Selection = selection;
		}
		else
		{
			_p2background.color = _p2Color;
			_p2ReadyText.text = "READY!";
			Curse.p2Selection = selection;
		}


		if( Curse.p1Selection != null && 
		    Curse.p2Selection != null )
		{
			Application.LoadLevel ("Level1");
		}
	}
}
