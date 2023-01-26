using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void OnHit(float damage, Vector2 knockback)
    {
        throw new System.NotImplementedException();
    }
}
