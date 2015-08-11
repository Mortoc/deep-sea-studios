using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class CameraRig : MonoBehaviour
{
    public Transform Target
    {
        get { return transform; }
    }

	[SerializeField]
	private UnityEngine.UI.RawImage _sceneRenderTarget;

	[SerializeField]
	private float _screenResPercent = 0.75f;

	private RenderTexture _sceneRender;
	private RenderTexture _blurRender;

    [SerializeField]
    private Camera _sceneCamera;
    public Camera SceneCamera
    {
        get { return _sceneCamera; }
    }

	[SerializeField]
	private Camera _blurCamera;
	public Camera BlurCamera
	{
		get { return _blurCamera; }
	}
	
	[SerializeField]
	private Camera _finalCamera;
	public Camera FinalCamera
	{
		get { return _finalCamera; }
	}

    private static CameraRig s_rig;
    private static Coroutine s_currentMoveCommand;
    [SerializeField]
    private float _animateTime = 2.0f;

	public static void PlaySoundAtCamera(AudioClip clip)
	{
		if (!s_rig)
		{
			throw new InvalidOperationException("Cannot call PlaySoundAtCamera until after CameraRig.Awake()"); 
		}

		var pos = s_rig.GetComponentInChildren<AudioListener>().transform.position;
		AudioSource.PlayClipAtPoint (clip, pos);
	}
    
    public static void MoveTo(Transform center, Vector3 lookAt)
    {
        if (!s_rig)
        {
            throw new InvalidOperationException("Cannot call MoveTo until after CameraRig.Awake()"); 
        }
        
        if( s_currentMoveCommand != null ) 
        {
            s_rig.StopCoroutine(s_currentMoveCommand);
        }
        
		s_currentMoveCommand = s_rig.StartCoroutine(s_rig.AnimateLookAtFrom(center, lookAt));
    }
    
    private IEnumerator AnimateLookAtFrom(Transform target, Vector3 targetLook)
	{
		var orbit = _sceneCamera.GetComponent<TBOrbit> ();
		orbit.ResetPanning ();
		transform.forward = targetLook;
		var cameraRot = transform.rotation.eulerAngles;
		orbit.IdealYaw = cameraRot.y;
		orbit.IdealPitch = cameraRot.x;

        var startPos = _sceneCamera.transform.position;
		var targetPos = target.position;
        var invAnimateTime = 1.0f / _animateTime;
        
        for (var t = 0.0f; t < _animateTime; t += Time.deltaTime)
        {
            var smoothT = Mathf.SmoothStep(0.0f, 1.0f, t * invAnimateTime);
            transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            yield return 0;
        }
        transform.position = targetPos;
    }
    
    void Awake()
    {
        s_rig = this;

		int width = (int)(Screen.width * _screenResPercent);
		int height = (int)(Screen.height * _screenResPercent);

		_sceneRender = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
		_blurRender = new RenderTexture(width / 2, height / 2, 0, RenderTextureFormat.ARGB32);

		_sceneCamera.targetTexture = _sceneRender;
		_blurCamera.targetTexture = _blurRender;

		_sceneRender.SetGlobalShaderProperty("_sceneRender");
		_blurRender.SetGlobalShaderProperty("_blurRender");

		_sceneRenderTarget.texture = _sceneRender;
    }
    
    void OnDestroy()
    {
        s_rig = null;
        s_currentMoveCommand = null;
    }
}
