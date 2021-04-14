using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		Core functions for the Mark And Execute input option in Player.cs
        "Mark" chooses a target and adds them to a list
        "Execute" runs a series of move actions between waypoints
        Whenever the player moves near an enemy, the game slows down
        and they have the choice of dashing or shooting them

	Parameters:
		(OPTIONAL) waypoints [GameObject]

	Dependencies:
		Player.cs

	Author:
		William Tat

	Changelog:
		04-05-2019	Initial version (not fully finished)

	Notes:
        Incomplete script; will be fleshed out at a later date

======================================================================
*/

public class MarkAndExecute : MonoBehaviour
{
    private GameObject dummyWP;                 // Dummy game object used to add new waypoints (for Mark)
    private Player player;                      // The player themselves

    [Tooltip("All waypoints that the Player will cycle through are stored in this list. Waypoints are defined by the player whenever they right-click anywhere on the map.\n\nFor a waypoint to be valid, the script will check to see whether it is inside:\n- The 'Ground' section of the Map\n- Must not be inside a collider\n- Is not within either the 'Scene' or 'Walls' portions of the Map\n\nIt is VERY important that the level designer does not add unpathable objects into the 'Ground' section of the map, otherwise the player will never be able to place any waypoints.")]
    public List<GameObject> waypoints;          // Primary list of waypoints, defined as GameObjects

    [Tooltip("Flag used to check whether the player is currently in the 'Execute' phase.\n\nShouldn't need to touch this except for debugging purposes.")]
    public bool isExecuting = false;            // Is the player current in "Execute" mode? (need this for whenever the cycle is interrupted by attacking, damage, etc.)

    void Start()
    {
        dummyWP = new GameObject("Dummy Waypoint - Mark");
        player = GetComponent<Player>();
    }

    void Update()
    {
        // Upon right-clicking...
        if (Input.GetMouseButtonDown(1))
        {
            // ...place a waypoint at the position
            Mark();
        }
    }

    // Marking targets for executing
    // Takes: nothing
    // Returns: nothing
    public void Mark()
    {
        // Get current mouse cursor position
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        // These are specifically for pathable terrain detection
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, new Vector2(0, 0), 0.01f);

        for (int i = 0; i < hits.Length; i++)
        {
            // Can't "Mark" outside of bounds or within doodads
            if (hits[i].collider.name == "Walls" || hits[i].collider.name == "Scene" || hits[i].collider.tag == "Doodad")
            {
                Debug.Log("That's an invalid spot.");
            }
            // ----------------------------------------------------------------------------------------------------
            // REQUIRED FIX: THIS PART NEEDS TO BE REFACTORED TO CHECK FOR LAYER AS OPPOSED TO COLLIDERS
            else
            {
                // Dummy waypoint object based on an empty GameObject
                GameObject wp = Instantiate(dummyWP, objectPos, Quaternion.identity);

                // Add it to the list
                waypoints.Add(wp);
                Debug.Log("Waypoint created at " + objectPos);
            }
            // ----------------------------------------------------------------------------------------------------
        }
    }

    // Begin execution phase
    // Takes: nothing
    // Returns: nothing
    public void Execute()
    {

    }
}
