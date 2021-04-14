using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BottleAttack : BaseAttack
{
    // public GameObject bullet;
    public float bulletSpeed = 10f;
    private GameManager gameManager;

    public bool unlimitedAmmo = false;

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

    /// <summary>
    /// Shoot in a direction
    /// </summary>
    public override void Attack(Vector2 target, LayerMask layerMask)
    {
        if (ammo > 0)
        {
            // Play sound and animation

            // Move this to be in a Player script
            /*foreach (GameObject e in gameManager.Enemies)
            {
                e.GetComponent<Enemy>().Alert(GetComponent<Entity>().Tile.Center);
            }*/

            // Transform.position will need to change to the guns firing point (How if it's sprite based?)
            GameObject b = Instantiate(Resources.Load<GameObject>("Weapons/Bottle"), transform.position, Quaternion.identity);
            b.tag = transform.tag + "Bottle"; // Assuming only player can use knives
            b.AddComponent(Type.GetType(b.tag)); // Adds the relevant Bullet script
            b.GetComponent<Rigidbody2D>().velocity = target * bulletSpeed;

            // Reduce the bullet count and reset the cooldown
            if (!unlimitedAmmo)
            {
                ammo -= 1;
            }
        }
        // Discard if zero ammo?
    }
}
