using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		Script component for valid pickup objects
		Make sure you attach it to any prefab that is usable by the player

	Parameters:
		None

	Dependencies:
		Inventory.cs

	Author:
		William Tat

	Changelog:
		26-03-2019	Initial version (NYI)
		02-04-2019	Prototype-ready changes added
		10-04-2019	Incorporated changes to accomodate for weapon types

======================================================================
*/

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour
{
    [HideInInspector]
    public bool canPickup = false;

    // public string type; // Use for Inventory? (How to set on drop? Maybe put into BaseAttack instead)

    // Player collides with valid pickup
    void OnTriggerStay2D(Collider2D collider)
    {
        // Only player can pick this up
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerInventory>().CanPickup = false;
        }
    }

    // Player is no longer colliding with pickup
    void OnTriggerExit2D(Collider2D collider)
    {
        // Only player can pick this up
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerInventory>().CanPickup = true;

        }
    }
}