using UnityEngine;
using System.Collections;

using BackstreetBots.States;

namespace BackstreetBots
{
    public class MainMenuController : MonoBehaviour
    {
        public void FightSelected()
        {
            FindObjectOfType<MainMenuState>().GoToFight();
        }

        public void WorkshopSelected()
        {
            FindObjectOfType<MainMenuState>().GoToWorkshop();
        }
    }
}
