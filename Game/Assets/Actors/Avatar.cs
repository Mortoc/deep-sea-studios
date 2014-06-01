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
		mGameObject = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		mGameObject.transform.position = initialPosition;
		mGameObject.name = "avatar";

		Camera.main.transform.parent = mGameObject.transform;
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localRotation = Quaternion.identity;
	
		var cameraGO = Camera.main.gameObject;
		var backgroundAudio = cameraGO.AddComponent<AudioSource>();
		backgroundAudio.clip = Resources.Load ("Sounds/ANW1043_01_Creeping-Shadow") as AudioClip;
		backgroundAudio.loop = true;
		backgroundAudio.playOnAwake = true;
		backgroundAudio.volume = 0.03f;
		cameraGO.audio.Play ();

		mGameObject.AddComponent<Rigidbody> ();
		mGameObject.rigidbody.freezeRotation = true;
	
		var audioSource = mGameObject.AddComponent<AudioSource>();
		audioSource.clip = Resources.Load ("Sounds/211389__monica137142__underwater---played when character moves") as AudioClip;
		audioSource.volume = 1f;
    }

    protected override void ActorUpdate()
    {
		bool pressedMovementButton = false;

		//on "A" key, add force up
		if (Input.GetKeyDown(KeyCode.JoystickButton0) || 
		    Input.GetKeyDown(KeyCode.Space))
		{
			mGameObject.rigidbody.AddForce(Vector3.up * 200);

			pressedMovementButton = true;
		}
		//on "X" key, add force up and forward
		if (Input.GetKeyDown(KeyCode.JoystickButton2) ||
		    Input.GetKeyDown(KeyCode.LeftShift))
		{
			mGameObject.rigidbody.AddForce(Vector3.up * 100);
			mGameObject.rigidbody.AddForce(mGameObject.transform.forward * 100);

			pressedMovementButton = true;
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

		//adding sound for when user inputs movement (not including velocity)
		if (horizontalMovement != 0 || verticalMovement != 0 || horizontalRotation != 0 || pressedMovementButton == true) {
			if(!mGameObject.audio.isPlaying){
				mGameObject.audio.Play();
			}
		}
		else{
		//	fadeOut();
			mGameObject.audio.Stop();

			//for (var i = 9; i > 0; i--){
			//	mGameObject.audio.volume = i * .1;
			
			}
			
		}
	//function fadeOut() {
	//	if(audioSource.volume > 0.1)
	//	{
	//		audioSource.volume -= 0.1 * Time.deltaTime;

}


		
		
		
