using UnityEngine;

using System;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    private float _hitPoints = 10.0f;
    private float _damage = 0.0f;
    private float _speed = 0.05f;

    private float _pathT = 0.0f;

    public void Init(ISpline path, Target target)
    {
        StartCoroutine(FollowPath(path, target));
    }

    private IEnumerator FollowPath(ISpline path, Target target)
    {
        var rotationOffset = new Vector3(0.0f, 90.0f, 0.0f);
        _pathT = 0.0f;
        while(_pathT < 1.0f)
        {
            transform.position = path.PositionSample(_pathT);
            transform.forward = path.ForwardSample(_pathT);
            transform.Rotate(rotationOffset);
            _pathT += _speed * Time.deltaTime;
            yield return 0;
        }

        target.TakeDamage(1.0f);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        _damage += damage;
        if (_damage >= _hitPoints)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
