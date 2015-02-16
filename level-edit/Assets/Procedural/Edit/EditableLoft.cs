using UnityEngine;
using System;
using System.Collections.Generic;

namespace Procedural.Edit 
{
	public class EditableLoft : MonoBehaviour 
	{
		[SerializeField]
		private GameObject _controlPointHandle;

		[SerializeField]
		private LoftComponent _loft;

		private void GenerateHandles()
		{
			if( _loft == null ) throw new InvalidOperationException();

			foreach(var controlPoint in _loft.Path)
			{
				var cpHandleObj = Instantiate<GameObject>(_controlPointHandle);
				var cpHandle = cpHandleObj.GetComponent<EditableControlPoint>();
				cpHandle.BindToControlPoint(controlPoint);
			}
		}
	}
}