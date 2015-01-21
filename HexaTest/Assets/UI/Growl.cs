using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

public class Growl : MonoBehaviour 
{
	[SerializeField]
	private float _fadeTime = 0.3f;

	[SerializeField]
	private Text _text;

	[SerializeField]
	private Image _background;

	private static Growl s_instance = null;
	private static Coroutine s_currentGrowl = null;

	private static void ShowGrowlWithProperties(string msg, Color color, float timeout, Func<bool> timeoutCallback = null)
	{
		if( s_instance )
		{
			if( s_currentGrowl != null ) 
			{
				s_instance.StopCoroutine(s_currentGrowl);
			}

			s_currentGrowl = s_instance.StartCoroutine
			(
				s_instance.ShowGrowl( msg, color, timeout, timeoutCallback )
			);
		}
	}

	public static void ShowError(object message, float timeout = 5.0f)
	{
		ShowGrowlWithProperties
		( 
			message.ToString (), 
			new Color(1.0f, 0.2f, 0.5f, 0.8f), 
			timeout
		);
	}
	public static void ShowError(object message, Func<bool> timeoutFunc)
	{
		ShowGrowlWithProperties
		(
			message.ToString (), 
			new Color(1.0f, 0.2f, 0.5f, 0.8f), 
			0.0f,
			timeoutFunc
		);
	}

	public static void ShowMessage(object message, float timeout = 5.0f)
	{
		ShowGrowlWithProperties
		( 
			message.ToString (), 
			new Color(0.0f, 0.6f, 1.0f, 0.8f),
			timeout
		);
	}
	public static void ShowMessage(object message, Func<bool> timeoutFunc)
	{
		ShowGrowlWithProperties
		(
			message.ToString (), 
			new Color(0.0f, 0.6f, 1.0f, 0.8f),
			0.0f,
			timeoutFunc
		);
	}

	void Awake()
	{
		s_instance = this;
		_background.CrossFadeAlpha(0.0f, 0.0f, true);
		_text.CrossFadeAlpha(0.0f, 0.0f, true);
		_background.enabled = false;
		_text.enabled = false;
	}

	private IEnumerator ShowGrowl(string message, Color color, float timeout, Func<bool> timeoutFunc)
	{
		_background.enabled = true;
		_text.enabled = true;

		_text.text = message;
		_background.color = color;
		GetComponentInChildren<Pulse>().UpdateOriginalColor(color);

		_background.CrossFadeAlpha(1.0f, _fadeTime, true);
		_text.CrossFadeAlpha(1.0f, _fadeTime, true);

		if( timeoutFunc != null ) 
		{
			while(!timeoutFunc())
			{
				yield return 0;
			}
		}
		else
		{
			yield return new WaitForSeconds(timeout - _fadeTime);
		}

		HideNow();
	}

	public void HideNow()
	{
		var halfFadeTime = 0.5f * _fadeTime;
		_background.CrossFadeAlpha(0.0f, halfFadeTime, true);
		_text.CrossFadeAlpha(0.0f, halfFadeTime, true);
		s_currentGrowl = null;
	}
}
