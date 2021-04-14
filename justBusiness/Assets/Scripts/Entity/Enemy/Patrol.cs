using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
======================================================================

	Purpose:
		Handles enemy AI patrolling

	Parameters:
		None

	Dependencies:
        Enemy.cs
        PatrolNode.cs

	Author:
		Phil

	Changelog:
		17-05-2019	Initial version
        18-05-2019  Added support for alternate patrol routes (boolean system)

	Notes:
        

======================================================================
*/

public class Patrol : MonoBehaviour
{
    [Tooltip("Use this to store the primary patrol route.\n\nYou should ALWAYS define a list of waypoints (dummy GameObjects) for this to work properly.")]
    public Vector2[] patrolPoints;
    [Tooltip("Use this to store waypoints for a secondary patrol route.\n\nThis does not always have to be defined, but you should do so if the enemy is meant to be toggled via the Booleans system.")]
    public Vector2[] altPatrolPoints;    // Added by W.T. (18-05-2019) for boolean toggle

    // Internal variables
    private int destinationNode = 0;
    private EntityInput input;
    private bool hitNode = false;
    private Vector2 destination;
    private bool altPatrol = false; // Make this true to use the alternate patrol path

    void Start()
    {
        input = GetComponent<EntityInput>();
    }

    // Edited by W.T. (18-05-2019)
    // Added support for alternate patrol routes
    public void NextPatrol()
    {
        GoToNextNode();
        IterateNode();
    }

    // Edited by W.T. (18-05-2019)
    // Added support for alternate patrol routes
    //5:30
    private void GoToNextNode()
    {
        if (!altPatrol && patrolPoints.Length == 0)
        {
            //Debug.Log("No patrol nodes set!");
            return;
        }
        else if (altPatrol && altPatrolPoints.Length == 0)
        {
            return;
        }

        if (!altPatrol)
        {
            destination = patrolPoints[destinationNode] - new Vector2(0.5f, 0.5f);
        }
        else if (altPatrol)
        {
            destination = altPatrolPoints[destinationNode] - new Vector2(0.5f, 0.5f);
        }

        if (destination != null)
        {
            StartCoroutine(GoToCurrentNode(destination));
        }
    }

    // Edited by W.T. (18-05-2019)
    // Added support for sprite changes based on enemy facing
    private IEnumerator GoToCurrentNode(Vector2 target)
    {
        if (Vector2.Distance(target, gameObject.transform.position) != 0f)
        {
            // Pretty gross line, should try to make better somehow
            yield return (StartCoroutine(input.Move(GameObject.FindGameObjectWithTag("GameController").GetComponent<PathFind.Grid>().GetTile(target))));
        }
    }

    // Edited by W.T. (18-05-2019)
    // Added support for alternate patrol routes
    private void IterateNode()
    {
        if (patrolPoints.Length != 0)
        {
            if (!altPatrol)
            {
                destinationNode = (destinationNode + 1) % patrolPoints.Length;
            }
            else if (altPatrol)
            {
                destinationNode = (destinationNode + 1) % altPatrolPoints.Length;
            }
        }
    }
}