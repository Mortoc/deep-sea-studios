using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace DSS.Construction
{
	// Will trace over the high res collision of the object we're working on
	public class OriginMarker : MonoBehaviour 
	{
		private const string COLLISION_LAYER = "HighResolutionCollision";
		private DragRecognizer _drag;

		private int _collisionMask;

		void Awake()
		{
			_collisionMask = 1 << LayerMask.NameToLayer(COLLISION_LAYER);
			_drag = GetComponent<DragRecognizer>();
			if( !_drag ) 
			{
	            _drag = gameObject.AddComponent<DragRecognizer>();
				_drag.RequiredFingerCount = 1;
				_drag.IsExclusive = true;
	            _drag.MaxSimultaneousGestures = 1;
	            _drag.SendMessageToSelection = GestureRecognizer.SelectionType.None;
			}
		}

		public void SpawnOnObject(GameObject obj)
		{
			gameObject.SetActive (true);
			var cam = Camera.main;
			Vector3 diff = obj.transform.position - cam.transform.position;
			float dist = diff.magnitude;
			Vector3 dir = diff / dist;
			RaycastHit rh;

			if (!Physics.Raycast (cam.transform.position, dir, out rh, dist * 2.0f, _collisionMask)) 
			{
				throw new Exception(String.Format("Can't calculate startpoint, didn't hit anything in the {0} layer", COLLISION_LAYER));
			}

			transform.position = rh.point;
			transform.up = rh.normal;
			
			CameraRig.MoveTo(transform, rh.normal * -1.0f);
		}
		
		public void Disable()
		{
			gameObject.SetActive(false);
		}


	}
}