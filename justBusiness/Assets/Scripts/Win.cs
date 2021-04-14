using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    public SpriteRenderer rend;
    public Sprite open;
    public AudioClip doorOpenSound;

    protected AudioSource audioSource;
    protected GameManager gameManager;
    protected float delayTime = 0f;
    public bool sound = true;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void DelayedAction()
    {
        StartCoroutine(gameManager.NextLevel());
    }

    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        // If player has collided with the exit
        if (collision.CompareTag("Player"))
        {
            // Check if the level is won
            if (gameManager.LevelWon && collision.GetComponent<PlayerInput>().Kick)
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
