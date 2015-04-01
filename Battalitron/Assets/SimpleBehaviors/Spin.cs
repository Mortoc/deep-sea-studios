using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour 
{
	[SerializeField]
	private Vector3 _spin;

	public Vector3 Speed 
	{ 
		get { return _spin; } 
		set { _spin = value; }
	}

	void Update () 
	{
		transform.Rotate(_spin * Time.deltaTime);
	}
}
