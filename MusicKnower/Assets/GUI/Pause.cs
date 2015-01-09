using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour
{
    public float _animateTime = 0.5f;
    public Image _fadePanel;
    public float _fadeAmount = 0.66f;

    private Coroutine _animateTimeCoroutine = null;

    public void PauseGame(Text textWidget)
    {
        if (_animateTimeCoroutine != null)
            StopCoroutine(_animateTimeCoroutine);

        if (Time.timeScale < 1.0f)
        {
            textWidget.text = "Pause";
            _animateTimeCoroutine = StartCoroutine(AnimatePause(1.0f));
        }
        else
        {
            textWidget.text = "Resume";
            _animateTimeCoroutine = StartCoroutine(AnimatePause(0.0f));
        }
    }

    IEnumerator AnimatePause(float toTimeScale)
    {
        var audioComponent = GetComponent<AudioSource>();
        var fadeColor = _fadePanel.color;
        
        float recipAnimTime = 1.0f / _animateTime;
        float startScale = Time.timeScale;
        for(float time = 0.0f; time < _animateTime; time += Mathf.Max(Time.deltaTime, 0.01f))
        {
            Time.timeScale = Mathf.SmoothStep(startScale, toTimeScale, recipAnimTime * time);
            audioComponent.pitch = Time.timeScale;
            audioComponent.volume = Time.timeScale;
            fadeColor.a = _fadeAmount * (1.0f - Time.timeScale);
            _fadePanel.color = fadeColor;
            yield return 0;
        }
        Time.timeScale = toTimeScale; 
        audioComponent.pitch = Time.timeScale;
        audioComponent.volume = Time.timeScale;
        fadeColor.a = _fadeAmount * (1.0f - Time.timeScale);
        _fadePanel.color = fadeColor;
    }
}
