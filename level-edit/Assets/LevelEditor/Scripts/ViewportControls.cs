using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewportControls : MonoBehaviour 
{
	
	[SerializeField]
	private float _zoomSpeed = 0.5f;
	
	[SerializeField]
	private float _panSpeed = 0.1f;

	[SerializeField]
	private Camera _sideCam;
	
	[SerializeField]
	private RectTransform _sideRect;

	[SerializeField]
	private Camera _topCam;
	
	[SerializeField]
	private RectTransform _topRect;

	[SerializeField]
	private Text _debugOut;

	void Update()
	{
		if( Input.touchCount == 1 )
		{
			var touch1 = Input.GetTouch (0);

			if( (touch1.position.x / Screen.width) > 0.5f ) 
			{
				_sideCam.transform.position += new Vector3(touch1.deltaPosition.x, touch1.deltaPosition.y, 0.0f) * _panSpeed * -1.0f;
			}
			else 
			{
				_topCam.transform.position += new Vector3(touch1.deltaPosition.x, 0.0f, touch1.deltaPosition.y) * _panSpeed * -1.0f;
			}
		}
		else if( Input.touchCount > 1 )
		{
			Touch touch1 = Input.GetTouch (0);
			Touch touch2 = Input.GetTouch (1);

			Vector2 touch1Prev = touch1.position - touch1.deltaPosition;
			Vector2 touch2Prev = touch2.position - touch2.deltaPosition;

			float prevTouchDelta = (touch1Prev - touch2Prev).magnitude;
			float touchDelta = (touch1.position - touch2.position).magnitude;

			float touchMagDiff = prevTouchDelta - touchDelta;

			_topCam.orthographicSize += touchMagDiff * _zoomSpeed;
			_topCam.orthographicSize = Mathf.Max (_topCam.orthographicSize, 0.1f);

			_sideCam.orthographicSize += touchMagDiff * _zoomSpeed;
			_sideCam.orthographicSize = Mathf.Max (_sideCam.orthographicSize, 0.1f);
		}
	}
}
