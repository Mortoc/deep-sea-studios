using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private ActorManager mActorManager = null;
    private MapManager mMapManager = null;

    private void Awake()
    {
        Debug.Log("game awake");

        mActorManager = new ActorManager();
        mMapManager = new MapManager();

    }

    private void Update()
    {
        Scheduler.Instance.Run();
    }
}
