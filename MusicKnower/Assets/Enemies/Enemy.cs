using UnityEngine;

using System;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    private float _hitPoints = 10.0f;
    private float _damage = 0.0f;

    private float _speed = 5.0f;

    private MusicKnower _knower;

    void Start()
    {
        _knower = GameObject.FindObjectOfType<MusicKnower>();
        _knower.OnBeat += ModifySpeed;
    }

    public void TakeDamage(float damage)
    {
        _damage += damage;
        if (_damage >= _hitPoints)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void ModifySpeed(MusicKnower.BeatInfo bi)
    {
        try
        {
            StartCoroutine(ModifySpeedCoroutine(bi));
        }
        catch(MissingReferenceException)
        {
            _knower.OnBeat -= ModifySpeed;
        }
    }

    private IEnumerator ModifySpeedCoroutine(MusicKnower.BeatInfo bi)
    {
        yield return 0;
    }

    private Coroutine _moveCoroutine = null;
    public void MoveToPosition(Vector3 position, Action onComplete)
    {
        if( _moveCoroutine != null )
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(AnimateMovement(position, onComplete));
    }

    private IEnumerator AnimateMovement(Vector3 position, Action onComplete)
    {
        var moveThresholdSqr = 0.5f;
        while((transform.position - position).sqrMagnitude > moveThresholdSqr)
        {
            var direction = (position - transform.position).normalized;
            transform.position = transform.position + (direction * _speed * Time.deltaTime);

            yield return 0;
        }
        onComplete();
        _knower.OnBeat -= ModifySpeed;
        GameObject.Destroy(gameObject);
    }
}
