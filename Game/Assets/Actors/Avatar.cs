using System;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : ActorBase
{
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
		
		var moveSpeed = 0.05f;
		var horizontalMovement = Input.GetAxis("Horizontal");
		var verticalMovement = Input.GetAxis("Vertical");
		var horizontalRotation = Input.GetAxis("Horizontal2");

		
		mGameObject.transform.position +=  horizontalMovement * mGameObject.transform.right * moveSpeed ;
		mGameObject.transform.position +=  verticalMovement  * mGameObject.transform.forward * moveSpeed;
		mGameObject.transform.Rotate(Vector3.up * horizontalRotation);
    }
}
