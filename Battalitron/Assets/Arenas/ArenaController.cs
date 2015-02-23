using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BackstreetBots.States;

namespace BackstreetBots.Arena
{
    public class ArenaController: MonoBehaviour
    {
        public void FightComplete()
        {
            FindObjectOfType<States.ArenaState>().FightComplete();
        }
    }
}