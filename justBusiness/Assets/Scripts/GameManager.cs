using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool planning;
    private PauseScreen pauseScreen;
    private PlanningPhase pp;
    private int currentLevelIndex;
    private Player player;
    private List<GameObject> enemies;
    //private Grid grid;
    private PathFind.Grid grid;
    private PostProcessing post;
    private TurnManager turnManager;
    private bool levelWon;
    private bool paused;
    private Canvas clapper;
    private GameOverTips gameOverTips;
    public GameObject redImage; // Will need to be manually linked in Inspector per level
    public bool PlanningPhaseOn;
    public bool Controller = false;
    [HideInInspector]
    public Transform LineEnd;
    [HideInInspector]
    public LineRenderer lineRenderer;

    // Added by W.T. (04-09-2019)
    // For detecting double clicks to end PP
    private float dblClickTimer = 0f;
    private bool hasDoubleClicked = false;
    private LayerMask layerMask;
    [HideInInspector]
    public GameObject targetedObject;
    public SetTile endTile;

    // Read only level state property
    public bool LevelWon { get { return levelWon; } }
    public bool Planning { get { return planning; } set { planning = value; } }
    public List<GameObject> Enemies { get { return enemies; } }
    // public Grid Grid { get { return grid; } }
    public PathFind.Grid Grid { get { return grid; } }
    public bool Paused { get { return paused; } set { paused = value; } }
    public Player Player { get { return player; } }
    public PostProcessing Post { get { return post; } }
    public TurnManager TurnManager { get { return turnManager; } }
    public Canvas Clapper { get { return clapper; } }

    public void Awake()
    {
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // grid = GetComponent<Grid>();
        grid = GetComponent<PathFind.Grid>();
        layerMask = LayerMask.GetMask("POI");
        post = GetComponent<PostProcessing>();
        pauseScreen = GetComponent<PauseScreen>();
        turnManager = GetComponent<TurnManager>();
    }

    public void Start()
    {
        // Not sure if necessary, sets the current level index
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        paused = false;
        planning = false;
        levelWon = false;
        clapper = GameObject.FindGameObjectWithTag("Clapper").GetComponentInChildren<Canvas>();
        clapper.enabled = false;
        gameOverTips = GameObject.FindGameObjectWithTag("Game Over").GetComponent<GameOverTips>();
        gameOverTips.Init();
        gameOverTips.enabled = false;
        pp = GetComponent<PlanningPhase>();

        // Start the Planning Phase
        if (PlanningPhaseOn)
        {
            pp.BeginPlanning(); // Uncomment to enable planning
        }
        else
        {
            pp.EndPlanning();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // PAX Shortcuts
        // Go to Menu
        if (Input.GetKey(KeyCode.M) && Input.GetKey(KeyCode.LeftControl))
        {
            StartCoroutine(ChangeScene("MenuEdit"));
        }
        // Edited by W.T. (03-09-2019)
        // Removed spacebar condition for cancelling PP
        // Exiting is now performed by double clicking anywhere on the screen after choosing a POI
        if (Input.GetKeyDown(KeyCode.Mouse0) && !paused && planning)
        {
            // Check that the player is actually double clicking on a valid target
            GetTarget();
            if (targetedObject != null)
            {
                // Edited by W.T. (16-09-2019)
                // Raised timer threshold from 0.15 to 0.3
                if (Time.time < dblClickTimer + 0.3f)
                {
                    hasDoubleClicked = true;
                }
                dblClickTimer = Time.time;
                if (hasDoubleClicked)
                {
                    // Exit the PP now to perform action
                    // pp.EndPlanning();
                }
                //Debug.Log("Clicking on a POI but not 2x clicking");
            }
        }
        // 05/10/19 Commented this out in replacement of Clapper Starting
        // Need to change so that when clapper closed is clicked end planning.
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        // pp.EndPlanning();
        //}

        // Edited by W.T. (03-09-2019)
        // Ditto for controllers
        if (Input.GetButton("B") && !paused && planning)
        {
            //Controller = true;
            GetTarget();
            if (targetedObject != null)
            {
                /*if (Time.time < dblClickTimer + 0.25f)
                {
                    hasDoubleClicked = true;
                }
                dblClickTimer = Time.time;
                if (hasDoubleClicked)
                {
                    pp.EndPlanning();
                }
            }*/
                pp.EndPlanning();
            }
        }
        // Commented out for testing purposes
        if (enemies.Count == 0)
        {
            turnManager.enabled = false;
            if (endTile != null && !levelWon)
            {
                if (player.Tile != endTile.Tile)
                {
                    // Pathfind to door
                    StartCoroutine(player.PlayerInput.MoveToDoor(endTile.Tile));
                }

            }
            // Ugly code for no door
            else if (!levelWon)
            {
                StartCoroutine(player.PlayerInput.MoveToEnd(GameObject.FindGameObjectWithTag("End").GetComponent<IHasTile>().Tile));
            }
            levelWon = true;
        }

        //Could also put in gameover script
        if (player.GetComponent<Player>().playerDead)
        {
            turnManager.enabled = false;
            Invoke("EnableGameOver", 1f);
            //load gameover scene
        }

        if (Controller)
        {
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, LineEnd.position);
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.R))
        {
            pauseScreen.Restart();
        }
    }

    // Do these need to wait?
    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(0.75f);

        // Get the index of the current scene
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        // Load next level
        SceneManager.LoadScene(currentLevelIndex + 1);
        // Reset state (Might not be needed if it is being remade on scene change)
        // levelWon = false;
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    public IEnumerator ChangeScene(string sceneName)
    {
        yield return new WaitForSeconds(0.75f);

        Scene nextScene = SceneManager.GetSceneByName(sceneName);

        if (nextScene != null)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.7f);

        // Application.LoadLevel(Application.loadedLevel);
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);

        // Load the Game Over screen, can reload level using the index set here
    }

    public void EnableGameOver()
    {
        gameOverTips.enabled = true;
    }

    // Moved from player to here, shows red on camera
    public void DisplayRed()
    {
        if (redImage != null)
        {
            redImage.SetActive(true);
        }
    }

    public void HideRed()
    {
        if (redImage != null)
        {
            redImage.SetActive(false);
        }
    }

    // Created by W.T. (07-09-2019)
    // Used to grab the first POI selected during the PP
    // Takes: nothing
    // Returns: nothing
    public void GetTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

        if (hit)
        {
            targetedObject = hit.transform.gameObject;
            //Debug.Log("Clicked on: " + targetedObject.name);
        }
    }
}
