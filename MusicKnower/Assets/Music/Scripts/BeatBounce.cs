using UnityEngine;
using System.Collections;

public class BeatBounce : MonoBehaviour
{
    public float upscaleAmount = 3.0f;
    public bool rotateToo = true;
    public float _rotateAmount = 15.0f;
    private Vector3 _initialScale;

    void Start()
    {
        _initialScale = transform.localScale;
        var musicKnower = GetComponent<MusicKnower>();
        musicKnower.OnBeat += b => StartCoroutine(DoBeat(b));
    }

    IEnumerator DoBeat(MusicKnower.BeatInfo bi)
    {
        var debeatTime = bi.Duration * 0.75f;
        var recipDebeatTime = 1.0f / debeatTime;
        transform.localScale = _initialScale * upscaleAmount;

        var rotAmount = _rotateAmount;
        if (bi.BeatNumber % 2 == 0) rotAmount *= -1.0f;

        var targetRot = Quaternion.Euler(0.0f, 0.0f, rotAmount);

        var startRot = transform.rotation;
        for (var beatAnimationTime = 0.0f; beatAnimationTime < debeatTime; beatAnimationTime += Time.deltaTime)
        {
            var t = recipDebeatTime * beatAnimationTime;
            transform.localScale = _initialScale * Mathf.Lerp(upscaleAmount, 1.0f, t);

            if( rotateToo ) transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return 0;
        }

        transform.localScale = _initialScale;
        transform.rotation = targetRot;
    }
}
