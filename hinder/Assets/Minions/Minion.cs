using UnityEngine;
using System.Collections;

public class Minion : Being 
{
    [SerializeField]
    private float _totalPathTime = 10.0f;

    [SerializeField]
    private float _offsetAmount = 1.0f;
    private Vector3 _offset;

    [SerializeField]
    private float _attackRange = 3.0f;

    [SerializeField]
    private LayerMask _attackableLayers;

    [SerializeField]
    private Material _bulletMaterial;

    public void Init(ISpline path, LayerMask layer)
    {
        _offset = Random.onUnitSphere * Mathf.Lerp(0.0f, _offsetAmount, Random.value);
        if (Vector3.Distance(transform.position, path.PositionSample(0)) < 
            Vector3.Distance(transform.position, path.PositionSample(1)))
        {
            StartCoroutine(FollowPath(path, 0.0f, 1.0f));
        }
        else
        {
            StartCoroutine(FollowPath(path, 1.0f, 0.0f));
        }
    }

    private IEnumerator FollowPath(ISpline path, float startT, float endT)
    {
        var recipTotalTime = 1.0f / _totalPathTime;
        for (var time = 0.0f; time < _totalPathTime; time += Time.deltaTime)
        {
            var t = Mathf.Lerp(startT, endT, time * recipTotalTime);
            var pathPnt = path.PositionSample(t);
            var pathOffset = Quaternion.FromToRotation(Vector3.up, path.ForwardSample(t)) * _offset;
            transform.position = pathPnt + pathOffset;
            yield return 0;

            if (Physics2D.OverlapCircle(transform.position, _attackRange, _attackableLayers)) 
            {
                rigidbody2D.velocity = Vector2.zero;
                yield return StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        while (Physics2D.OverlapCircle(transform.position, _attackRange, _attackableLayers))
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _attackRange, _attackableLayers);
            //focus player
            var target = targets[Random.Range(0, targets.Length)].GetComponent<Being>();
            StartCoroutine(FireBulletAt(target));
            yield return 0; 
        }
        
        yield return 0;
    }

    IEnumerator FireBulletAt(Being enemy)
    {
        var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.localScale = Vector3.one * 0.1f;
        bullet.transform.position = transform.position;
        bullet.GetComponent<Renderer>().material = _bulletMaterial;
        var bulletSpeed = 12.0f;
        var hitDistance = bulletSpeed * 0.05f;

        while ((bullet.transform.position - enemy.transform.position).magnitude > hitDistance)
        {
            var bulletDirection = (enemy.transform.position - bullet.transform.position).normalized;
            bullet.transform.Translate(bulletDirection * bulletSpeed * Time.deltaTime);
            yield return 0;

            if (!enemy)
            {
                GameObject.Destroy(bullet);
                yield break;
            }
        }

        GameObject.Destroy(bullet);
        //enemy.TakeDamage(1.0f);
        
    }

   
}
