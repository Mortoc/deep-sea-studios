using UnityEngine;
using System.Collections;

public class testMoveVRCamera : MonoBehaviour {

	public float speed = 2.0f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			transform.Translate (new Vector3 (speed * -1.0f, 0, 0) * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate (new Vector3 (speed * 1.0f, 0, 0) * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.Space))
		{
			transform.Translate (new Vector3 (0, 0, speed * 1.0f) * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.LeftShift))
		{
			transform.Translate (new Vector3 (0, 0, speed * -1.0f) * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.UpArrow))
		{
			transform.Translate (new Vector3 (0, speed * 1.0f, 0) * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.DownArrow))
		{
			transform.Translate (new Vector3 (0, speed * -1.0f, 0) * Time.deltaTime);
		}
	}
}
