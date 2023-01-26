using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage;
    public float knockbackForce;

    void OnCollisionEnter2D(Collision2D col)
    {
        IDamageable damageable = col.collider.GetComponent<IDamageable>();

        if(damageable != null)
        {
            Collider2D collider = col.collider;
            // Calculate direction between character and enemy
            Vector3 position = transform.position;

            Vector2 direction = (Vector2) (collider.gameObject.transform.position - position).normalized;
            Vector2 knockback = direction * knockbackForce;
            damageable.OnHit(damage, knockback);
        }
    }
}