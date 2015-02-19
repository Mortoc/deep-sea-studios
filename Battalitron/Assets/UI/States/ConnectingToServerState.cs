﻿using UnityEngine;

using System;
using System.Collections.Generic;

namespace Botter.States 
{
	public class ConnectingToServerState : BasicState 
	{
		public ConnectingToServerState(GameStateManager manager)
			: base(manager)
		{
		}

		#region implemented abstract members of BasicState
		public override void EnterState ()
		{
			throw new NotImplementedException ();
		}
		public override void ExitState ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}