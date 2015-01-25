using UnityEngine;
using System.Collections;

public abstract class Being : MonoBehaviour 
{
	[SerializeField]
	protected float _hitPoints;
	protected float _damageTaken;

	[SerializeField]
	protected float _attackDamage;
	public float AttackDamage 
	{ 
		get { return _attackDamage; }
		set { _attackDamage = value; }
	}

	[SerializeField]
	protected LayerMask _groundLayers;

	[SerializeField]
	protected ControllerManager.PlayerNumber _playerNum;
	public ControllerManager.PlayerNumber PlayerNumber
	{
		get { return _playerNum; }
	}

	public virtual void RecieveDamage(float damage)
	{
		_damageTaken += damage;

		if( _damageTaken >= _hitPoints )
		{
			TimeToDie();
		}
	}

	public abstract void TimeToDie();
}
