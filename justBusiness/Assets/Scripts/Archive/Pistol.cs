//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class Pistol : Gun
//{
//    public bool test = true; // temporary variable to test functionality
//    public float currentCooldown = 0;
//
//    public AudioClip pistol;
//    AudioSource audioSource;
//
//    public Pistol(int currentAmmo = 6)
//    {
//        ammoCount = currentAmmo;
//        fireRate = 1f;
//		bulletSpeed = 1f;
//    }
//
//    // This will spawn bullets upon being called as long as the player has enough ammo and isn't trying to shoot too quickly;
//    //When calling this function on a player/enemy passn that objects transform to the function so the bullets know where to spawn.
//	public override void Fire(Vector2 shootPosition, Vector2 targetPosition) 
//    {
//		currentCooldown -= Time.deltaTime;
//    
//        //Debug.Log("FIRING");
//        //Debug.Log(currentCooldown + " cooldown");
//        //Debug.Log(ammoCount + " ammo");
//
//        if (currentCooldown <= 0 && ammoCount > 0)
//        {
//            //audioSource.clip = pistol;
//            //audioSource.Play();
//            currentCooldown = fireRate;
//            //Debug.Log("SHOULD SPAWN");
//            //Instantiate(projectile, pos.position + new Vector3(0.9f, 0, 0), new Quaternion(0, 0, pos.rotation.z, 0));
//            //Debug.Log(transform.position);
//
//			//Rigidbody2D bulletProjectile = Instantiate (bullet, pos.position + new Vector3(0.9f, 0, 0) ,transform.rotation) as Rigidbody2D;
//			//bulletProjectile.velocity = transform.up * bulletSpeed;
//
//			//Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
//			//Vector2 shootPosition = new Vector2(pos.transform.position.x, pos.transform.position.y);
//			Vector2 direction = targetPosition - shootPosition;
//			Rigidbody2D bulletProjectile = Instantiate (bullet, shootPosition, Quaternion.identity) as Rigidbody2D;
//			bulletProjectile.velocity = direction * bulletSpeed;
//            audioSource.PlayOneShot(pistol);
//            
//
//            ammoCount -= 1;
//        }
//    }
//
//
//    // Start is called before the first frame update
//    void Start()
//    {
//        audioSource = GetComponent<AudioSource>();
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//		
//    }
//}
