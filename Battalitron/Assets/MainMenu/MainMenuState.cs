using UnityEngine;

using System;
using System.Collections.Generic;

namespace BackstreetBots.States
{
    public class MainMenuState : GameState
    {
        public override void EnterState()
        {
            base.EnterState();

            Application.LoadLevel("MainMenu");
        }
    }
}