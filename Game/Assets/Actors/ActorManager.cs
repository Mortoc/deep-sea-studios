using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager
{
    private Avatar mAvatar = null;
    private EnemyManager mEnemyManager = null;

    public ActorManager()
    {
        mAvatar = new Avatar(Vector3.one);
        mEnemyManager = new EnemyManager(5);
    }
}
