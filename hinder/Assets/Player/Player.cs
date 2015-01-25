using UnityEngine;

using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Player : Being 
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
	private float _maxAttackRate = 1.0f;
	private float _lastAttack = 0.0f;

	[SerializeField]
	private GameObject _attackObject;


	void Awake()
	{
		_animator = GetComponent<Animator>();
		_rightScale = transform.localScale;
		_leftScale = _rightScale;
		_leftScale.x *= -1.0f;
		_controllers = GameObject.FindObjectOfType<ControllerManager>();

		if( !_controllers ) 
			throw new InvalidOperationException("No controller manager found in this scene");
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

		if( button == ControllerManager.ButtonLabel.LeftButton && Time.time - _lastAttack > _maxAttackRate )
		{
			_lastAttack = Time.time;
			_animator.SetTrigger("OnAttack");
			StartCoroutine(DoAttack());
		}
	}

	IEnumerator DoAttack()
	{
		_attackObject.SetActive(true);
		yield return 0;
		_attackObject.SetActive(false);
	}

	public void AttackLanded(Collision2D colliderHit)
	{
		var beingHit = colliderHit.gameObject.GetComponentInChildren<Being>();
		if( beingHit )
		{
			beingHit.RecieveDamage(_attackDamage);
			AttackLandedThisFrame();
		}
	}

	private int _lastAttackHitFrame = 0;
	private void AttackLandedThisFrame()
	{
		if( Time.frameCount > _lastAttackHitFrame )
		{
			_lastAttackHitFrame = Time.frameCount;
			Debug.Log ("hit");
		}
	}

	void FixedUpdate()
	{
		var speed = _inputMovement * _speed * Time.fixedDeltaTime;
		rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
		_animator.SetFloat("X-Speed", rigidbody2D.velocity.x);
		_animator.SetFloat("Y-Speed", rigidbody2D.velocity.y);

		if( speed < -0.01f ) 
		{
			transform.localScale = _leftScale;
		}
		else if( speed > 0.01f )
		{
			transform.localScale = _rightScale;
		}

		_grounded = Physics2D.Raycast
		(
			rigidbody2D.position, 
			Vector2.up * -1.0f, 
			0.66f, 
			_groundLayers.value
		);

		Debug.DrawLine(rigidbody2D.position, rigidbody2D.position + Vector2.up * -0.66f);

		_animator.SetBool("IsGrounded", _grounded);
	}

	
	public override void TimeToDie ()
	{
		_animator.SetTrigger("OnDeath");
		Destroy (this);
	}
}
