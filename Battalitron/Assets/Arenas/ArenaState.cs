using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BackstreetBots.States;

namespace BackstreetBots.Arena.States
{
    public class ArenaState : GameState
    {
        public override void EnterState()
        {
            base.EnterState();
            Application.LoadLevel("Arena1"); // the only arena for now
        }

        public void FightComplete()
        {
            Manager.TransitionToState<MainMenuState>();
        }
    }
}