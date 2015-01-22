using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour 
{
	private struct HexPosition
	{
		public int i;
		public int j;
	}

	public float _moveTime = 0.4f;
	public HexMap Map { get; set; }
	public TurnManager TurnManager { get; set; }
	private HexPosition Position;

	void Start()
	{
		Position = new HexPosition();
		foreach(var r in GetComponentsInChildren<Renderer>())
		{
			foreach(var m in r.materials)
			{
				var colorVec = new Vector3(
					m.color.r,
					m.color.g,
					m.color.b
				);
				var mag = colorVec.magnitude;
				colorVec /= mag;

				var newColorVec = Vector3.Slerp(colorVec, Random.onUnitSphere, Random.value) * mag;
				m.color = new Color(
					newColorVec.x, 
					newColorVec.y, 
					newColorVec.z, 
					m.color.a
				); 
			}
		}
	}

	public virtual void Murder()
	{
		Destroy (gameObject);
	}

	public bool IsAt(int i, int j)
	{
		return i == Position.i && j == Position.j;
	}

	protected IEnumerator AnimateToHex(int i, int j, Action afterAnimation = null)
	{
		Position.i = i;
		Position.j = j;
		var targetPos = Map.HexIdxToPos(i, j);
		var startPos = transform.position;
		var recipMoveTime = 1.0f / _moveTime;
		for(var time = 0.0f; time < _moveTime; time += Time.deltaTime)
		{
			var t = Mathf.SmoothStep(0.0f, 1.0f, time * recipMoveTime);
			transform.position = Vector3.Lerp (startPos, targetPos, t);
			yield return 0;
		}
		
		transform.position = targetPos;
		if( afterAnimation != null )
			afterAnimation();
	}
}
