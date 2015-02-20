using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Botter.States;

namespace Botter.Networking.States
{
    public class ConnectingState : GameState
    {
        public override void EnterState()
        {
            PhotonNetwork.ConnectUsingSettings("0.1");
        }
    }
}