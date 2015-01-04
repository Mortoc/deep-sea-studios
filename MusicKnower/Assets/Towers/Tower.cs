using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Material _bulletMaterial;

    [SerializeField]
    private float _maxRange = 10.0f;

    void Start()
    {
        var knower = GameObject.FindObjectOfType<MusicKnower>();
        knower.OnBeat += OnBeat;
    }

    void OnBeat(MusicKnower.BeatInfo bi)
    {
        Enemy nearestEnemy = null;
        float nearest = Mathf.Infinity;
        foreach(var collider in Physics.OverlapSphere(transform.position, _maxRange)) 
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
            StartCoroutine(FireBulletAt(nearestEnemy));
        }
    }

    IEnumerator FireBulletAt(Enemy enemy)
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
