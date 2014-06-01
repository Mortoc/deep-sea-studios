using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private Enemy[] mEnemyArray = null;

    public EnemyManager(int numberOfEnemies)
    {
        mEnemyArray = new Enemy[numberOfEnemies];
        for (int i = 0; i < mEnemyArray.Length; ++i)
        {
            mEnemyArray[i] = new Enemy(new Vector3(1000.0f, 500.0f, 1000.0f));
        }
    }
}
