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

    protected IEnumerator<IYieldInstruction> Update()
    {
        while (!mIsDisposed)
        {
            Debug.Log(mGameObject.name + " update");

            yield return YieldReturnZero.Instance;
        }
    }

    public void Dispose()
    {
        mIsDisposed = true;
    }
}
