using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;

namespace DSS.UI
{
    public class BackButton : MonoBehaviour
    {
        public void GoBack()
        {
			FindObjectOfType<WorkshopState> ().TransitionToState<HomeState>();
        }
    }
}
