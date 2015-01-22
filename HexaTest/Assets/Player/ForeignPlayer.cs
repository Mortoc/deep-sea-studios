using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ForeignPlayer : Player 
{
	public void UpdatePosition(int i, int j)
	{
		StartCoroutine (AnimateToHex(i, j));
	}
	
	public override void Murder()
	{
		FindObjectOfType<GameController>().ShowYouWin();
		base.Murder ();
	}
}