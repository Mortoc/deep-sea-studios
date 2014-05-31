using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Game : MonoBehaviour
{
    private ActorManager mActorManager = null;

    private void Awake()
    {
        Debug.Log("game awake");

        mActorManager = new ActorManager();
    }
}
