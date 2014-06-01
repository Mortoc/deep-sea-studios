using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    private GameObject mRootMapGameObject = null;
    private Terrain mTerrain = null;

    public MapManager()
    {
        mRootMapGameObject = new GameObject("root map");

        mTerrain = ((GameObject)GameObject.Instantiate(Resources.Load("Terrain"))).GetComponent<Terrain>();
        mTerrain.name = "terrain";
        mTerrain.transform.position = Vector3.zero;

        mTerrain.transform.parent = mRootMapGameObject.transform;

    }
}


