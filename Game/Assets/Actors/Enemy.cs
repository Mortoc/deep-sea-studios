using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ActorBase
{
    private static int mEnemyIdCounter = 1;
    private int mId = 0;

    public Enemy(Vector3 initialPosition)
        : base(initialPosition)
    {
        mId = mEnemyIdCounter++;
        mGameObject.name = "enemy " + mId;
    }

    protected override void LoadModel(Vector3 initialPosition)
    {
        mGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mGameObject.transform.position = initialPosition;
    }
}

