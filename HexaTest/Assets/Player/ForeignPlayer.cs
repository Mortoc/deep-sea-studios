using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class ForeignPlayer : Player 
{
	public void UpdatePosition(int i, int j)
	{
		StartCoroutine (AnimateToHex(i, j));
	}
}
