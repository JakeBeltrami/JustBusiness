using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnife : Bullet
{
	void Awake()
	{
		this.GetComponent<AudioSource>().Play();
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        // If we hit an Enemy
        if (col.CompareTag("Enemy"))
        {
            // Hurt the Enemy (Call Slow Down here on in that method)
            col.GetComponent<Enemy>().TakeDamage(damage, transform.position);
            // GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnManager>().GameState = TurnManager.State.EnemyTurn;
            Destroy(gameObject);
        }
        // If we hit a wall
        else if (col.CompareTag("Wall"))
        {
            // GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnManager>().GameState = TurnManager.State.EnemyTurn;
            Destroy(gameObject);
        }

        // NOTE: Will need some way to determine cover
    }
}
