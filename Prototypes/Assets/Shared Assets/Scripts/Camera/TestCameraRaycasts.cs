using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class TestCameraRaycasts : MonoBehaviour
{
    IEnumerator Start()
    {
        var cam = GetComponent<Camera>();
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var xScale = (float)cam.targetTexture.width / (float)Screen.width;
        var yScale = (float)cam.targetTexture.height / (float)Screen.height;

        while (gameObject)
        {
            yield return 0;
            var pos = Input.mousePosition;
            pos.x *= xScale;
            pos.y *= yScale;
            sphere.transform.position = cam.ScreenPointToRay(pos).GetPoint(10.0f);
        }
    }
}
