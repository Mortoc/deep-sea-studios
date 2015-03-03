using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FPS : MonoBehaviour 
{
	public Text _fpsDisplay;
	public float _updateRate = 0.5f;
	private float _lastUpdate = 0.0f;

	private float[] _samples = new float[32];
	private int _nextSampleIdx = 0;
	private float _recipSampleCount;

	void Start() 
	{
		_recipSampleCount = 1.0f / (float)_samples.Length;
	}

	void Update () 
	{
		float sample = 1.0f / Time.deltaTime;
		_samples[_nextSampleIdx] = sample;
		_nextSampleIdx = (_nextSampleIdx + 1) % _samples.Length;

		if( _lastUpdate + _updateRate <= Time.time )
		{
			_lastUpdate = Time.time;
			float accum = 0.0f;
			for(int i = 0; i < _samples.Length; ++i)
			{
				accum += _samples[i];
			}

			_fpsDisplay.text = string.Format("FPS: {0}", (accum * _recipSampleCount).ToString ("f1"));
		}
	}
}
