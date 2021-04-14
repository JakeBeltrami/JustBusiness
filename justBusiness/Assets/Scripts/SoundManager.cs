using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		This script handles sound effects for weapons and other objects.

	Parameters:


	Dependencies:
		Weapon.cs

	Author:
		Lauren

	Changelog:
		13-05-2019	Initial version
        15-05-2019  Restructured script for detecting weapon type

	Notes:


======================================================================
*/

public class SoundManager : MonoBehaviour
{
    public AudioClip enemyDie;

    // Gun Sounds
    public AudioClip pistolSound;
    public AudioClip punchSound;
    public AudioClip tommyGunSound;
    public AudioClip switchBooleanSound;
	public AudioClip tickSound; //sound for timer
	public AudioClip reloadSound; //sound for reload

    AudioSource audioSource;
    private Weapon weaponScript;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (GameObject.FindGameObjectWithTag("Pickup") != null)
        {
            weaponScript = GameObject.FindGameObjectWithTag("Pickup").GetComponent<Weapon>();
        }
    }

    public void PlayEnemyDeath()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.clip = enemyDie;
        audioSource.Play();
    }

    // Added by L.S. (15-05-2019)
    // Handles weapon firing SFX
    // Takes: string
    // Returns: nothing
    public void bulletSounds(string weap)
    {
        switch (weap)
        {
            case "pistol":
                audioSource.pitch = 0.2f;
                audioSource.PlayOneShot(pistolSound);
                break;
            case "tommygun":
                audioSource.PlayOneShot(tommyGunSound);
                break;
            default:
                break;
        }
    }

    public void PlayBooleanSound()
	{
        audioSource.pitch = 0.2f;
        audioSource.clip = switchBooleanSound;
        audioSource.Play();
    }

	public void Reload()
	{
		audioSource.PlayOneShot(reloadSound);
	}
	
		public void timerSound(float timer)
	{
		if(timer==1||timer==2||timer==3)
		{
			audioSource.PlayOneShot(tickSound);
		}
	}
		
}
