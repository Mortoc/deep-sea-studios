using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BackstreetBots.States;

namespace BackstreetBots.Workshop.States
{
    public class WorkshopState : GameState
    {
        public override void EnterState()
        {
            base.EnterState();
            Application.LoadLevel("Workshop");
        }


        public void DoneEditingBot()
        {
            Manager.TransitionToState<MainMenuState>();
        }
    }
}