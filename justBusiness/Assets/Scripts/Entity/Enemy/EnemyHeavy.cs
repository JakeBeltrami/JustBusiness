using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

    Purpose:
        Behaviour for Grunt enemy type

    Parameters:
        None

    Dependencies:
        Player.cs
        CameraShake.cs

    Author:
        Phil

    Changelog:
        (Trent) Changed to inherit from Enemy as only TakeDamage needed changing

======================================================================
*/

public class EnemyHeavy : Enemy
{
    public AudioClip injured;
    public override void TakeDamage(int amount, Vector2 source, string type = "none")
    {
        // Play animation and sound

        //Debug.Log("took damage! " + health + " OUT OF " + maxHP);
        if (health == maxHP && type == "melee")
        {
            // Counterattack
            SetDirection(player.transform.position);
            melee.Attack(player.GetComponent<BoxCollider2D>().bounds.center, myInput.Mask);
            // player.GetComponent<Player>().TakeDamage(1, transform.position, "melee");
        }

        health -= amount;

        if (health <= 0)
        {
            // Spawn blood
            blood.SpawnBlood(GetComponent<BoxCollider2D>().bounds.center, source, colour);
            Die();
        }
        else
        {
            // Spawn a little blood
            animator.SetBool("hit", true);
            blood.SpawnBlood(GetComponent<BoxCollider2D>().bounds.center, source, colour, true);
            GetComponent<AudioSource>().clip = injured;
            GetComponent<AudioSource>().Play();
            if (myState != State.Aiming)
            {
                myState = State.Agro;
            }
            StartCoroutine(cameraShake.ShakeForSeconds(1f, 2f, 0.1f, 0.1f, 0.5f, false));
            // Agro(player.transform.position);
            gameManager.GetComponent<TurnManager>().EnemyCount++;
        }
    }

    public void FinishedHurting()
    {
        animator.SetBool("hit", false);
    }
}
