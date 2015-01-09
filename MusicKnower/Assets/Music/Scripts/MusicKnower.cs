using UnityEngine;

using System;
using System.Collections;

using SimpleJSON;

[RequireComponent(typeof(AudioSource))]
public class MusicKnower : MonoBehaviour 
{
    public struct BeatInfo
    {
        public float Start { get; set; }
        public float Duration { get; set; }
        public int BeatNumber { get; set; }
    }

    private BeatInfo[] _beats;

    public event Action<BeatInfo> OnBeat;

    [SerializeField]
    private TextAsset _echoNestAnalysis;

    [SerializeField]
    private AudioClip _song;

    void Start()
    {
        LoadAnalysis();
        StartCoroutine(PlaySong());
    }

    void LoadAnalysis()
    {
        var json = JSON.Parse(_echoNestAnalysis.text);
        var beats = json["beats"];
        _beats = new BeatInfo[beats.Count];

        for (int i = 0; i < beats.Count; i++)
        {
            _beats[i] = new BeatInfo() {
                Start = beats[i]["start"].AsFloat,
                Duration = beats[i]["duration"].AsFloat,
                BeatNumber = i
            };

        }
    }

    IEnumerator PlaySong()
    {
        var audioComponent = GetComponent<AudioSource>();
        audioComponent.clip = _song;
        audioComponent.Play();

        int currentBeat = -1;
        while (audioComponent.isPlaying)
        {
            if (audioComponent.time >= _beats[currentBeat + 1].Start)
            {
                currentBeat++;
                if (OnBeat != null)
                {
                    OnBeat(_beats[currentBeat]);
                }

                if (currentBeat == _beats.Length)
                {
                    yield break;
                }
            }
          
            yield return 0;
        }
    }

}
