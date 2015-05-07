using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    [RequireComponent(typeof(AudioSource))]
    public class Wheel : MonoBehaviour
    {
        [SerializeField]
        private Plug _plug;

        [SerializeField]
        private HingeJoint _wheelJoint;

        [SerializeField]
        private AudioClip _runningSound;
        private float _originalVolumne;

        void Awake()
        {
            var audio = GetComponent<AudioSource>();
            audio.pitch = Mathf.Lerp(0.9f, 1.1f, Rand.value);
            _originalVolumne = audio.volume;
        }

        void OnEnable()
        {
            TurnWheelOn();
            _plug.OnPower += TurnWheelOn;
            _plug.OnPowerLoss += TurnWheelOff;
        }

        void OnDisable()
        {
            _plug.OnPower -= TurnWheelOn;
            _plug.OnPowerLoss -= TurnWheelOff;
        }

        private void TurnWheelOn()
        {
            _wheelJoint.useMotor = true;
            var audio = GetComponent<AudioSource>();

            audio.clip = _runningSound;
            audio.loop = true;
            
            audio.FadeIn(_originalVolumne);
        }

        private void TurnWheelOff()
        {
            _wheelJoint.useMotor = false;
            GetComponent<AudioSource>().FadeOut();
        }

    }
}
