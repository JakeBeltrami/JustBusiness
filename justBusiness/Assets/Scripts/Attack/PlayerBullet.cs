using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    void Awake()
    {
        GetComponent<AudioSource>().Play();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // If we hit an Enemy
        if (col.CompareTag("Enemy"))
        {
            // Hurt the Enemy (Call Slow Down here on in that method)
            Entity hit = col.GetComponent<Entity>();
            if (hit == Target)
            {
                col.GetComponent<Enemy>().TakeDamage(damage, transform.position);
                Destroy(gameObject);
            }
        }
        // If we hit a wall
        else if (col.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public override void OnDestroy()
    {
        // End turn
        shooter.GetComponent<PlayerInput>().DelayTurn(0.5f);
        base.OnDestroy();
    }
}
