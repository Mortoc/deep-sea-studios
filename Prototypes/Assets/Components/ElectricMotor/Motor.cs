using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    [RequireComponent(typeof(AudioSource))]
    public class Motor : PowerableObject
    {
        [SerializeField]
        private HingeJoint _motorJoint;

        [SerializeField]
        private AudioClip _runningSound;

        void Awake()
        {
            base.Awake();
        }

        public override void On()
        {
            if (_motorJoint.useMotor != true)
            {
                _motorJoint.useMotor = true;
            }
            var audio = GetComponent<AudioSource>();

            audio.clip = _runningSound;
            audio.loop = true;

            audio.FadeIn(1.0f);
        }

        public override void Off()
        {
            _motorJoint.useMotor = false;
            GetComponent<AudioSource>().FadeOut();
        }
    }
}
