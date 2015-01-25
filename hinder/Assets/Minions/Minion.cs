using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private LayerMask _wallLayer;

    [SerializeField]
    private GameObject _bulletPrefab;

    

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
				Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _attackRange, _attackableLayers);
				List<Collider2D> realTargets = AttackableTargets(targets);
				if (realTargets.Count > 0)
				{
	                rigidbody2D.velocity = Vector2.zero;
	                yield return StartCoroutine(Attack());
				}
            }
        }
    }

	private List<Collider2D> AttackableTargets(Collider2D[] targets)
	{
		List<Collider2D> realTargets = new List<Collider2D>();
		foreach (var target in targets)
		{
			if (!Physics2D.Linecast(transform.position, target.gameObject.transform.position, _wallLayer))
			{
				realTargets.Add(target);
			}
		}
		return realTargets;
	}

    private IEnumerator Attack()
    {
		while (AttackableTargets(Physics2D.OverlapCircleAll(transform.position, _attackRange, _attackableLayers)).Count > 0)
        {
			var targets = AttackableTargets(Physics2D.OverlapCircleAll(transform.position, _attackRange, _attackableLayers));
            Being target = null;
            foreach (var t in targets)
            {
        		target = t.GetComponent<Player>();
				if (target)
                    break;
            }

            if (!target)
            {
                target = targets[Random.Range(0, targets.Count)].GetComponent<Being>();
            }

            GameObject go = (GameObject)Instantiate(_bulletPrefab);
            go.transform.position = transform.position;
            go.GetComponent<Bullet>().DoThing(target);
            
            yield return new WaitForSeconds(1); 
        }
        
        yield return 0;
    }


    public override void TimeToDie()
    {
        Destroy(gameObject);
    }
}
