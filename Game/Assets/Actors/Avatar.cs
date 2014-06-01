using System;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : ActorBase
{
	float topSpeed = 5.0f;
	float moveSpeed = 20.0f;

    public Avatar(Vector3 initialPosition)
        : base(initialPosition)
    {
    }

    protected override void  LoadModel(Vector3 initialPosition)
    {
        mGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mGameObject.transform.position = initialPosition;
        mGameObject.name = "avatar";

		Camera.main.transform.parent = mGameObject.transform;
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localRotation = Quaternion.identity;

		mGameObject.AddComponent<Rigidbody>();
		mGameObject.rigidbody.freezeRotation = true;
    }

    protected override void ActorUpdate()
    {
		//on "A" key, add force up
		if (Input.GetKeyDown(KeyCode.JoystickButton0))
		{
			mGameObject.rigidbody.AddForce(Vector3.up * 200);
		}
		//on "X" key, add force up and forward
		if (Input.GetKeyDown(KeyCode.JoystickButton2))
		{
			mGameObject.rigidbody.AddForce(Vector3.up * 100);
			mGameObject.rigidbody.AddForce(mGameObject.transform.forward * 100);
		}
		

		var horizontalMovement = Input.GetAxis("Horizontal");
		var verticalMovement = Input.GetAxis("Vertical");
		var horizontalRotation = Input.GetAxis("Horizontal2");

		
		mGameObject.rigidbody.AddForce (horizontalMovement * mGameObject.transform.right * moveSpeed);
		mGameObject.rigidbody.AddForce (verticalMovement  * mGameObject.transform.forward * moveSpeed);
		mGameObject.transform.Rotate(Vector3.up * horizontalRotation);

		//Debug.Log(mGameObject.rigidbody.velocity.magnitude);

		if (mGameObject.rigidbody.velocity.magnitude > topSpeed)
		{
			mGameObject.rigidbody.velocity = mGameObject.rigidbody.velocity.normalized * topSpeed;
		}
    }
}
