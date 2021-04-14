using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		Handles enemy AI line-of-sight

	Parameters:
		None

	Dependencies:
        Enemy.cs
        Player.cs

	Author:
		Phil

	Changelog:
		16-05-2019	Initial version
        18-05-2019  Added tooltips

	Notes:


======================================================================
*/

//public class Eyes : MonoBehaviour
//{
//    [Tooltip("Adjusts the width of the enemy's line-of-sight cone.\n\nHigher values = Wider sight radius\nLower values = reduced/narrow vision")]
//    public float fovAngle = 10; // Controls the width of the field of view.
//    [Tooltip("Adjusts the length of the enemy's line-of-sight cone.\n\nHigher values = Longer sight range\nLower values = shorter sight range")]
//    public int eyesightRange = 8; // Controls how far ahead the enemy can see.

//    // Internal variables
//    private LineRenderer line; //line used to visualise FOV
//    private bool playerSeen;
//    private GameObject player;
//    private GameObject enemy;
//    private int layerMask; // Filter for raycast so that it will only collide with the Player's layer.
//    private Transform parent; // Transform of parent object.
//    private float distanceToPlayer;

//    public GameObject alertBox;

//    // Property for aiming to access
//    public bool PlayerSeen { get { return playerSeen; } }

//    void Start()
//    {
//        playerSeen = gameObject.GetComponentInParent<Enemy>().TargetSpotted;
//        player = GameObject.Find("Player");
//        enemy = transform.parent.gameObject;
//        layerMask = LayerMask.GetMask("Player");
//        parent = transform.parent.transform;

//        line = GetComponent<LineRenderer>();
//        line.enabled = false;
//        line.useWorldSpace = true;
//        line.SetWidth(0.05f, 0.05f);

//        alertBox = GameObject.FindGameObjectWithTag("alertBox");
//        alertBox.SetActive(false);
//    }

//    void Update()
//    {
//        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<FrozenTime>().enemyActive)
//        {
//            Looking();
//            gameObject.GetComponentInParent<Enemy>().TargetSpotted = playerSeen;
//        }
//    }


//    void Looking()
//    {
//        if (playerSeen)
//        {
//            enemy.GetComponent<Enemy>().xSpd = player.transform.position.x - enemy.transform.position.x;
//            enemy.GetComponent<Enemy>().ySpd = player.transform.position.y - enemy.transform.position.y;
//        }

//        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
//        Vector3 targetDir = player.transform.position - parent.position;
//        Vector3 right = transform.right;
//        Vector3 ahead = transform.position;
//        ahead.x = ahead.x + 5;

//        Vector3 dir = ahead - right;
//        dir = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z) * dir;



//        float angle = Vector3.Angle(targetDir, transform.right);
//        //Debug.Log("LOOKING in " + angle);
//        if (angle < fovAngle && distanceToPlayer <= eyesightRange)
//        {
//            //Debug.Log("SEEN at " + distanceToPlayer);
//            playerSeen = true;
//            // Build bug here
//            if (alertBox != null)
//            {
//                alertBox.SetActive(true);
//            }

//            //Debug.Log("SEEN");
//        }
//        // else
//        // {
//        //     playerSeen = false;
//        // }


//        line.SetPosition(0, parent.position);



//        //RaycastHit2D eyeline = Physics2D.Raycast(parent.position, transform.right, eyesightRange, layerMask);
//        //RaycastHit2D eyes = Physics2D.Raycast()
//        Vector3 eyelineEndpoint = parent.position + (transform.right * eyesightRange);
//        //Vector3 positiveEndpoint = dir;
//        //Vector3 negativeEndpoint = dir;

//        line.SetPosition(1, eyelineEndpoint);
//        //line.SetPosition(2, positiveEndpoint);
//        /*if (eyeline.collider != null)
//        {

//            Debug.Log("PEW PEW");
//            Debug.Log(eyeline.collider.name);

//            //Debug.DrawLine(transform.position, eyes.collider.transform.position);
//            //Debug.DrawRay(transform.position, transform.right, Color.green);
//        }*/
//    }
//    //7:30 - 10:46 3:31
//    //11:10 - 12:30 4:51
//    //1:50 6:11
//    //4:30 8:41

//}
