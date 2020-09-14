using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Health health;
    public Rigidbody2D rbody;

    public Vector2 knockbackAmount = new Vector2(-1f, 1f);

    void Awake()
    {
        if (health == null)
            health = GetComponent<Health>();
        if (rbody == null)
            rbody = GetComponent<Rigidbody2D>();
        health.OnTakeDamageDirectional += HandleKnockback;
    }

    public void HandleKnockback(Vector2 direction)
    {
        rbody.velocity = new Vector2(knockbackAmount.x * direction.x, knockbackAmount.y);
    }

}
