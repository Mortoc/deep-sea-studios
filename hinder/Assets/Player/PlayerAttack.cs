using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour 
{
	[SerializeField]
	private Player _owner;

	void OnCollisionEnter2D(Collision2D coll) 
	{
		coll.gameObject.SendMessage("ApplyDamage", _owner.AttackDamage);
	}
}
