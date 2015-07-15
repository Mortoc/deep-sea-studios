using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

using DSS.States;
using DSS.Builds;

namespace DSS.UI
{
    public class StartMenuButton : MonoBehaviour
    {

        [SerializeField]
        private Image _notificationBadge;

        private int _notificationCount;
        public int NotificationCount
        {
            get { return _notificationCount; }
            set
            {
                _notificationCount = value;
                _notificationBadge.gameObject.SetActive(_notificationCount != 0);
                if (_notificationCount > 0)
                {
                    _notificationBadge.GetComponentInChildren<Text>().text = _notificationCount.ToString();
                }
            }
        }

        void Start()
        {
            NotificationCount = 2;
        }

        public void Clicked()
        {
            FindObjectOfType<WorkshopState>().GoToState<BuildsState>();
        }
        
    }
}
