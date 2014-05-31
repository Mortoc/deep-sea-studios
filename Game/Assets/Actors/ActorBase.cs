using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorBase
{
    protected Vector3 mInitialPosition = Vector3.zero;
    protected GameObject mGameObject = null;

    protected ActorBase(Vector3 initialPosition)
    {
        LoadModel(initialPosition);
    }

    protected abstract void LoadModel(Vector3 initialPosition);
}
