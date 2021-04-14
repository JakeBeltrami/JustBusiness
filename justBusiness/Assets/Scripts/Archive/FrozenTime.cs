using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class FrozenTime : MonoBehaviour
//{

//    public GameObject energyBar;
//    //Canvas for the planning phase text
//    public GameObject planningText;

//    // Start is called before the first frame update
//    public AudioClip executionPhase;
//    AudioSource audioSource;
//    // Time is slowed when action phase begins
//    private SlowDown slowDown;
//    private PostProcessing post;

//    private GameObject player;
//    private GameObject arm;

//    private GameObject[] enemies;

//    private GameObject[] environments;

//    private GameObject[] poiTargets;

//    public bool paused = false;

//    public bool tut = false;

//    public bool enemyActive = false;

//    private bool planningPhase;

//    void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player");
//        player.GetComponent<PlayerInput>().enabled = false;
//        player.GetComponent<Player>().enabled = false;

//        audioSource = GetComponent<AudioSource>();
//        energyBar = GameObject.Find("EnergyBar");
//        energyBar.SetActive(false);
//        Time.timeScale = 0; // Freezes time in game
//        player.GetComponent<Player>().InputLock = true;

//        // Change in camera movement from true to false LS 20/05/19 (player can no longer move camera)
//        CameraMovement.cameraMove = true;
//        slowDown = this.GetComponent<SlowDown>();
//        post = this.GetComponent<PostProcessing>();
//        // post.EnablePost();

//        arm = GameObject.Find("Player/arm");
//        // arm.GetComponent<Aiming>().lockmove = true;

//        enemies = GameObject.FindGameObjectsWithTag("Enemy");
//        for (int i = 0; i < enemies.Length; i++)
//        {
//            enemies[i].GetComponent<Enemy>().enabled = false;
//            //enemies[i].GetComponent<Patrol>().enabled = false;
//        }

//        // Not sure what this is wanting to do. Get all enivronment pieces and disable their collider?
//        // Regardless, the Environment isn't tagged so this isn't working
//        environments = GameObject.FindGameObjectsWithTag("Environment");
//        for (int i = 0; i < environments.Length; i++)
//        {
//            // Build bug here
//            environments[i].GetComponent<Collider2D>().enabled = false;
//            //enemies[i].GetComponent<Patrol>().enabled = false;
//        }

//        //spacePressed = false;

//        poiTargets = GameObject.FindGameObjectsWithTag("POI Target");
//        foreach (GameObject poi in poiTargets)
//        {
//            poi.GetComponent<Collider2D>().enabled = false;
//        }

//        planningPhase = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Planning;
//        planningPhase = true;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // Time is frozen until Space is pressed
//        if (Input.GetKeyDown(KeyCode.Space) && !paused && !tut)
//        {
//            // Set planning phase as false
//            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Planning = false;

//            //spacePressed = true;
//            audioSource.clip = executionPhase;
//            audioSource.Play();

//            CameraMovement.cameraMove = false;
//            energyBar.SetActive(true);
//            player.GetComponent<Player>().InputLock = false;
//            planningText.SetActive(false);

//            // Disable the post processing effect
//            post.DisablePost();

//            slowDown.SlowTime();

//            // arm.GetComponent<Aiming>().lockmove = false;

//            player.GetComponent<PlayerInput>().enabled = true;
//            player.GetComponent<Player>().enabled = true;

//            for (int i = 0; i < enemies.Length; i++)
//            {
//                enemies[i].GetComponent<Enemy>().enabled = true;
//                // Disable their outline
//                if (enemies[i].GetComponent<BooleanObject>().enabled)
//                {
//                    enemies[i].GetComponent<BooleanObject>().DisableOutline();
//                }
//                //enemies[i].GetComponent<Patrol>().enabled = true;
//            }

//            for (int i = 0; i < environments.Length; i++)
//            {
//                // Build bug here
//                environments[i].GetComponent<Collider2D>().enabled = true;
//                //enemies[i].GetComponent<Patrol>().enabled = false;
//            }



//            foreach (GameObject poi in poiTargets)
//            {
//                if (poi.transform.parent.name == "TogglePistol")
//                {
//                    poi.transform.parent.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Weapons/Pistol");
//                    poi.transform.parent.GetChild(0).gameObject.SetActive(false);
//                }

//                // Edited by Trent
//                // Changed the POI system so I commented this stuff out

//                //if (poi.transform.parent.name == "Pistol_Grunt")
//                //{
//                //    poi.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
//                //}

//                //GameObject[] targets = new GameObject[poi.transform.childCount];
//                //int k = 0;
//                //foreach (Transform child in poi.transform)
//                //{
//                //    targets[k] = child.gameObject;
//                //    k++;
//                //}

//                //foreach (GameObject tar in targets)
//                //{
//                //    if (tar.tag == "Target")
//                //    {
//                //        tar.GetComponent<SpriteRenderer>().color = new Color(tar.GetComponent<SpriteRenderer>().color.r,
//                //                                                              tar.GetComponent<SpriteRenderer>().color.g,
//                //                                                              tar.GetComponent<SpriteRenderer>().color.b,
//                //                                                              0.2f);
//                //    }
//                //}

//                poi.GetComponent<Collider2D>().enabled = true;
//            }

//            enemyActive = true;

//            slowDown.SlowTime();

//            enabled = false; // Disable this script
//        }


//    }

//}
