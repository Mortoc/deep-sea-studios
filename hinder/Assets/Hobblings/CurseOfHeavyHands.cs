using UnityEngine;
using System.Collections;

public class CurseOfHeavyHands : Curse 
{
	[SerializeField]
	private float _healthRegenReduction = 0.9f;

	[SerializeField]
	private float _speedReduction = 0.1f;

	[SerializeField]
	private float _sizeIncrease = 0.15f;
	
	[SerializeField]
	private float _attackIncrease = 0.15f;

	public override void Apply(Player fromPlayer, Player targetPlayer)
	{
		targetPlayer.HealthRegen = (1.0f - _healthRegenReduction) * targetPlayer.HealthRegen;
		targetPlayer.Speed = (1.0f + _speedReduction) * targetPlayer.Speed;
		targetPlayer.SetScale(1.0f + _sizeIncrease);
		targetPlayer.AttackDamage = targetPlayer.AttackDamage * (1.0f + _attackIncrease);
	}
}
