using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int ammoCount; // ammo capacity of weapon

    private string weaponName; // name of the weapon

    public float fireRate; // The fire rate of the weapon. The amount of time that must pass before the weapon can fire again.

    public int damage; // Amount of damage

    public float range; // Bullet's distance

    public GameObject bullet;

    public float bulletSpeed;

    public float currentCooldown = 0;

    public string weaponType;

    private Vector2 currentTile = new Vector2(1, 1);
    public Vector2 CurrentTile { get { return currentTile; } }

    // Added by L.S. (15-05-2019)
    // For weapon SFX
    [HideInInspector]
    public SoundManager sound;

    void Start()
    {
        // bullet.GetComponent<Bullet>().range = range;
        bullet.GetComponent<Bullet>().damage = damage;
        currentCooldown = fireRate;

        currentTile = new Vector2((int)Mathf.Round(transform.position.x + 0.5f), (int)Mathf.Round(transform.position.y + 0.5f));
    }

    public virtual void Fire(Vector2 shootPosition, Vector2 targetPosition) // fires weapon. Overridden by subclasses
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown <= 0 && ammoCount > 0)
        {
            currentCooldown = fireRate;
            Vector2 direction = targetPosition - shootPosition;
            GameObject bulletProjectile = Instantiate(bullet, shootPosition, Quaternion.identity);
            bulletProjectile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

            sound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
            sound.bulletSounds(weaponType);
            ammoCount -= 1;
        }
        //if (ammoCount == 0)
        //{
        //    Destroy(this);
        //}
    }

    public Weapon()
    {

    }
}
