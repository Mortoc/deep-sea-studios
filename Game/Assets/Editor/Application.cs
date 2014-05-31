using System;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;

public class ApplicationMenu : EditorWindow
{
    [MenuItem("Application/LoadProject")]
    private static void LoadProject()
    {
        string unitySceneFileName = UnityEngine.Application.dataPath + "/Game.unity";

        //check if we have the scene previously saved
        Debug.Log("unity scene file: " + unitySceneFileName);
        if (!File.Exists(unitySceneFileName))
        {
            //if not, create the scene
            EditorApplication.NewScene();

            //add the weird beard scene
            GameObject mainGameObject = new GameObject("Game");
            mainGameObject.AddComponent<Game>();

            Debug.Log("saving scene: " + unitySceneFileName);
            EditorApplication.SaveScene(unitySceneFileName);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
        else
        {
            //if yes, open the scene
            EditorApplication.OpenScene(unitySceneFileName);
        }
    }
}