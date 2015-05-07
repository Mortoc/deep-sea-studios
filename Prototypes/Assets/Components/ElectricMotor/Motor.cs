using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    [RequireComponent(typeof(AudioSource))]
    public class Motor : MonoBehaviour
    {
        [SerializeField]
        private Plug _plug;

        [SerializeField]
        private HingeJoint _motorJoint;

        [SerializeField]
        private AudioClip _runningSound;

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
            _motorJoint.useMotor = true;
            var audio = GetComponent<AudioSource>();

            Debug.Log("Fading in sound");
            audio.clip = _runningSound;
            audio.loop = true;

            audio.FadeIn(1.0f);
        }

        private void TurnMotorOff()
        {
            _motorJoint.useMotor = false;

            Debug.Log("Fading out sound");
            GetComponent<AudioSource>().FadeOut();
        }
    }
}
