using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorBase : IDisposable
{
    protected Vector3 mInitialPosition = Vector3.zero;
    protected GameObject mGameObject = null;

    private bool mIsDisposed = false;

    protected ActorBase(Vector3 initialPosition)
    {
        LoadModel(initialPosition);

        Scheduler.Instance.AddCoroutine(Update());
    }

    protected abstract void LoadModel(Vector3 initialPosition);

    private IEnumerator<IYieldInstruction> Update()
    {
        while (!mIsDisposed)
        {
            ActorUpdate();

            yield return YieldReturnZero.Instance;
        }
    }

    protected abstract void ActorUpdate();

    public void Dispose()
    {
        Debug.Log(mGameObject.name + " disposed");
        mIsDisposed = true;
    }
}
