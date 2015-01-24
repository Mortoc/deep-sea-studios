using UnityEngine;

using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour 
{
	[SerializeField]
	private float _speed = 10.0f;
	
	[SerializeField]
	private float _jumpForce = 100.0f;

	[SerializeField]
	private CircleCollider2D _footCollider;

	private Animator _animator;
	private Vector3 _rightScale;
	private Vector3 _leftScale;

	private bool _grounded = true;

	private ControllerManager _controllers;
	private float _inputMovement = 0.0f;

	[SerializeField]
	private LayerMask _groundLayers;

	[SerializeField]
	private ControllerManager.PlayerNumber _playerNum;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_rightScale = transform.localScale;
		_leftScale = _rightScale;
		_leftScale.x *= -1.0f;
		_controllers = GameObject.FindObjectOfType<ControllerManager>();
		if( !_controllers ) throw new InvalidOperationException("No controller manager found in this scene");
	}

	void OnEnable()
	{
		_controllers.OnButtonPress += HandleButtonPressed;
		_controllers.OnAnalogMovement += HandleOnAnalogMovement;
	}

	void OnDisable()
	{
		_controllers.OnButtonPress -= HandleButtonPressed;
		_controllers.OnAnalogMovement -= HandleOnAnalogMovement;
	}
	
	private void HandleOnAnalogMovement (ControllerManager.AnalogLabel label, ControllerManager.PlayerNumber playerNum, float value)
	{
		if( _playerNum != playerNum ) return;


		if( label == ControllerManager.AnalogLabel.LeftAnalogX ) 
		{
			_inputMovement = value;
		}
	}

	private void HandleButtonPressed(ControllerManager.ButtonLabel button, ControllerManager.PlayerNumber playerNum)
	{
		if( _playerNum != playerNum ) return;

		if( _grounded && button == ControllerManager.ButtonLabel.BottomButton )
		{
			_grounded = false;
			rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
		}
	}

	void FixedUpdate()
	{
		var speed = _inputMovement * _speed * Time.fixedDeltaTime;
		_animator.SetFloat ("X-Speed", speed);

		rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);

		if( speed < 0.0f ) 
		{
			transform.localScale = _leftScale;
		}
		else
		{
			transform.localScale = _rightScale;
		}

		_grounded = Physics2D.Raycast
		(
			rigidbody2D.position, 
			Vector2.up * -1.0f, 
			0.5f, 
			_groundLayers.value
		);

		Debug.DrawLine (
			rigidbody2D.position, 
			rigidbody2D.position + (Vector2.up * -0.5f),
			Color.red
		);

		_animator.SetBool("IsGrounded", _grounded);
	}
}
