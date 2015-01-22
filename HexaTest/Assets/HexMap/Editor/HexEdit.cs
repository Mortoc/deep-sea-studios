using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class HexEdit : EditorWindow
{
	[MenuItem("Window/Hex Editor")]
	public static void ShowWindow() 
	{
		var window = EditorWindow.GetWindow(typeof(HexEdit));
		window.title = "Hex Map Editor";
	}

	private bool _settingsFoldout = false;
	private int _mapWidth = 10;
	private int _mapHeight = 10;
	private GUIContent _settingsFoldoutContent = new GUIContent("Settings");
	private Material _hexMaterial = null;
	
	private static void MakeNewHex()
	{
		Undo.RegisterCreatedObjectUndo(Hex.MakeHex(), "Created Hex");
	}
	
	void OnGUI()
	{
		var margins = 10.0f;
		GUILayout.Space (margins);
		GUILayout.BeginHorizontal();
		GUILayout.Space (margins);

		if( GUILayout.Button ("New Hex", GUILayout.Width(100.0f), GUILayout.Height(40.0f) ) )
		{
			MakeNewHex();
		}

		GUILayout.EndHorizontal();

		
		GUILayout.BeginHorizontal();
		GUILayout.Space (margins);
		
		if( GUILayout.Button ("New Hex Map", GUILayout.Width(100.0f), GUILayout.Height(40.0f) ) )
		{
			var ground = new GameObject("Ground");
			ground.transform.position = Vector3.zero;
			var map = ground.AddComponent<HexMap>();
			map._width = _mapWidth;
			map._height = _mapHeight;
			map._hexMaterial = _hexMaterial;
			map.Awake();
			
			Undo.RegisterCreatedObjectUndo(map, "Created Hex Map");
		}

		_mapWidth = EditorGUILayout.IntField(_mapWidth);
		_mapHeight = EditorGUILayout.IntField(_mapHeight);
		
		GUILayout.EndHorizontal();


		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal();
		GUILayout.Space (margins);

		_settingsFoldout = EditorGUILayout.Foldout(_settingsFoldout, _settingsFoldoutContent);
		if( _settingsFoldout ) 
		{
			GUILayout.BeginVertical();
			_hexMaterial = (Material)EditorGUILayout.ObjectField(_hexMaterial, typeof(Material), false);
			GUILayout.EndVertical();
		}


		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
	}
}
