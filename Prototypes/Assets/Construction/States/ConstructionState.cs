﻿using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.Construction
{
    public class ConstructionState : GameState
    {
        [SerializeField]
        private GameObject _constructionScreenGUIPrefab;
		
		[SerializeField]
		private GameObject _originMarkerPrefab;
		private OriginMarker _originMarker;
		public OriginMarker OriginMarkerObject
		{
			get 
			{ 
				if( !_originMarker ) 
				{
					_originMarker = Instantiate<GameObject>(_originMarkerPrefab).GetComponent<OriginMarker>();
				}
				return _originMarker; 
			}
		}
        
        private ConstructionUI _constructionScreenGUI;

        public ConstructionTool ActiveTool
        {
            get
            {
                return this.ActiveState as ConstructionTool;
            } 
        }

        public override void EnterState()
        {
            base.EnterState();

            _constructionScreenGUI = Instantiate(_constructionScreenGUIPrefab).GetComponent<ConstructionUI>();
            _constructionScreenGUI.Init(this);
            
            TransitionToState<NoToolSelected>();
        }

        public override void ExitState()
        {
            base.ExitState();
			if (_originMarker) 
			{
				Destroy(_originMarker.gameObject);
			}
            Destroy(_constructionScreenGUI.gameObject);
        }

    }
}
