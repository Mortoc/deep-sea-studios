using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
	[SerializeField]
	private Vector3 _spinAxes = Vector3.up * 10.0f;

	void Update() {
		transform.Rotate(_spinAxes * Time.deltaTime);
	}
}
