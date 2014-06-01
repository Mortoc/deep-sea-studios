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
		mGameObject = GameObject.CreatePrimitive (PrimitiveType.Capsule);
		mGameObject.transform.position = initialPosition;
		mGameObject.transform.localScale = new Vector3(1,2,1);
		mGameObject.name = "avatar";

		Camera.main.transform.parent = mGameObject.transform;
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localRotation = Quaternion.identity;
	
		var cameraGO = Camera.main.gameObject;
		var backgroundAudio = cameraGO.AddComponent<AudioSource>();
		backgroundAudio.clip = Resources.Load ("Sounds/ANW1043_01_Creeping-Shadow") as AudioClip;
		backgroundAudio.loop = true;
		backgroundAudio.playOnAwake = true;
		backgroundAudio.volume = 0.02f;
		cameraGO.audio.Play ();

		mGameObject.AddComponent<Rigidbody> ();
		mGameObject.rigidbody.freezeRotation = true;
	
		var audioSource = mGameObject.AddComponent<AudioSource>();
		audioSource.clip = Resources.Load ("Sounds/211389__monica137142__underwater---played when character moves") as AudioClip;
    }

    protected override void ActorUpdate()
    {
		//on "A" key, add force up
		if (Input.GetKeyDown(KeyCode.JoystickButton0) || 
		    Input.GetKeyDown(KeyCode.Space))
		{
			mGameObject.rigidbody.AddForce(Vector3.up * 250);
		}
		
		var horizontalMovement = Input.GetAxis("Horizontal");
		var verticalMovement = Input.GetAxis("Vertical");
		var horizontalRotation = Input.GetAxis("Horizontal2");

		
		mGameObject.rigidbody.AddForce (horizontalMovement * mGameObject.transform.right * moveSpeed);
		mGameObject.rigidbody.AddForce (verticalMovement  * mGameObject.transform.forward * moveSpeed);
		mGameObject.transform.Rotate(Vector3.up * horizontalRotation);

		//limit speed
		if (Math.Abs(mGameObject.rigidbody.velocity.z) > topSpeed)
		{
			mGameObject.rigidbody.velocity = new Vector3(
				mGameObject.rigidbody.velocity.x, 
				mGameObject.rigidbody.velocity.y,
				mGameObject.rigidbody.velocity.z > 0 ? topSpeed : topSpeed * -1);
		}
		if (Math.Abs(mGameObject.rigidbody.velocity.x) > topSpeed)
		{
			mGameObject.rigidbody.velocity = new Vector3(
				mGameObject.rigidbody.velocity.x > 0 ? topSpeed : topSpeed * -1, 
				mGameObject.rigidbody.velocity.y,
				mGameObject.rigidbody.velocity.z);
		}


		//adding sound for when user inputs movement (not including velocity)
		if (horizontalMovement != 0 || verticalMovement != 0 || horizontalRotation != 0) {
			if(!mGameObject.audio.isPlaying){
				mGameObject.audio.Play();
			}
		}
		else{
			mGameObject.audio.Stop();
		}

    }
}
