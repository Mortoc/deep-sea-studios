using UnityEngine;
using System.Collections;

public class Being : MonoBehaviour 
{
	[SerializeField]
	protected float _hitPoints;
	protected float _damageTaken;

	[SerializeField]
	protected float _attackDamage;
	public float AttackDamage 
	{ 
		get { return _attackDamage; }
	}

	[SerializeField]
	protected LayerMask _groundLayers;

	[SerializeField]
	protected ControllerManager.PlayerNumber _playerNum;


}
