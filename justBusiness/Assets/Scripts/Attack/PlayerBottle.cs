using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottle : Bullet
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // If we hit an Enemy
        if (col.CompareTag("Enemy"))
        {
            // Hurt the Enemy (Call Slow Down here on in that method)
            col.GetComponent<Enemy>().TakeDamage(damage, transform.position);
            Destroy(gameObject);
        }
        // If we hit a wall
        else if (col.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        // NOTE: Will need some way to determine cover
    }
}
