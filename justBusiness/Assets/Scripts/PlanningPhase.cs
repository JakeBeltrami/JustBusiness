using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningPhase : MonoBehaviour
{
    //Canvas for the planning phase text
    public GameObject planningText;
    public AudioClip executionPhase;
    private AudioSource audioSource;
    private PostProcessing post;
    private TurnManager turnManager;
    private GameObject player;
    private GameManager gameManager;
    private POI poi;
    private List<GameObject> enemies;
    public LayerMask toggleMask;

    public bool tut = false; // Not sure how to handle the tutorial yet

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        enemies = gameManager.Enemies;
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        post = GetComponent<PostProcessing>();
        turnManager = GetComponent<TurnManager>();
        poi = GetComponent<POI>();
    }

    private void Start()
    {
        enemies = gameManager.Enemies;
    }

    void Update()
    {
        // If toggle button pressed and we're planning
        if (Input.GetKeyDown(KeyCode.Mouse0) && gameManager.Planning)
        {
            // // Create a ray at input position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Check intersection of ray using camera depth (z) + buffer and toggle LayerMask
            // NOTE: Is array due to overlap bug
            RaycastHit2D[] hit2D = Physics2D.GetRayIntersectionAll(ray, 12f, toggleMask);
            if (hit2D != null)
            {
                foreach (RaycastHit2D c in hit2D)
                {
                    // Check if it has a Toggleable component
                    if (c.transform.GetComponent<Toggleable>() != null)
                    {
                        // Toggle the object
                        c.transform.GetComponent<Toggleable>().Toggle();
                    }
                }
            }
        }

        if (gameManager.Controller && gameManager.Planning)
        {
            ControllerPlanning();
        }
    }

    void ControllerPlanning()
    {
        //gameManager.lineRenderer.SetPosition(0, player.transform.position);
        //gameManager.lineRenderer.SetPosition(1, gameManager.LineEnd.position);
        if (Input.GetButtonDown("A") && gameManager.Planning)
        {
            RaycastHit2D hit = Physics2D.Linecast(player.transform.position, gameManager.LineEnd.position, toggleMask);
            if (hit)
            {
                // Check if it has a Toggleable component
                if (hit.transform.gameObject.GetComponent<Toggleable>() != null)
                {
                    // Toggle the object
                    hit.transform.gameObject.GetComponent<Toggleable>().Toggle();
                }
            }
        }
    }

    public void BeginPlanning()
    {
        gameManager.Planning = true;
        // Don't show POI in planning phase
        poi.DisableAllPOI();
        player.GetComponent<Player>().enabled = false;

        turnManager.enabled = false;

        if (enemies != null)
        {
            foreach (GameObject e in enemies)
            {
                // enemies[i].GetComponentInChildren<FieldOfView>().enabled = false;
                // enemies[i].GetComponent<Enemy>().enabled = false;
                // Enable outline if toggleable
                if (e.GetComponent<Toggleable>() != null)
                {
                    e.GetComponent<Animator>().SetBool("outlined", true);
                    // Debug.Log(e.transform.GetChild(e.transform.childCount - 1).name);
                    // Disable outline on alt
                    e.transform.GetChild(e.transform.childCount - 1).GetComponent<Animator>().SetBool("outlined", false);
                }
                else
                {
                    // Ensure they aren't outlined
                    e.GetComponent<Animator>().SetBool("greyscale", true);
                    e.GetComponent<Animator>().SetBool("outlined", false);
                    // Destroy alt enemy
                    Destroy(e.transform.GetChild(e.transform.childCount - 1).gameObject);
                }
            }
        }
        // Player walks in
        StartCoroutine(player.GetComponent<PlayerInput>().WalkIn());
    }

    public void EndPlanning()
    {
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<Player>().enabled = true;


        // Disable the post processing effect
        post.DisablePost();

        // Play Execution Phase music
        audioSource.clip = executionPhase;
        audioSource.Play();

        planningText.SetActive(false);

        if (enemies != null)
        {
            foreach (GameObject e in enemies)
            {
                // enemies[i].GetComponentInChildren<FieldOfView>().enabled = true;
                // enemies[i].GetComponent<Enemy>().enabled = true;
                // Disable outline if toggleable
                if (e.GetComponent<Toggleable>() != null)
                {
                    e.GetComponent<Animator>().SetBool("outlined", false);
                    e.GetComponent<Animator>().SetBool("greyscale", false);
                    // Destroy alt enemy
                    Destroy(e.transform.GetChild(e.transform.childCount - 1).gameObject);
                }
                else
                {
                    // Disable greyscale
                    e.GetComponent<Animator>().SetBool("greyscale", false);
                }
                e.GetComponent<Enemy>().SetInitialState();
            }
        }

        // Get all Toggleable scripts and remove them
        Toggleable[] toggles = FindObjectsOfType<Toggleable>();
        foreach (Toggleable t in toggles)
        {
            t.Remove();
        }

        // Update Grid
        gameManager.Grid.UpdateGrid();
        gameManager.Planning = false;
        turnManager.enabled = true;
        enabled = false; // Disable this script
    }

}
