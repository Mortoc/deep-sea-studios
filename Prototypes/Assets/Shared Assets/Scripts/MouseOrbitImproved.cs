using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour
{

    public Transform _target;
    public float _distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        UpdateCamera();
    }

    void OnPinch(float pinch)
    {
        _distance = Mathf.Clamp(_distance - pinch * 5, distanceMin, distanceMax);
    }

    private void UpdateCamera()
    {
        if( Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Moved )
        {
            x += Input.touches[0].deltaPosition.x * xSpeed * _distance * 0.02f;
            y -= Input.touches[0].deltaPosition.y * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);


        Vector3 negDistance = new Vector3(0.0f, 0.0f, -_distance);
        Vector3 position = rotation * negDistance + _target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    void LateUpdate()
    {
        UpdateCamera();
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}