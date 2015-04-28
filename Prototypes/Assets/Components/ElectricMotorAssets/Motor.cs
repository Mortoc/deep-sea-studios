using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public class Motor : MonoBehaviour
    {
        [SerializeField]
        private Plug _plug;

        [SerializeField]
        private HingeJoint _motorJoint;

        void OnEnable()
        {
            _plug.OnPower += TurnMotorOn;
            _plug.OnPowerLoss += TurnMotorOff;
        }

        void OnDisable()
        {
            _plug.OnPower -= TurnMotorOn;
            _plug.OnPowerLoss -= TurnMotorOff;
        }

        private void TurnMotorOn()
        {

        }

        private void TurnMotorOff()
        {

        }
    }
}
