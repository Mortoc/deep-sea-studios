using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BackstreetBots.States;

namespace BackstreetBots.Workshop
{
    public class WorkshopController: MonoBehaviour
    {
        public void DoneEditing()
        {
            FindObjectOfType<States.WorkshopState>().DoneEditingBot();
        }
    }
}