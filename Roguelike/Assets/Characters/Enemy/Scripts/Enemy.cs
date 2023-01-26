using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    Rigidbody2D rb;
    public float Health
    {
        set
        {
            _health = value;

            if(_health <= 0)
            {
                Death();
            }
        }

        get
        {
            return _health;
        }
    }
    public float _health;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;

        // Apply force to slime
        rb.AddForce(knockback);
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
