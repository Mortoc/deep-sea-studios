using UnityEngine;

using System;
using System.Collections;

public class PlayerAttack : MonoBehaviour 
{
	[SerializeField]
	private Player _owner;

	void OnCollisionEnter2D(Collision2D col) 
	{
		_owner.AttackLanded(col);
	}
}
