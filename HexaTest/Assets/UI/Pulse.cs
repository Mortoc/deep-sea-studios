using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pulse : MonoBehaviour 
{
	public SpriteRenderer _sprite;
	public Image _image;


	private Color _originalColor;
	public float _pulseRate = 1.0f;
	public float _darkAmount = 0.5f;

	void Start()
	{
		if( _sprite )
			_originalColor = _sprite.color;
		else
			_originalColor = _image.color;
	}

	public void UpdateOriginalColor(Color newColor)
	{
		_originalColor = newColor;
	}

	void Update()
	{	
		var t = (1.0f + Mathf.Sin(Time.time * _pulseRate)) * _darkAmount * 0.5f;

		if( _sprite )
			_sprite.color = Color.Lerp (_originalColor, Color.black, t);
		else
			_image.color = Color.Lerp (_originalColor, Color.black, t);
	}
}
