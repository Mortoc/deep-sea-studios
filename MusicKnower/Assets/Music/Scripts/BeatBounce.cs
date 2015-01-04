using UnityEngine;
using System.Collections;

public class BeatBounce : MonoBehaviour
{
    public float upscaleAmount = 3.0f;

    void Start()
    {
        var musicKnower = GetComponent<MusicKnower>();
        musicKnower.OnBeat += b => StartCoroutine(DoBeat(b.Duration));
    }

    IEnumerator DoBeat(float beatTiming)
    {
        var debeatTime = beatTiming * 0.75f;
        var recipDebeatTime = 1.0f / debeatTime;
        transform.localScale = Vector3.one * upscaleAmount;
        for (var beatAnimationTime = 0.0f; beatAnimationTime < debeatTime; beatAnimationTime += Time.deltaTime)
        {
            var t = recipDebeatTime * beatAnimationTime;
            transform.localScale = Vector3.one * Mathf.Lerp(upscaleAmount, 1.0f, t);
            yield return 0;
        }

        transform.localScale = Vector3.one;
    }
}
