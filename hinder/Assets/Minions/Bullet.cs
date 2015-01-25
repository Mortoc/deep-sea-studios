using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    
    [SerializeField]
    private Material _bulletMaterial;

    [SerializeField]
    private float _bulletSpeed = 5;

    public void DoThing(Being enemy)
    {
        StartCoroutine (Thing(enemy));
    }
    IEnumerator Thing(Being enemy)
    {
        var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.localScale = Vector3.one * 0.1f;
        bullet.transform.position = transform.position;
        bullet.GetComponent<Renderer>().material = _bulletMaterial;

        var hitDistance = _bulletSpeed * 0.05f;

        while (enemy && ((bullet.transform.position - enemy.transform.position).magnitude > hitDistance))
        {
            var bulletDirection = (enemy.transform.position - bullet.transform.position).normalized;
            bullet.transform.Translate(bulletDirection * _bulletSpeed * Time.deltaTime);
            yield return 0;

            if (!enemy)
            {
                GameObject.Destroy(bullet);
                Destroy(gameObject);
                yield break;
            }
        }

        GameObject.Destroy(bullet);
        Destroy(gameObject);
        enemy.RecieveDamage(1.0f);
    }
}
