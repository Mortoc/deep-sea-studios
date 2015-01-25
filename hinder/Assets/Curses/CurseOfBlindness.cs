using UnityEngine;
using System.Collections;

public class CurseOfBlindness : Curse 
{
	[SerializeField]
	private float _minVisibility = 0.15f;
	
	[SerializeField]
	private float _maxVisibility = 0.35f;
	
	[SerializeField]
	private float _cycleTime = 4.0f;


	public override void Apply (Player from, Player target)
	{
		StartCoroutine(AnimateAlpha(from));
	}

	private IEnumerator AnimateAlpha(Player player)
	{
		var recipCycleTime = 1.0f / _cycleTime;
		while( gameObject )
		{
			var sprite = player.GetComponent<SpriteRenderer>();

			var color = sprite.color;
			color.a = Mathf.Lerp
			( 
			    _minVisibility, 
				_maxVisibility, 
				(Mathf.Sin (Time.time * recipCycleTime) + 1.0f) * 0.5f
			);
			sprite.color = color;
			yield return 0;
		}
	}
}
