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

        public void GoToWorkshop()
        {
            Manager.TransitionToState<Workshop.States.WorkshopState>();
        }

        public void GoToFight()
        {
            //Manager.CreateSubState<Arena.States.ArenaState>(true);
        }
    }
}