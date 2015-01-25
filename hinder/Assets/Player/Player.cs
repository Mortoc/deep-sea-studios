using UnityEngine;
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

	[SerializeField]
	private LayerMask _groundLayers;

    public AudioSource jumpSound1;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_rightScale = transform.localScale;
		_leftScale = _rightScale;
		_leftScale.x *= -1.0f;

        jumpSound1 = (AudioSource)gameObject.AddComponent("AudioSource");
        AudioClip myAudioClip;
        myAudioClip = (AudioClip)Resources.Load("Player/SoundEffects/PlayerJumpVariation1");
        jumpSound1.clip = myAudioClip;
	}

	void Update()
	{
		if( _grounded && Input.GetKeyDown (KeyCode.Space) )
		{
			_grounded = false;
			rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            jumpSound1.Play();
		}
	}

	void FixedUpdate()
	{
		var movement = Input.GetAxis ("Horizontal");
		var speed = movement * _speed * Time.fixedDeltaTime;
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

		_grounded = Physics2D.Raycast(
			rigidbody2D.position + _footCollider.center, 
			Vector2.up * -1.0f, 
			_footCollider.radius * 1.5f, 
			_groundLayers.value
		);

		_animator.SetBool("IsGrounded", _grounded);
	}
}
