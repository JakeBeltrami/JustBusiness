using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GunAttack : BaseAttack
{
    private float bulletSpeed;
    private GameManager gameManager;

    public bool unlimitedAmmo = false;

    private Vector3 shootPosition; //Position from where the bullet will spawn from

    // Adding sound LS
    AudioSource audioSource;
    public AudioClip gunshotSound;

    public void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void Start()
    {
        // Set default values here
        damage = 1;
        bulletSpeed = 20f;
        range = 50f;
        audioSource = GetComponent<AudioSource>();
        //gunshotSound = Resources.Load<AudioClip>("PistolSFX/136766__mitchelk__gunshot001");
        gunshotSound = Resources.Load<AudioClip>("PistolSFX/Gunshot_Low");
        audioSource.clip = gunshotSound;
    }

    /// <summary>
    /// Shoot in a direction
    /// </summary>
    public override void Attack(Vector2 target, LayerMask layerMask)
    {
        if (ammo > 0)
        {
            audioSource.clip = gunshotSound;
            // Play sound
            audioSource.Play();
            //Debug.Log(audioSource.clip.name);

            // Use Arm-->Bullet Spawn
            shootPosition = transform.GetChild(0).GetChild(0).position;
            // Instatiate bullet
            GameObject b = Instantiate(Resources.Load<GameObject>("Weapons/Bullet"), shootPosition, Quaternion.identity);
            // Tag the bullet based on who shot it
            b.tag = transform.tag + "Bullet";
            // Add the appropriate bullet script
            b.AddComponent(Type.GetType(b.tag));
            // Assign shooter
            b.GetComponent<Bullet>().Shooter = GetComponent<Entity>();
            // Assign target
            b.GetComponent<Bullet>().Target = GetComponent<EntityInput>().Target.GetComponent<Entity>();
            Vector2 direction = (target - (Vector2)shootPosition).normalized;
            b.transform.right = direction;
            b.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

            // Reduce the bullet count and reset the cooldown
            if (!unlimitedAmmo)
            {
                ammo -= 1;
            }
            // Enemy will discard gun otherwise
            if (tag == "Enemy" && ammo == 0)
            {
                GetComponent<NewInventory>().Remove();
            }
        }
        // Discard if zero ammo?
    }
}
