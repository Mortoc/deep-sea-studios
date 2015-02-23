using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BackstreetBots.States;

namespace BackstreetBots.Networking.States
{
    public class ConnectingState : GameState
    {
        public override void EnterState()
        {
            PhotonNetwork.ConnectUsingSettings("0.1");
        }
    }
}