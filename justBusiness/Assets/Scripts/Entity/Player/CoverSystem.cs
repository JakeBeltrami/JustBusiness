using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		Enables the player to take cover if they are at a CoverNode

	Parameters:
		None

	Dependencies:
        Player.cs
        PlayerInput.cs

	Author:
		William Tat

	Changelog:
		09-09-2019	Initial creation
        16-09-2019  LeaveCover functionality added
        17-09-2019  TakeCover no longer returns a List
        24-09-2019  POI not spawning correctly fixed, added animations
        25-09-2019  Refactoring to support node-based cover instead
        26-09-2019  Continued refactoring for fixed cover points
        27-09-2019  Triggering cover animations shifted to PlayerInput
        29-09-2019  Switching in-and-out of cover is now handled by LeaveCover and EnterCover
        01-10-2019  Fixed issue with TakeCover not determining angle between objects correctly
        03-10-2019  Changed nodeMask from Node to dedicated Cover layer instead
        06-10-2019  Reduced from 8-point to 4-point directional cover

======================================================================
*/

public class CoverSystem : MonoBehaviour
{
    // All objects that can provide cover are stored here
    // 01-10-2019: Reverted to list
    private List<GameObject> coveringObjects = new List<GameObject>();

    // Flags
    private bool isAtSameCoverLocation = false; // For EnterCover()

    public bool PlayerIsAtSameCoverLocation { get => isAtSameCoverLocation; set => isAtSameCoverLocation = value; }

    // Internal variables for grabbing targets
    private Player player;
    private LayerMask nodeMask;
    private LayerMask obstacleMask;

    public void Start()
    {
        player = GetComponent<Player>();
        nodeMask = LayerMask.GetMask("Cover");  // Originally "Node"
        obstacleMask = LayerMask.GetMask("Environment");
        //coveringNodes = GameObject.FindGameObjectsWithTag("CoverNode");
    }

    // SeekCover
    // Search for CoverNodes at the POI marker and raycast for directional cover
    // Takes: GameObject
    // Returns: nothing
    public void SeekCover(Vector2 target)
    {
        // This is for storing the CoverNodes
        Collider2D[] nodes = Physics2D.OverlapCircleAll(target, 1f, nodeMask);

        if (nodes.Length != 0)
        {
            // // Find if said node is actually a CoverNode
            // for (int i = 0; i < nodes.Length; i++)
            // {
            //     if (nodes[i].GetComponent<Collider2D>().enabled)
            //     {
            //         // If yes, then take cover
            //         if (nodes[i].CompareTag("CoverNode"))
            //         {
            //             //Debug.Log("=================================== <color=yellow>Found CoverNode at: " + nodes[i].transform.position + "</color> ===================================");
            //             TakeCover(nodes[i].gameObject);
            //             break;
            //         }
            //     }
            // }
            TakeCover();
        }
        else
        {
            player.IsInCover = false;
            //Debug.Log("=================================== <color=red>No cover available.</color> ===================================");
        }

        /*
         * =================================================================================================
         * START OLD CODE FOR DYNAMIC COVER SEEKING
         * =================================================================================================
        // Check radius around player to find any barrier-type objects
        // TODO: Better method for object validation?
        Collider2D[] doodadsFound = Physics2D.OverlapCircleAll(target.transform.position, 0.77f, obstacleMask);

        if (doodadsFound != null)
        {
            for (int i = 0; i < doodadsFound.Length; i++)
            {
                // Double check if we actually have objects we can hide behind
                if (doodadsFound[i].GetComponent<Collider2D>().enabled)
                {
                    coveringObjects.Add(doodadsFound[i].gameObject);
                    Debug.Log("Found: " + doodadsFound[i].gameObject.name);
                    canTakeCover = true;
                }
            }
            if (canTakeCover)
            {
                // Now change them
                TakeCover();
                // Create POI at feet so that player can leave cover (optional)
                canTakeCover = false;
                //return true;
            }
            else
            {
                Debug.Log("No cover found within proximity of POI");
                canTakeCover = false;
                //return false;
            }
        }
        else if (doodadsFound == null)
        {
            Debug.Log("No cover found within proximity of POI");
            canTakeCover = false;
            //return false;
        }
        //return false;
         * =================================================================================================
         * END OLD CODE FOR DYNAMIC COVER SEEKING
         * =================================================================================================
        */
    }

    // TakeCover
    // Player is now "hidden" at CoverNode
    // Takes: GameObject
    // Returns: nothing
    public void TakeCover(GameObject node)
    {
        // For storing what objects are near the CoverNode
        Collider2D[] coverObjs = Physics2D.OverlapCircleAll(node.transform.position, 1f, obstacleMask);

        //Debug.Log("=================================== <color=green>Taking cover.</color> ===================================");
        player.IsInCover = true;
        isAtSameCoverLocation = true;

        // We now need to find what directions they're shielded in
        // Enemy bullets cannot damage the player if they come from these angles
        for (int i = 0; i < coverObjs.Length; i++)
        {
            // Find out what direction this object is in and add to the Player's anglesCantBeHitFrom list
            player.BulletsCantHitFromTheseDirections.Add(FindDirection(coverObjs[i].gameObject));
            //Debug.Log("Found: " + coverObjs[i].name);
        }
        foreach (string s in player.BulletsCantHitFromTheseDirections)
        {
            Debug.Log(s);
        }

        /*
         * =================================================================================================
         * START OLD CODE FOR DYNAMIC COVER SEEKING
         * =================================================================================================
        // Change them to walls
        // TODO: Change to something else?
        foreach (GameObject o in coveringObjects)
        {
            o.gameObject.tag = "Wall";
        }
        // Now hidden behind cover
        player.IsInCover = true;
        Debug.Log("Player in cover.");
        * =================================================================================================
        * END OLD CODE FOR DYNAMIC COVER SEEKING
        * =================================================================================================
        */
    }

    public void TakeCover()
    {
        player.IsInCover = true;
        isAtSameCoverLocation = true;

        // for (int x = -1; x <= 1; x++)
        // {
        //     for (int y = -1; y <= 1; y++)
        //     {
        //         Vector2 direction = new Vector2(x, y);
        //         RaycastHit2D envHit = Physics2D.Raycast(player.Tile.Center, direction, 1f, obstacleMask);
        //         // If we hit a Node
        //         if (envHit)
        //         {
        //             // coverDirections.Add(direction);
        //         }
        //     }
        // }
    }

    //public bool CheckCover(Vector2 dir)
    //{
    //    Vector2 direction = -dir;

    //}

    // EnterCover
    // Player goes back into cover at the same node
    // Takes: nothing
    // Returns: nothing
    public void EnterCover()
    {
        //Debug.Log("=================================== <color=yellow>Entering cover.</color> ===================================");
        player.IsInCover = true;
        isAtSameCoverLocation = true;
        player.Animator.SetBool("cover", true);
    }

    // LeaveCover
    // Player is no longer hidden behind CoverNode and can shoot (but will take hits)
    // Takes: nothing
    // Returns: nothing
    public void LeaveCover()
    {
        //Debug.Log("=================================== <color=purple>Getting out from cover.</color> ===================================");
        player.IsInCover = false;
        isAtSameCoverLocation = true;
        player.Animator.SetBool("cover", false);

        /* 
         * =================================================================================================
         * START OLD CODE FOR DYNAMIC COVER SEEKING
         * =================================================================================================
        // Revert cover to normal objects (SHOULD BE CHANGED TO SOMETHING ELSE!)
        foreach (GameObject o in coveringObjects)
        {
            o.gameObject.tag = "Untagged";
            //Debug.Log(o.GetComponent<Collider2D>().tag);
        }
        // Then remove them from the list
        coveringObjects.Clear();
        // Player has now left cover
        player.IsInCover = false;
        // Also remove POI from feet
        Debug.Log("Player is not behind cover.");
        * =================================================================================================
        * END OLD CODE FOR DYNAMIC COVER SEEKING
        * =================================================================================================
        */
    }

    // ExitCover
    // Player is no longer hidden behind cover and leaves the CoverNode
    // Takes: nothing
    // Returns: nothing
    public void ExitCover()
    {
        //Debug.Log("=================================== <color=blue>Exiting from cover.</color> ===================================");
        player.IsInCover = false;
        isAtSameCoverLocation = false;
        player.Animator.SetBool("cover", false);

        // Don't forget to empty out the list since we're not at the same CoverNode any more
        coveringObjects.Clear();

        /* 
         * =================================================================================================
         * START OLD CODE FOR DYNAMIC COVER SEEKING
         * =================================================================================================
        foreach (GameObject o in coveringObjects)
        {
            o.gameObject.tag = "Untagged";
            //Debug.Log(o.GetComponent<Collider2D>().tag);
        }
        coveringObjects.Clear();
        player.IsInCover = false;
        Debug.Log("Player has exited from cover.");
        * =================================================================================================
        * END OLD CODE FOR DYNAMIC COVER SEEKING
        * =================================================================================================
        */
    }

    // FindDirection
    // Determines the angle of an object so that we know what directions the player is protected
    // Takes: GameObject
    // Returns: string
    public string FindDirection(GameObject o)
    {
        // What direction is the target object located?
        string coveredDir;

        // -----------------------------------------------------------------------------------------------
        // START CALCULATE ANGLE TO OBJECT

        // What angle is the object (in degrees)?
        float angle;
        Vector2 dir = o.transform.position - transform.position;

        // For conversion/signage purposes; we don't want angle to calculate beyond 360 degrees
        float sign, offset;
        sign = (dir.y >= 0) ? 1 : -1;
        offset = (sign >= 0) ? 0 : 360;

        // Grab the final angle in degrees (adjusted)
        angle = Vector2.Angle(Vector2.right, dir) * sign + offset;

        // END CALCULATE ANGLE TO OBJECT
        // -----------------------------------------------------------------------------------------------

        // Now determine what directions the player is covered in
        /*
        if (angle > 10 && angle < 80)
        {
            coveredDir = "NE";
        }
        else if (angle >= 80 && angle <= 100)
        {
            coveredDir = "N";
        }
        else if (angle > 100 && angle < 170)
        {
            coveredDir = "NW";
        }
        else if (angle >= 170 && angle <= 190)
        {
            coveredDir = "W";
        }
        else if (angle > 190 && angle < 260)
        {
            coveredDir = "SW";
        }
        else if (angle >= 260 && angle <= 280)
        {
            coveredDir = "S";
        }
        else if (angle > 280 && angle < 350)
        {
            coveredDir = "SE";
        }
        else if (angle >= 350 && angle <= 360 || angle >= 0 && angle <= 10)
        {
            coveredDir = "E";
        }
        else
        {
            // Technically there should never not be any cover but just in-case...
            coveredDir = "";
        }*/

        // 06-01-2019
        // Reduced to only 4-points for simplicity
        if (angle >= 45 && angle <= 135)
        {
            coveredDir = "N";
        }
        else if (angle > 135 && angle <= 225)
        {
            coveredDir = "W";
        }
        else if (angle > 225 && angle <= 315)
        {
            coveredDir = "S";
        }
        else if (angle > 315 && angle <= 360 || angle >= 0 && angle < 45)
        {
            coveredDir = "E";
        }
        else
        {
            coveredDir = "";
        }

        //Debug.Log("Angle to object: " + angle);
        Debug.Log("Can't be hit from " + coveredDir);
        return coveredDir;
    }

    // WIP to transle direction to string
    public string FindDirection(Vector2 dir)
    {
        string direction = "";
        if (dir.x > 0)
        {
            if (dir.y > 0)
            {
                direction = "NE";
            }
        }
        return direction;
    }
}
