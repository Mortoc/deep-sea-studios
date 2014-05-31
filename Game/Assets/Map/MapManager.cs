using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    private GameObject mTerrain = null;

    public MapManager()
    {
        mTerrain = (GameObject)GameObject.Instantiate(Resources.Load("Terrain"));

        mTerrain.name = "terrain";
    }
}
