using UnityEngine;
using System.Collections;

using DSS.States;
using DSS.Construction;

namespace DSS.Builds
{
	public class BuildsState : GameState
	{
		[SerializeField]
		private GameObject _buildsGUIPrefab;
		
		private BuildsUI _buildsGUI;

		
		public override void EnterState()
		{
			base.EnterState();
			
			_buildsGUI = Instantiate(_buildsGUIPrefab).GetComponent<BuildsUI>();
			_buildsGUI.Init(this);
		}
		
		public override void ExitState()
		{
			base.ExitState();
			Destroy(_buildsGUI.gameObject);
		}

		public void NewBuildSelected()
		{
			FindObjectOfType<WorkshopState>().TransitionToState<ConstructionState>();
		}
	}
}