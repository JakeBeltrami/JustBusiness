using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : MonoBehaviour
{
    private GameObject soundManager; //Added by K.C. for reloading sound
    private float timeLeft;
    private AudioSource audioSource;
    public AudioClip reloadSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the Player
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInventory inv = other.GetComponent<PlayerInventory>();
            // If they have a weapon and it's a gun
            if (inv.weapon != null && inv.weapon is GunAttack)
            {
                // Increase the ammo
                audioSource.PlayOneShot(reloadSound);
                other.GetComponent<GunAttack>().Ammo += 1;
                GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(InitDestroy());
                // Destroy(gameObject);
            }
        }
    }

    private IEnumerator InitDestroy(float countdownValue = 1f)
    {
        timeLeft = countdownValue;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timeLeft--;
            if (timeLeft < 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
