using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class KnifeAttack : BaseAttack
{
    public float thrownSpeed = 20f;
    private GameManager gameManager;

    public bool unlimitedAmmo = false;

    private Vector2 shootPosition; //Position from where the knife will spawn from


    public void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        ammo = 1;
    }

    public void Start()
    {
        // Set default values here
        damage = 1;
    }

    public void Update()
    {
        //		if (Input.GetKeyDown(KeyCode.KeypadPlus))
        //		{
        //			GameObject b = Instantiate(Resources.Load<GameObject>("Weapons/Knife"), transform.position, Quaternion.identity);
        //			b.tag = "PlayerBullet"; // Assuming only player can use knives
        //			b.AddComponent(Type.GetType(b.tag)); // Adds the relevant Bullet script
        //			b.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        //		}
    }


    /// <summary>
    /// Shoot in a direction
    /// </summary>
    public override void Attack(Vector2 target, LayerMask layerMask)
    {
        if (ammo > 0)
        {
            shootPosition = transform.GetChild(0).GetChild(0).position;
            // Calculate direction
            Vector2 direction = target - shootPosition;
            // Play sound ?

            // Throw from Arm-->Bullet Spawn
            GameObject knife = Instantiate(Resources.Load<GameObject>("Weapons/Knife"), shootPosition, Quaternion.identity);
            // Play animation
            knife.GetComponent<Animator>().SetFloat("x", direction.x);
            knife.GetComponent<Animator>().SetFloat("y", direction.y);
			knife.GetComponent<Animation>().Play();
            // Set properties for functionality
            knife.tag = "PlayerBullet"; // Assuming only player can use knives
            knife.AddComponent<PlayerBullet>(); // Adds the relevant Bullet script
            knife.GetComponent<Rigidbody2D>().velocity = direction.normalized * thrownSpeed;

            // Reduce the bullet count and reset the cooldown
            if (!unlimitedAmmo)
            {
                ammo -= 1;
            }
            if (ammo == 0)
            {
                // Discard weapon
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().Remove();
            }
        }
    }
}
