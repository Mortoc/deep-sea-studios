using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private Enemy[] mEnemyArray = null;
    private static float mSpawnArea = 400.0f;

    public EnemyManager(int numberOfEnemies)
    {
        mEnemyArray = new Enemy[numberOfEnemies];
        Vector3 mapMiddle = new Vector3(1000.0f, 250.0f, 1000.0f);

        for (int i = 0; i < mEnemyArray.Length; ++i)
        {
            Vector3 position = mapMiddle + UnityEngine.Random.onUnitSphere * mSpawnArea;
            position.y = mapMiddle.y;
            mEnemyArray[i] = new Enemy(position);
        }
    }
}
