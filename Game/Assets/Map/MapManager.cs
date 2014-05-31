using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    private GameObject mRootMapGameObject = null;
    private GameObject mTerrainGameObject = null;

    public MapManager()
    {
        mRootMapGameObject = new GameObject("root map");

        mTerrainGameObject = (GameObject)GameObject.Instantiate(Resources.Load("Terrain"));
        mTerrainGameObject.name = "terrain";
        mTerrainGameObject.transform.position = Vector3.zero;

        mTerrainGameObject.transform.parent = mRootMapGameObject.transform;

    }
}


