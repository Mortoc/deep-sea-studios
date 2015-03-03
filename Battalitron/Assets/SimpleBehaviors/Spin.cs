using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour 
{
	[SerializeField]
	private Vector3 _spin;

	void Update () 
	{
		transform.Rotate(_spin * Time.deltaTime);
	}
}
