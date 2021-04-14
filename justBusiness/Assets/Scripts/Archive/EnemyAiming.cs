using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================
	Purpose:
		Displays enemy aiming
	Parameters:
		None
	Dependencies:
        Eyes
	Author:
		Trent Williams
	Changelog:
	Notes:
        
======================================================================
*/

//public class EnemyAiming : MonoBehaviour
//{
//    public Vector2 frontArmPosition;
//    public Vector2 backArmPosition;
//    public Vector2 leftArmPosition;
//    public Vector2 rightArmPosition;

//    private SpriteRenderer enemySprite;
//    private string direction;
//    private int enemySortingOrder;
//    private SpriteRenderer armSpriteRenderer;
//    // private Player enemyScript;

//    private GameObject enemy;
//    private GameObject player;
//    private bool aiming;

//    public Eyes eyes;

//    void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player");
//        enemy = gameObject.transform.parent.gameObject;
//        armSpriteRenderer = GetComponent<SpriteRenderer>();
//        enemySprite = enemy.GetComponent<SpriteRenderer>();
//        // playerScript = player.GetComponent<Player>();
//        enemySortingOrder = enemySprite.sortingOrder;
//    }

//    void Update()
//    {
//        // aiming = eyes.PlayerSeen;
//        aiming = eyes.PlayerSeen;
//        if (aiming)
//        {
//            // Added by K.C. (21-05-2019)
//            //Checks if weapon equipped
//            armSpriteRenderer.enabled = true;
//            string spriteName = enemySprite.sprite.name;
//            direction = GetDirection();
//            // direction = spriteName.Substring(spriteName.Length - 1, 1);
//            spriteName = "Enemy/Aiming/Regular_Grunt_Aiming_" + direction;
//            enemySprite.sprite = Resources.Load<Sprite>(spriteName);
//            armSpriteRenderer.sprite = Resources.Load<Sprite>(spriteName + "_Pistol");

//            // Change arm position
//            switch (direction)
//            {
//                case "B":
//                    transform.localPosition = backArmPosition;
//                    armSpriteRenderer.sortingOrder = enemySortingOrder - 1;
//                    break;
//                case "F":
//                    transform.localPosition = frontArmPosition;
//                    armSpriteRenderer.sortingOrder = enemySortingOrder + 1;
//                    break;
//                case "L":
//                    transform.localPosition = leftArmPosition;
//                    armSpriteRenderer.sortingOrder = enemySortingOrder - 1;
//                    break;
//                case "R":
//                    transform.localPosition = rightArmPosition;
//                    armSpriteRenderer.sortingOrder = enemySortingOrder + 1;
//                    break;
//            }

//            FacePlayer();
//        }
//        else
//        {
//            armSpriteRenderer.enabled = false;
//        }

//        // FaceMouse();
//    }

//    private void FacePlayer()
//    {
//        Vector2 targetPosition = new Vector2(player.transform.position.x, player.transform.position.y);
//        Vector2 shootPosition = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
//        Vector2 direction = targetPosition - shootPosition;
//        transform.up = direction;
//    }


//    private void FaceMouse()
//    {
//        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
//        transform.up = direction;
//    }

//    private string GetDirection()
//    {
//        //Vector2 targetPosition = new Vector2(player.transform.position.x, player.transform.position.y);
//        //Vector2 shootPosition = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
//        //Vector2 direction = targetPosition - shootPosition;

//        Vector3 playerPosition = player.transform.position;
//        Vector2 direction = new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y).normalized;

//        float x = direction.x;
//        float y = direction.y;
//        string facing = "L"; // Set default direction

//        // if x is positive
//        if (x > 0)
//        {
//            // if top right
//            if (y > 0)
//            {
//                if (x >= y)
//                {
//                    facing = "R";
//                }
//                else
//                {
//                    facing = "B";
//                }
//            }
//            // if bottom right
//            else
//            {
//                if (x >= -y)
//                {
//                    facing = "R";
//                }
//                else
//                {
//                    facing = "F";
//                }
//            }
//        }
//        // if x is negative
//        if (x < 0)
//        {
//            // if top left
//            if (y > 0)
//            {
//                if (-x >= y)
//                {
//                    facing = "L";
//                }
//                else
//                {
//                    facing = "B";
//                }
//            }
//            // if bottom left
//            else
//            {
//                if (x <= y)
//                {
//                    facing = "L";
//                }
//                else
//                {
//                    facing = "F";
//                }
//            }
//        }

//        return facing;
//    }
//}