using UnityEngine;
using System.Collections;

public abstract class Curse : MonoBehaviour 
{
	[SerializeField]
	private ControllerManager.PlayerNumber _targetPlayer;

	
	[SerializeField]
	private ControllerManager.PlayerNumber _fromPlayer;


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
