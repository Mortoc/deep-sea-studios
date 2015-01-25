using UnityEngine;

using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Player : Being 
{
	[SerializeField]
	private float _speed = 10.0f;
	public float Speed
	{
		get { return _speed; }
		set { _speed = value; }
	}

	
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
	private Transform _attackCenter;

	[SerializeField]
	private LayerMask _enemyLayers;

	[SerializeField]
	private ParticleSystem _onHitEffect;

	private PlayerStatusGUI _gui;

	[SerializeField]
	private float _healthRegen = 1.0f;
	public float HealthRegen
	{
		get { return _healthRegen; }
		set { _healthRegen = value; }
	}

    public AudioClip jumpSound1;
    private AudioSource source;
    private float volLowRange = 0.1f;
    private float volHighRange = 0.3f;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_rightScale = transform.localScale;
		_leftScale = _rightScale;
		_leftScale.x *= -1.0f;
		_controllers = GameObject.FindObjectOfType<ControllerManager>();

		if( !_controllers ) 
			throw new InvalidOperationException("No controller manager found in this scene");

        source = GetComponent<AudioSource>();

		foreach(var gui in GameObject.FindObjectsOfType<PlayerStatusGUI>())
		{
			if( gui.playerNum == _playerNum )
			{
				_gui = gui;
			}
		}
	}

	public void SetScale(float targetScale)
	{
		_rightScale *= targetScale;
		_leftScale *= targetScale;
		transform.localScale = transform.localScale * targetScale;
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

		if( button == ControllerManager.ButtonLabel.BottomButton )
		{
			Jump();
		}

		if( button == ControllerManager.ButtonLabel.LeftButton )
		{
			Attack();
		}
	}

	public void Attack()
	{
		if( Time.time - _lastAttack > _maxAttackRate  )
		{
			_lastAttack = Time.time;
			_animator.SetTrigger("OnAttack");
			
			foreach(var hit in Physics2D.OverlapCircleAll(_attackCenter.position, 0.5f, _enemyLayers))
			{
				AttackLanded (hit);
			}

		}
	}

	public void Jump()
	{
		if( _grounded )
		{
            float vol = UnityEngine.Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(jumpSound1, vol);

			_grounded = false;
			rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
		}
	}

	public override void RecieveDamage(float damage)
	{
		base.RecieveDamage(damage);

		_onHitEffect.Emit(Mathf.FloorToInt(10.0f * damage / _hitPoints));
		_gui.UpdateHealth(_hitPoints, _damageTaken);
	}

	public void AttackLanded(Collider2D colliderHit)
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
		}
	}

	void Update()
	{
		_damageTaken -= _healthRegen * Time.deltaTime;
		_gui.UpdateHealth(_hitPoints, _damageTaken);
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

		//Debug.DrawLine(rigidbody2D.position, rigidbody2D.position + Vector2.up * -0.66f);

		_animator.SetBool("IsGrounded", _grounded);
	}

	
	public override void TimeToDie ()
	{
		_animator.SetTrigger("OnDeath");

		foreach(var col in gameObject.GetComponents<Collider2D>())
			col.enabled = false;

		gameObject.rigidbody2D.Sleep();

		this.enabled = false;

		_gui.SetRespawnTimer(5.0f);
	}
}
