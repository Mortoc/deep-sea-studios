using UnityEngine;
using System.Collections;

public class Minion : Being 
{
    private float _hitPoints = 10.0f;
    private float _damage = 0.0f;

    void Update()
    {
        rigidbody2D.AddForce(Vector2.right * 3);
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
