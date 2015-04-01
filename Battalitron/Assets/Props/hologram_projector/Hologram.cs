using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Hologram : MonoBehaviour
{
	[SerializeField]
	private Color _hologramColor;
	[SerializeField]
	private float _gridLinesIntensity = 0.25f;
	[SerializeField]
	private float _outlineSize = 0.1f;

	private Material _outlineMaterial;
	private Material _hologramMaterial;
	private List<GameObject> _outlines = new List<GameObject>();

	private Dictionary<Renderer, Material[]> _originalMaterials = new Dictionary<Renderer, Material[]>();


	void OnEnable()
	{
		_outlineMaterial = new Material(Shader.Find("Unlit/Color"));
		_outlineMaterial.color = _hologramColor;

		_hologramMaterial = new Material(Shader.Find("Custom/Hologram/Grid"));
		_hologramMaterial.SetColor("_color", Color.Lerp(Color.black, _hologramColor, _gridLinesIntensity));
		_hologramMaterial.SetColor("_baseColor", Color.black);
		_hologramMaterial.SetVector("_gridSpacing", Vector4.one * _outlineSize * 1.0f);
		_hologramMaterial.SetFloat("_gridThickness", _outlineSize * 10.0f);

		_originalMaterials.Clear();
		foreach(var r in GetComponentsInChildren<Renderer>())
		{
			_originalMaterials.Add(r, (Material[])r.sharedMaterials.Clone());
			var newMaterials = new Material[r.sharedMaterials.Length];
			for(var i = 0; i < newMaterials.Length; ++i)
			{
				newMaterials[i] = _hologramMaterial;
			}
			r.sharedMaterials = newMaterials;
		}

		_outlines = new List<GameObject>(Outliner.BuildOutline(
			gameObject,
			_outlineSize,
			_outlineMaterial,
			true,
			0.0f,
			true
		));
	}

	void OnDisable()
	{
		DestroyImmediate(_outlineMaterial);
		DestroyImmediate(_hologramMaterial);

		foreach(var outline in _outlines)
		{
			DestroyImmediate(outline);
		}
		_outlines.Clear();

		foreach(var originalRendererMaterial in _originalMaterials)
		{
			originalRendererMaterial.Key.sharedMaterials = originalRendererMaterial.Value;
		}
		_originalMaterials.Clear();
	}
}
