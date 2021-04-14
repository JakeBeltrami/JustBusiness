using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		Controls player's bullets

	Parameters:
		None

	Dependencies:

	Author:
		Kshitij Choudhary

	Changelog:

	Notes:
        Might help to split into Enemy and Player bullet?

======================================================================
*/

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    // public float range = 1f;

    protected Entity shooter;
    protected Entity target;

    public GameManager gameManager;

    // private SlowDown slowDown;

    //public AudioClip punchSound;
    //AudioSource audioSource;

    public int Damage { get { return damage; } set { damage = value; } }
    public Entity Shooter { get { return shooter; } set { shooter = value; } }
    public Entity Target {  get { return target; } set { target = value; } }

    public virtual void Start()
    {
        //player = GameObject.Find("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        // slowDown = gameManager.GetComponent<SlowDown>();
        //audioSource = GetComponent<AudioSource>();
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public virtual void OnDestroy()
    {
        shooter.Animator.SetBool("aiming", false);
    }

    // void Update()
    // {
    //     if (shooter != null)
    //     {
    //         if (Vector3.Distance(transform.position, shooter.transform.position) > range)
    //         {
    //             Destroy(gameObject);
    //         }
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }


    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (gameObject.tag == "Bullet")
    //     {
    //         if (collision.tag == "Player")
    //         {
    //         }
    //         else
    //         {
    //             if (collision.tag == "Enemy")
    //             {
    //                 //if(this.name == "PlayerPunch(Clone)")
    //                 //{
    //                 //audioSource.clip = punchSound;
    //                 //audioSource.pitch = 1f;
    //                 //audioSource.Play();

    //                 //}

    //                 // Spawn blood
    //                 gameManager.GetComponent<Blood>().SpawnBlood(collision, transform.position);

    //                 collision.GetComponent<Enemy>().TakeDamage(damage);
    //                 Destroy(gameObject);
    //             }
    //             else if (collision.tag == "Wall")
    //             {
    //                 Destroy(gameObject);
    //             }
    //         }
    //     }
    //     else if (gameObject.tag == "Enemy")
    //     {
    //         if (collision.tag == "Enemy")
    //         {

    //         }
    //         else
    //         {
    //             if (collision.tag == "Player")
    //             {
    //                 if (this.name == "EnemyPunch(clone)")
    //                 {
    //                     //audioSource.clip = punchSound;
    //                     //audioSource.pitch = 0.6f;
    //                     //audioSource.Play();
    //                 }


    //                 Camera.main.GetComponent<CameraMovement>().shake = true;
    //                 collision.GetComponent<Player>().TakeDamage(damage);

    //                 Destroy(gameObject);
    //             }
    //             else if (collision.tag == "Wall")
    //             {
    //                 Destroy(gameObject);
    //             }
    //         }

    //     }
    //     else
    //     {
    //     }

    // }

    // NOTE: Commented out cleaned up version DON'T DELETE
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     // If player bullet collides with an enemy
    //     if (gameObject.tag == "PlayerBullet" && collision.tag == "Enemy")
    //     {


    //         GameObject blood = Instantiate(bloodParticle, collision.transform.position, Quaternion.identity);
    //         GameObject blood_pool = Instantiate(bloodPool, collision.transform.position, Quaternion.identity);
    //         GameObject blood_pool_sprite = Instantiate(bloodPoolSprite, collision.transform.position, Quaternion.identity);
    //         GameObject.Find("Main Camera").GetComponent<MicroCameraShake>().shakeTrue = true;
    //         blood.transform.right = (collision.transform.position - transform.position);


    //         collision.GetComponent<Enemy>().health -= damage;
    //         Destroy(gameObject);

    //     }
    //     // if an enemy bullet collides with the player
    //     else if (gameObject.tag == "EnemyBullet" && collision.tag == "Player")
    //     {
    //         Camera.main.GetComponent<CameraMovement>().shake = true;
    //         DisplayRed();

    //         Destroy(gameObject);
    //     }
    //     // Collision with wall
    //     else if (collision.tag == "Wall")
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // Added by Jake, slows down time when bullet despawns rather than when enemy dies (Can probably be slowed down somewhere else)
    // private void OnDestroy()
    // {
    //     if (this.name == "PlayerBullet(Clone)" || this.name == "PlayerPunch(Clone)")
    //     {
    //         slowDown.SlowTime();
    //     }
    // }
}
