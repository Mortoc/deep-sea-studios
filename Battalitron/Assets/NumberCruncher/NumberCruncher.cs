using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Runtime.InteropServices;


public class NumberCruncher : MonoBehaviour 
{
	[SerializeField]
	private int _floatArrayLength = 32768; // 1mb of floats default
	
	[SerializeField]
	private int _numberOfRuns = 128;

	[SerializeField]
	private Text _outputText;

	private float[] _a;
	private float[] _b;

	private float[] _managedResults;
	private float[] _nativeResults;

	private float _managedRunTime;
	private float _nativeRunTime;

	private bool _runningTest = false;

	IEnumerator Start()
	{
		yield return 0;
		
		InitializeArrays();

		Run();
	}

	public void Run()
	{
		if( !_runningTest )
		{
			StartCoroutine(DoTest());
		}
	}

	private IEnumerator DoTest()
	{
		_outputText.text = "Managed: Running...\nNative: Queued\nData Verified: ?";
		yield return 0;

		DoManaged();
		_outputText.text = String.Format ("Managed: {0} s\nNative: Running...\nData Verified: ?", _managedRunTime);
		yield return 0;

		DoNative();
		_outputText.text = String.Format ("Managed: {0} s\nNative: {1} s\nData Verified: ?", _managedRunTime, _nativeRunTime);

		yield return 0;
		var passes = Verification();
		_outputText.text = String.Format ("Managed: {0} s\nNative: {1} s\nData Verified: {2}", _managedRunTime, _nativeRunTime, passes);
	}

	private bool Verification()
	{
		for(var i = 0; i < _floatArrayLength; ++i)
		{
			if( _managedResults[i] != _nativeResults[i] )
			{
				return false;
			}
		}
		return true;
	}

	private void InitializeArrays()
	{
		_a = new float[_floatArrayLength];
		_b = new float[_floatArrayLength];
		_managedResults = new float[_floatArrayLength];
		_nativeResults = new float[_floatArrayLength];

		for(var i = 0; i < _floatArrayLength; ++i)
		{
			_a[i] = UnityEngine.Random.value;
			_b[i] = UnityEngine.Random.value;
		}
	}

	private void DoManaged()
	{
		float start = Time.realtimeSinceStartup;
		
		for(var runNumber = 0; runNumber < _numberOfRuns; ++runNumber)
		{
			for(var i = 0; i < _floatArrayLength; ++i)
			{
				_managedResults[i] = _a[i] * _b[i];
			}
		}

		_managedRunTime = Time.realtimeSinceStartup - start;
	}

	
	#if UNITY_IPHONE || UNITY_XBOX360
	[DllImport ("__Internal")]
	#else
	[DllImport ("NumberCruncher")]
	#endif
	private static extern void Mult(uint length, 
	                                [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] float[] a, 
	                                [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] float[] b, 
	                                [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] float[] results);


	private unsafe void DoNative()
	{
		float start = Time.realtimeSinceStartup;

		for( var runNumber = 0; runNumber < _numberOfRuns; ++runNumber)
		{
			Mult ((uint)_floatArrayLength, _a, _b, _nativeResults);
		}

		_nativeRunTime = Time.realtimeSinceStartup - start;
	}
}
