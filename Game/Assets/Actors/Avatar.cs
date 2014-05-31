using System;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : ActorBase
{
    public Avatar(Vector3 initialPosition)
        : base(initialPosition)
    {
        mGameObject.name = "avatar";
    }

    protected override void  LoadModel(Vector3 initialPosition)
    {
        mGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mGameObject.transform.position = initialPosition;
    }
}
