using UnityEngine;
using System.Collections;

public class Minion : Being 
{
	public override void TimeToDie ()
	{
	}

    [SerializeField]
    private float _totalPathTime = 10.0f;

    [SerializeField]
    private float _offsetAmount = 1.0f;
    private Vector3 _offset;

    [SerializeField]
    private float _attackRange = 3.0f;

    
    private LayerMask _attackableLayers;


    public void Init(ISpline path)
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
                yield return StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        yield return 0;
        //while( shits in RangeAttribute ) 
        //{
        //    // attack it
        //}

        // returns back to follow path
    }
}
