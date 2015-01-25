using UnityEngine;
using System.Collections;

public abstract class Curse : MonoBehaviour 
{
	public static System.Type p1Selection = null;
	public static System.Type p2Selection = null;


	[SerializeField]
	private ControllerManager.PlayerNumber _targetPlayer;

	public ControllerManager.PlayerNumber TargetPlayer 
	{
		get 
		{
			return _targetPlayer;
		}
		set 
		{
			_targetPlayer = value;
		}
	}

	
	[SerializeField]
	private ControllerManager.PlayerNumber _fromPlayer;
	public ControllerManager.PlayerNumber FromPlayer 
	{
		get 
		{
			return _fromPlayer;
		}
		set 
		{
			_fromPlayer = value;
		}
	}

	void Start()
	{
		Player target = null;
		Player from = null;

		foreach(var p in GameObject.FindObjectsOfType<Player>())
		{
			if( p.PlayerNumber == _targetPlayer )
			{
				target = p;
			}
			else if( p.PlayerNumber == _fromPlayer )
			{
				from = p;
			}
		}

		if( !target || !from ) 
		{
			Debug.LogWarning("unable to find the target or the source player for this curse");
			return;
		}

		this.Apply (from, target);

	}
	
	public abstract void Apply(Player from, Player target);
}
