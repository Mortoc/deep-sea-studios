using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MetalBarRoundSchematicEditor : MonoBehaviour 
{
	[SerializeField]
	private GameObject _editingObject;

	[SerializeField]
	private Slider _lengthSlider;
	
	[SerializeField]
	private Text _output;

	[SerializeField]
	private float _minLength = 0.1f;

	[SerializeField]
	private float _maxLength = 1.5f;

	private Spin _spin;
	private Vector3 _startSpin;

	void OnEnable()
	{
		var bones = GetBones();
		var dist = (bones[1].position - bones[0].position).magnitude;
		_output.text = dist.ToString ("f1");
		dist -= _minLength;
		dist /= _maxLength;
		_lengthSlider.value = Mathf.Clamp01(dist);
		_lengthSlider.onValueChanged.AddListener(UpdateLength);

		_spin = _editingObject.GetComponentInChildren<Spin>();
		_startSpin = _spin.Speed;
	}

	void OnDisable()
	{
		_lengthSlider.onValueChanged.RemoveListener(UpdateLength);
	}

	private Transform[] GetBones()
	{
		var skinnedMesh = _editingObject.GetComponentInChildren<SkinnedMeshRenderer>();
		if( skinnedMesh.bones.Length != 2 ) throw new InvalidOperationException("The metal bar must have exactly 2 bones");
		return skinnedMesh.bones;
	}

	private void UpdateLength(float t)
	{
		var bones = GetBones ();
		var middle = (bones[1].position + bones[0].position) * 0.5f;
		var boneDir = (bones[1].position - bones[0].position).normalized;
		var dist = Mathf.Lerp (_minLength, _maxLength, t);
		var offset = boneDir * (dist * 0.5f);
		bones[0].position = middle - offset;
		bones[1].position = middle + offset;

		_spin.Speed = _startSpin * Mathf.SmoothStep(1.2f, 0.5f, t);
		_output.text = dist.ToString ("f1");
	}

}
