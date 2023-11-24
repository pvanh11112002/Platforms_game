using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockback = new Vector2(0, 0);
    Rigidbody2D rb;
    
    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y); // Config hướng đẩy lùi của attack
            bool gotHit = damageable.Hit(damage, deliveredKnockback);
            if (gotHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
