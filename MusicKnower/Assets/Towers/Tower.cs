using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    private static int s_towerNum = 0;
    private int _towerNum;
    private int _fireBeat;

    [SerializeField]
    private Material _bulletMaterial;

    [SerializeField]
    private int _nthBeatFire = 5;
	
    [SerializeField]
    private float _maxRange = 10.0f;

	private int _lastBeatNum;
	private int _lastUsedAbiltyBeatNum = 0;
	private int _abilityTolerance = 1;

    void Start()
    {
        _towerNum = s_towerNum++;
        _fireBeat = _towerNum % _nthBeatFire;
        var knower = GameObject.FindObjectOfType<MusicKnower>();
        knower.OnBeat += OnBeat;
    }

	void OnMouseDown()
	{
		if (Mathf.Abs((_lastBeatNum % _nthBeatFire) - _fireBeat) <= _abilityTolerance)
		{
			if (_lastBeatNum - _lastUsedAbiltyBeatNum > _abilityTolerance * 2)
			{
				ShootAtEnemy(3);
				_lastUsedAbiltyBeatNum = _lastBeatNum;
			}
		}
	}

    void OnBeat(MusicKnower.BeatInfo bi)
    {
		Behaviour h = (Behaviour)GetComponent("Halo");
		h.enabled = false;

		_lastBeatNum = bi.BeatNumber;
        if (bi.BeatNumber % _nthBeatFire != _fireBeat) return;

		h.enabled = true;

		ShootAtEnemy(1);
    }

	void ShootAtEnemy(int numShots)
	{
		Enemy nearestEnemy = null;
		float nearest = Mathf.Infinity;
		
		foreach(var collider in Physics2D.OverlapCircleAll(transform.position, _maxRange)) 
		{
			var thisDist = (collider.transform.position - transform.position).magnitude;
			if( collider.GetComponent<Enemy>() && thisDist < nearest ) 
			{
				nearestEnemy = collider.GetComponent<Enemy>();
				nearest = thisDist;
			}
		}
		
		if( nearestEnemy ) 
		{
			StartCoroutine(FireBulletAt(nearestEnemy, numShots));
		}
	}
	
	IEnumerator FireBulletAt(Enemy enemy, int numShots)
	{
		for (int i = 0; i < numShots; i++)
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
	        enemy.TakeDamage(1.0f);
		}
    }
}
