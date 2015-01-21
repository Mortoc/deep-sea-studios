using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float _moveTime = 0.4f;
	public GameObject _bombPrefab;
	public HexMap Map { get; set; }

	void Start()
	{
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

	protected IEnumerator AnimateToHex(int i, int j)
	{
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
	}
}
