using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : NewInventory
{
    private bool canPickup;
    // Dictionary to map weapon type to UI
    // public GameObject test;

    public bool CanPickup { get { return canPickup; } set { canPickup = value; } }

    public override void Start()
    {
        base.Start();
        canPickup = true;
    }

    public void Update()
    {
        // For testing purposes
        // if (Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     Pickup(test);
        // }
        // if (Input.GetKeyDown(KeyCode.Tab))
        // {
        //     Drop();
        // }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            // Debug.Log("Can't pickup");
            // If we can pickup and it's not already our weapon
            if (canPickup)// && other.gameObject != pickup)
            {
                Pickup(other.gameObject);
            }
            //Pickup(other.gameObject);
        }
    }
}
