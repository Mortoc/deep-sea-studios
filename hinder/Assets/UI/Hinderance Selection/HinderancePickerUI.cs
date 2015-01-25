using UnityEngine;
using System.Collections;

public class HinderancePickerUI : MonoBehaviour 
{
	public void SelectedHinderanceP1(string hinderanceP1)
	{
		Debug.Log("Player 1 selected Hinderance: " + hinderanceP1);
	}

	public void SelectedHinderanceP2(string hinderanceP2)
	{
		Debug.Log("Player 2 selected Hinderance: " + hinderanceP2);
	}

}
