using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.Builds
{
	public class BuildsUI : MonoBehaviour
	{
		private BuildsState _state;
		
		public void Init(BuildsState state)
		{
			_state = state;
		}

		public void NewBuild()
		{
			_state.NewBuildSelected();
		}
	}
}
