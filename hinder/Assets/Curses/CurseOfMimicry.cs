using UnityEngine;
using System.Collections;

public class CurseOfMimicry : Curse 
{
	[SerializeField]
	private float _chanceOfJump = 0.5f;
	
	[SerializeField]
	private float _chanceOfAttack = 0.25f;
	
	private ControllerManager _manager;

	private Player _from;
	private Player _target;

	public override void Apply (Player from, Player target)
	{
		_from = from;
		_target = target;
	}

	
	void HandleOnButtonPress (ControllerManager.ButtonLabel button, ControllerManager.PlayerNumber player)
	{
		if( player == _from.PlayerNumber )
		{
			if( button == ControllerManager.ButtonLabel.BottomButton && Random.value < _chanceOfJump )
			{
				_target.Jump();
			}

			if( button == ControllerManager.ButtonLabel.LeftButton && Random.value < _chanceOfAttack )
			{
				var startDamage =  _target.AttackDamage;
				_target.AttackDamage = 0.0f;
				_target.Attack();
				_target.AttackDamage = startDamage;
			}
		}
	}

	public void OnEnable()
	{
		_manager = GameObject.FindObjectOfType<ControllerManager>();
		_manager.OnButtonPress += HandleOnButtonPress;
	}


	public void OnDisable()
	{
		_manager.OnButtonPress -= HandleOnButtonPress;
	}

}
