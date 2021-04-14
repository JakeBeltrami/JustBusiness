using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinNoDoor : Win
{
    public override void OnTriggerStay2D(Collider2D collision)
    {
        // If player has collided with the exit
        if (collision.CompareTag("Player"))
        {
            // Check if the level is won
            if (gameManager.LevelWon)
            {
                // Play kick animaiton
                if (rend != null)
                {
                    rend.sprite = open;
                }
                Invoke("DelayedAction", delayTime);
                if (sound)
                {
                    audioSource.PlayOneShot(doorOpenSound);
                    sound = false;
                }
            }
        }

    }
}
