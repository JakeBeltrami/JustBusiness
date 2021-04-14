using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private LayerMask envMask;
    private float offset;

    public override void Start()
    {
        base.Start();
        envMask = LayerMask.GetMask("Environment", "Low Environment");
        offset = gameManager.Player.GetComponent<Collider2D>().bounds.extents.y;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.CompareTag("Environment") || col.CompareTag("Player")) && gameManager.Player.IsInCover)
        {
            CheckCover();
        }
        // Added by W.T. (01-10-2019)
        // Check if we hit a player and they're behind cover
        // if (col.CompareTag("Player") && gameManager.Player.IsInCover)
        // {
        //     // Get what direction the bullet's striking from relative to the player's location
        //     float angle;
        //     Vector2 dir = transform.position - col.transform.position;
        //     float sign, offset;
        //     sign = (dir.y >= 0) ? 1 : -1;
        //     offset = (sign >= 0) ? 0 : 360;
        //     angle = Vector2.Angle(Vector2.right, dir) * sign + offset;

        //     // 06-10-2019
        //     // Reduced to 4 directions for simplicity

        //     if ((angle >= 45 && angle <= 135) && col.GetComponent<Player>().BulletsCantHitFromTheseDirections.Contains("N"))
        //     {
        //         Destroy(gameObject);
        //     }
        //     else if ((angle > 135 && angle <= 225) && col.GetComponent<Player>().BulletsCantHitFromTheseDirections.Contains("W"))
        //     {
        //         Destroy(gameObject);
        //     }
        //     else if ((angle > 225 && angle <= 315) && col.GetComponent<Player>().BulletsCantHitFromTheseDirections.Contains("S"))
        //     {
        //         Destroy(gameObject);
        //     }
        //     else if ((angle > 315 && angle <= 360 || angle >= 0 && angle < 45) && col.GetComponent<Player>().BulletsCantHitFromTheseDirections.Contains("E"))
        //     {
        //         Destroy(gameObject);
        //     }
        //     else
        //     {
        //         // If they aren't physically behind something then apply damage
        //         col.GetComponent<Player>().TakeDamage(damage, transform.position);
        //         Destroy(gameObject);
        //     }
        // }
        // If they aren't behind cover then that means we hit them out in the open
        else if (col.CompareTag("Player"))
        {
            // Hurt the Player
            gameManager.Player.TakeDamage(damage, transform.position);
            Destroy(gameObject);
        }
        // If we hit a wall
        else if (col.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        // Add code to check if Environment
        // Check direction
        // Cast ray in direction using length of 1
        // If hit player, destroy bullet
    }

    /// <summary>
    /// If Player is in cover, check to see if we are protected
    /// </summary>
    public void CheckCover()
    {
        Vector2 origin = gameManager.Player.Tile.Center;
        Vector2 bulletPos = transform.position;
        // Offset to match tile position
        bulletPos.y -= offset;
        // Direction is inverse of bullet velocity
        Vector2 direction = (bulletPos - origin).normalized;
        // If bullet is close to player
        if (Vector2.Distance(origin, bulletPos) <= 1.5f)
        {
            // Debug.Log("Close enough");
            // Raycast from player to bullet to check cover
            bool hit = Physics2D.Raycast(origin, direction, 1.5f, envMask);
            if (hit)
            {
                Debug.Log("Destroyed bullet coming from: " + direction);
                Destroy(gameObject);
            }
            else
            {
                // Hurt the Player
                gameManager.Player.TakeDamage(damage, transform.position);
                Destroy(gameObject);
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        gameManager.TurnManager.EnemyCount++;
    }
}
