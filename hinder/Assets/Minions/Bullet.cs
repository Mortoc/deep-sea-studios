using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{

    [SerializeField]
    private float _bulletSpeed = 5;

    public void DoThing(Being enemy)
    {
        StartCoroutine (AttackEnemy(enemy));
    }

    IEnumerator AttackEnemy(Being enemy)
    {
        var hitDistance = _bulletSpeed * 0.05f;

        while (enemy && ((transform.position - enemy.transform.position).magnitude > hitDistance))
        {
            var bulletDirection = (enemy.transform.position - transform.position).normalized;
            transform.Translate(bulletDirection * _bulletSpeed * Time.deltaTime);
            yield return 0;
        }
		
		enemy.RecieveDamage(1.0f);
		GameObject.Destroy(gameObject);
    }
}
