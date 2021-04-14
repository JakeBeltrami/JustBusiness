using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    private GameObject player;
    private GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        //Kills all enemies in scene
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.K) && !(Input.GetKey(KeyCode.LeftShift)))
        {

            Debug.Log("Kill all");

            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().Die();
            }
        }

        //Punches all enemies once
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.H) && !(Input.GetKey(KeyCode.LeftShift)))
        {

            Debug.Log("Punch all");

            foreach (GameObject enemy in enemies)
            {//warning: will trigger disabled scripts. remove scripts to make sure they don't intefere with desired result.
                enemy.GetComponent<Enemy>().TakeDamage(1, enemy.transform.position, "melee");
            }
        }

        //Kills player
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.K))
        {

            Debug.Log("Kill self");
            player.GetComponent<Player>().Die();

        }

        //Moves player to invincibility layer. Can toggle.
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.I))
        {
            if (player.layer == 8)
            {
                player.layer = 14;
                Debug.Log("Invincibility on");
            }

            else
            {
                player.layer = 8;
                Debug.Log("Invincibility off");
            }
        }

        //Unlimited ammo. Can toggle.
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
        {
            if (player.GetComponent<GunAttack>().unlimitedAmmo == false)
            {
                player.GetComponent<GunAttack>().unlimitedAmmo = true;
                Debug.Log("Unlimited ammo on");
            }

            else
            {
                player.GetComponent<GunAttack>().unlimitedAmmo = false;
                Debug.Log("Unlimited ammo off");
            }


        }

        //Get Spotted.
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponentInChildren<FieldOfView>().PlayerSpotted = true;
            }
            Debug.Log("Spotted.");

        }

        //Toggle freeze.
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
        {

            Debug.Log("Freeze toggle.");

        }

        //Dump selected enemy info
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D) && !(Input.GetKey(KeyCode.LeftShift)))
        {
            foreach (GameObject enemy in enemies)
            {
                //   if (enemy.GetComponent<POI>(). == true)
                // {

                Debug.Log("<color=red>--------------------ENEMY DUMP-----------------------</color>");
                Debug.Log("NAME: " + enemy.name);
                Debug.Log("LAYER: " + enemy.layer);
                Debug.Log("MAX HEALTH: " + enemy.GetComponent<Enemy>().maxHP);
                Debug.Log("STATE: " + enemy.GetComponent<Enemy>().MyState);
                Debug.Log("FOV ANGLE: " + enemy.GetComponent<Enemy>().FOV.viewAngle);
                Debug.Log("FOV RADIUS: " + enemy.GetComponent<Enemy>().FOV.viewRadius);
                Debug.Log("FOV PLAYER SPOTTED?: " + enemy.GetComponent<Enemy>().FOV.PlayerSpotted);
                Debug.Log("PRIMARY PATROL POINTS: ");
                Debug.Log(enemy.GetComponent<Patrol>().patrolPoints);
                Debug.Log("SECONDARY PATROL POINTS: ");
                Debug.Log(enemy.GetComponent<Patrol>().altPatrolPoints);
                //Debug.Log("INVENTORY (WEAPON): " + enemy.GetComponent<NewInventory>().Weapon.name);
                //Debug.Log("INVENTORY (WEAPON RANGE): " + enemy.GetComponent<NewInventory>().Weapon.Range);

                if (enemy.GetComponent<GunAttack>())
                {
                    Debug.Log("AMMO COUNT: " + enemy.GetComponent<GunAttack>().Ammo);
                    Debug.Log("UNLIMITED AMMO?" + enemy.GetComponent<GunAttack>().unlimitedAmmo);
                }
                //}
            }
        }

        //Dump player info
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("PLAYER DUMP");
            Debug.Log("LAYER: " + player.layer);
            Debug.Log("INPUT OPTION: " + player.GetComponent<Player>().inputOption);
            //Debug.Log("INPUT LOCK?: " + player.GetComponent<Player>().InputLock);
            Debug.Log("PLAYER DEAD?: " + player.GetComponent<Player>().playerDead);
            //Debug.Log("WEAPON: " + player.GetComponent<PlayerInventory>().Weapon.name);

            if (player.GetComponent<GunAttack>())
            {
                Debug.Log("AMMO COUNT: " + player.GetComponent<GunAttack>().Ammo);
                Debug.Log("UNLIMITED AMMO?: " + player.GetComponent<GunAttack>().unlimitedAmmo);
            }



        }


    }
}
