using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    Enemy[] mEnemyArray = null;

    public EnemyManager(int numberOfEnemies)
    {
        mEnemyArray = new Enemy[numberOfEnemies];
        for (int i = 0; i < mEnemyArray.Length; ++i)
        {
            mEnemyArray[i] = new Enemy(Vector3.zero);
        }
    }
}
