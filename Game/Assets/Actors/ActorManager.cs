using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager
{
    private Avatar mAvatar = null;
    private EnemyManager mEnemyManager = null;

    public ActorManager()
    {
        mAvatar = new Avatar(new Vector3(1102,161,923));
        mEnemyManager = new EnemyManager(0);
    }
}
