using UnityEngine;
using System.Collections;

namespace DSS
{
	[RequireComponent(typeof(Camera))]
	public class SceneCamera : MonoBehaviour
	{
		[SerializeField]
		[Range(1, 8)]
		private int _downsample = 1;
		
		void Start()
		{
			var cam = GetComponent<Camera> ();
			
			cam.targetTexture.width = Screen.width / _downsample;
			cam.targetTexture.height = Screen.height / _downsample;
		}
	}
}