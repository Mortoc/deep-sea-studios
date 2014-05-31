using System;
using System.Collections.Generic;

public class Scheduler
{
    private static Scheduler mInstance = null;
    public static Scheduler Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new Scheduler();
            }
            return mInstance;
        }
    }

    private Scheduler()
    {
        mCoroutineLinkedList= new LinkedList<KeyValuePair<uint,IEnumerator<IYieldInstruction>>>();
    }

    private static uint mCoroutineId = 0;
    private LinkedList<KeyValuePair<uint, IEnumerator<IYieldInstruction>>> mCoroutineLinkedList = null;

    public uint AddCoroutine(IEnumerator<IYieldInstruction> coroutine)
    {
        uint coroutineId = ++mCoroutineId;
        coroutine.MoveNext();

        KeyValuePair<uint, IEnumerator<IYieldInstruction>> coroutineObject = new KeyValuePair<uint,IEnumerator<IYieldInstruction>>(coroutineId, coroutine);
        mCoroutineLinkedList.AddLast(coroutineObject);

        return coroutineId;
    }

    public void Run()
    {
        LinkedListNode<KeyValuePair<uint, IEnumerator<IYieldInstruction>>> currentCoroutineObject = mCoroutineLinkedList.First;
        while(currentCoroutineObject.Next != null)
        {
            bool moveNext = currentCoroutineObject.Value.Value.MoveNext();

            LinkedListNode<KeyValuePair<uint, IEnumerator<IYieldInstruction>>> prevCoroutine = currentCoroutineObject;
            currentCoroutineObject = currentCoroutineObject.Next;

            if (!moveNext)
            {
                mCoroutineLinkedList.Remove(prevCoroutine);
            }
        }
    }

}
