using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager
{
    private Avatar mAvatar = null;
    private EnemyManager mEnemyManager = null;

    public ActorManager()
    {
        mAvatar = new Avatar(new Vector3(1115,161,883));
        mEnemyManager = new EnemyManager(5);
    }
}
