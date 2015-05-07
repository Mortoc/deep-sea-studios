using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
    public static class AudioSourceExt
    {
        private class AudioFadeHandler : MonoBehaviour
        {
            private Action _thenAction;

            public void Then(Action thenAction)
            {
                _thenAction += thenAction;
            }

            public AudioFadeHandler FadeSound(AudioSource src, float time, float targetVolume)
            {
                StartCoroutine(FadeSoundCoroutine(src, time, targetVolume));
                return this;
            }

            private IEnumerator FadeSoundCoroutine(AudioSource src, float time, float targetVolume)
            {
                var startVolume = src.volume;
                var invTime = 1.0f / time;
                
                for (var t = 0.0f; t < time; t += Time.deltaTime)
                {
                    src.volume = Mathf.Lerp(startVolume, targetVolume, t * invTime);
                    yield return 0;
                }

                src.volume = targetVolume;
                
                if( _thenAction != null )
                {
                    _thenAction();
                }

                Destroy(this);
            }
        }

        public static void FadeIn(this AudioSource src, float volume = 1.0f, float fadeTime = 0.2f)
        {
            src.volume = 0.0f;
            src.gameObject.AddComponent<AudioFadeHandler>()
                .FadeSound(src, fadeTime, volume);
            src.Play();
        }

        public static void FadeOut(this AudioSource src, float fadeTime = 0.2f)
        {
            src.gameObject.AddComponent<AudioFadeHandler>()
                .FadeSound(src, fadeTime, 0.0f)
                .Then(src.Stop);
        }
    }
}
