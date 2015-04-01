using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

public class ArtTools : EditorWindow
{
	[MenuItem("Window/Art Tools")]
	static void Init()
	{
		EditorWindow.GetWindow(typeof(ArtTools));
	}

	public void OnGUI()
	{
		DrawOutlinerTools();
	}

	private float _outlineAmount = 0.25f;
	private bool _highQuality = false;
	private Material _outlineMaterial = null;
	private float _jitter = 0.0f;
	private void DrawOutlinerTools()
	{
		_outlineAmount = EditorGUILayout.FloatField("Outline Size", _outlineAmount);
		_jitter = EditorGUILayout.FloatField("Jitter", _jitter);
		_highQuality = EditorGUILayout.Toggle("High Quality", _highQuality);
		_outlineMaterial = EditorGUILayout.ObjectField("Outline Material", _outlineMaterial, typeof(Material), false) as Material;

		GUILayout.BeginHorizontal();
		{
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Outline Selection", GUILayout.Width(128.0f)) && Selection.activeGameObject)
			{
				Outliner.BuildOutline(Selection.activeGameObject, _outlineAmount, _outlineMaterial, true, _jitter, _highQuality);
			}
		}
		GUILayout.EndHorizontal();
	}



}
