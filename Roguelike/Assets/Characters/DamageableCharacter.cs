using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableCharacter : MonoBehaviour, IDamageable
{
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
    Rigidbody2D rb;
    
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;

        // Apply force to character
        rb.AddForce(knockback, ForceMode2D.Impulse);
    }

    void Death()
    {
        Destroy(gameObject);
    }
}