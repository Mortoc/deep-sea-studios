using UnityEngine;

using System;
using System.Collections.Generic;

namespace Botter.States
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