using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS.States
{
    public class HomeState : GameState
    {
        [SerializeField]
        private GameObject _homeScreenGUIPrefab;
        private GameObject _homeScreenGUI;

        public override void EnterState()
        {
            base.EnterState();
            _homeScreenGUI = Instantiate(_homeScreenGUIPrefab);
        }

        public override void ExitState()
        {
            base.ExitState();
            Destroy(_homeScreenGUI);
        }
    }
}
