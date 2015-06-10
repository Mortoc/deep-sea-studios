using UnityEngine;
using System.Collections;

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
}
