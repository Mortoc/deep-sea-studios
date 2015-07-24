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
    private Camera _sceneCamera;
    public Camera SceneCamera
    {
        get { return _sceneCamera; }
    }
    
    private static CameraRig s_rig;
    private static Coroutine s_currentMoveCommand;
    [SerializeField]
    private float _animateTime = 2.0f;
    
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
    }
    
    void OnDestroy()
    {
        s_rig = null;
        s_currentMoveCommand = null;
    }
}
