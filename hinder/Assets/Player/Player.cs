using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	[SerializeField]
	private float _speed = 10.0f;

	void Awake()
	{
	}

	void FixedUpdate()
	{
		var movement = Vector3.zero;

		if( Input.GetKey(KeyCode.A) )
		{
			movement += Vector3.left;
		}
		
		if( Input.GetKey(KeyCode.D) )
		{
			movement += Vector3.right;
		}

		//_controller.Move (movement * _speed * Time.fixedDeltaTime);
	}
}
