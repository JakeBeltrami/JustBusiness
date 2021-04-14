using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code adapted from Sebastian Lague
public class FieldOfView : MonoBehaviour
{
    private GameManager gameManager;
    private Enemy self;
    private LayerMask targetMask;
    private LayerMask obstacleMask; // Enable if cover exists
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    private bool playerSpotted;
    // public Vector3 direction; // For testing purposes
    private RaycastHit2D environmentDetector;
    private bool lowEnvironment;
    public bool PlayerSpotted { get { return playerSpotted; } set { playerSpotted = value; } }

    private void Awake()
    {
        // Sets the rotation to align with initial direction
        self = GetComponentInParent<Enemy>();
        FaceDirection();
        playerSpotted = false;
    }

    void Start()
    {
        // Sets the rotation to align with initial direction
        self = GetComponentInParent<Enemy>();
        FaceDirection();

        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        obstacleMask = LayerMask.GetMask("Environment");
        targetMask = LayerMask.GetMask("Player");
    }

    // NOTE: Optional to search FOV with delay
    // private IEnumerator FindTargetsWithDelay(float delay)
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(delay);
    //         FindVisibleTargets();
    //     }
    // }

    private void Update()
    {
        if (!playerSpotted && !gameManager.Planning)
        {
            FindVisibleTargets();
        }
        //Debug.DrawLine(transform.position, player.transform.position);
        if (playerSpotted && !gameManager.Paused)
        {
            // Set State
            self.Agro(player.GetComponent<IHasTile>().Tile.Center);
            TrackPlayer();
        }
    }

    // NOTE: Will have to use Grid based search instead?
    /// <summary>
    /// Determine if the player is within the field of view
    /// </summary>
    public void FindVisibleTargets()
    {
        // Check if player is in range
        Vector2 dirToTarget = (player.transform.position - transform.position).normalized;
        bool playerInRange = Physics2D.Raycast(transform.position, dirToTarget, viewRadius, targetMask);

        if (playerInRange)
        {
            // If they are within our fov
            if (Vector3.Angle(self.Direction, dirToTarget) < viewAngle / 2)
            {
                // NOTE: Might be best to change Environment Low to its own layer instead of using tags
                float dstToTarget = Vector3.Distance(transform.position, player.transform.position);
                // Debug.Log("I am: " + transform.position);
                // Debug.Log("Player is: " + player.transform.position);
                // Debug.Log("Distance: " + dstToTarget);
                RaycastHit2D[] obstacles = Physics2D.RaycastAll(transform.position, dirToTarget, dstToTarget, obstacleMask);
                if (obstacles.Length == 0)
                {
                    playerSpotted = true;
                    return;
                }
            }
        }

        // Else we don't see the player
        // playerSpotted = false;
    }

    public void FaceDirection()
    {
        transform.eulerAngles = Vector3.forward * AngleFromDir(self.Direction);
    }

    /// <summary>
    /// Aligns with the player and sets the enemy direction accordingly
    /// </summary>
    private void TrackPlayer()
    {
        // Rotate to align Y axis with player
        Vector3 dirToTarget = (player.transform.position - transform.position).normalized;
        transform.up = dirToTarget;
        // Set our direction according to our angle
        self.SetDirection(self.transform.position + DirFromAngle(transform.rotation.z, false));
    }

    /// <summary>
    /// Returns a direction given an angle
    /// </summary>
    /// <returns></returns>
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    /// <summary>
    /// Returns an angle given a direction. Used to rotate enemy FOV.
    /// </summary>
    /// <returns></returns>
    public float AngleFromDir(Vector3 direction)
    {
        // Can add in +360f angle if required (instead of +/-90)
        return Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
    }
}
