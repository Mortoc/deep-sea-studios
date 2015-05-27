using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    [RequireComponent(typeof(AudioSource))]
    public class Wheel : PowerableObject
    {
        [SerializeField]
        private HingeJoint _wheelJoint;

        [SerializeField]
        private AudioClip _runningSound;
        private float _originalVolumne;

        void Awake()
        {
            base.Awake();

            var audio = GetComponent<AudioSource>();
            audio.pitch = Mathf.Lerp(0.9f, 1.1f, Rand.value);
            _originalVolumne = audio.volume;
        }

        public override void On()
        {
            if (_wheelJoint.useMotor == false)
            {
                _wheelJoint.useMotor = true;
                var audio = GetComponent<AudioSource>();

                audio.clip = _runningSound;
                audio.loop = true;

                audio.FadeIn(_originalVolumne);
            }
        }

        public override void Off()
        {
            _wheelJoint.useMotor = false;
            GetComponent<AudioSource>().FadeOut();
        }
    }
}
