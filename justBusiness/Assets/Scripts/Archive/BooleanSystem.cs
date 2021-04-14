using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		Core functions for the boolean object system
        For any of these functions to work, BooleanObject must be added
        as a component onto the prefab/GameObject
        Only "active" when the game is in the planning phase

	Parameters:
		None

	Dependencies:
        BooleanObject.cs
        Enemy.cs
		FrozenTime.cs
        Patrol.cs
        Eyes.cs

	Author:
		William Tat

	Changelog:
		03-05-2019	Initial version
        04-05-2019  Bugfixes to RelocateObject()
        07-05-2019  Shifted most attributes over to BooleanObject.cs
        09-05-2019  Completed most functions except for enemy-related booleans
        12-05-2019  Functionality to RotateObject() and MorphObject()
        13-05-2019  Buxfixes to RotateObject() and RefactorObject()
        15-05-2019  Enemy rotations now support all four headings
        18-05-2019  Added support for alternate patrol routes
        19-05-2019  Shifted some functionality over to BooleanObject.cs to allow for individual rotations

	Notes:
        ***SOLVED*** KNOWN ISSUE: Enemy positions get shifted once rotated
        ***SOLVED*** KNOWN ISSUE: Collision box for mouse click detection does not relocate with enemy rotation
        ***SOLVED*** BUG: Script does not function upon level restarts

======================================================================
*/

//public class BooleanSystem : MonoBehaviour
//{

//    [Header("FOR ENEMY BOOLEANS ONLY")]
//    [Tooltip("Determines whether the enemy's type gets transformed or not.")]
//    public bool changeEnemyType = true;
//    [Tooltip("Do enemies have their patrol routes changed on click?\n\nThis will have no effect even when enabled if the enemy does not have an alternate list of routes defined!")]
//    public bool changeEnemyPatrolRoute = false;
//    [Tooltip("Determines whether the boolean will rotate the enemy or not.")]
//    public bool changeEnemyRotation = true;
//    // DISABLED - REENABLE IF DESIGNERS NEED GLOBAL CHANGES
//    //[Tooltip("Determines what orientation booleans are permitted to flip in.\n\nYou can choose any of the options but this operation will not work unless 'Change Enemy Rotation' has been enabled.")]
//    //public Orientation enemyRotationDirections = Orientation.Horizontal;

//    [Header("FOR PICKUP BOOLEANS ONLY")]
//    [Tooltip("Determines whether the boolean will relocate the pickup or not.")]
//    public bool relocatePickup = true;
//    [Tooltip("Determines whether the pickup's weapon type will be modified or not.")]
//    public bool swapPickupType = true;

//    // Internal variables
//    private GameObject objectToBoolean;     // This is a reference to the object which will be boolean'd
//    private bool booleanAllowed = true;     // Whether or not we can carry out boolean operations (must be in Planning Phase no matter what)
//    private SoundManager soundManager;

//    void Start()
//    {
//        // Check if the game is in planning phase...
//        if (CameraMovement.cameraMove)
//        {
//            // ...if yes then enable booleans
//            booleanAllowed = true;
//        }
//        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
//    }

//    void Update()
//    {
//        // Check whether the game is STILL in planning phase
//        if (!CameraMovement.cameraMove)
//        {
//            // We aren't, so no more object shifting allowed
//            booleanAllowed = false;
//            // For good measure, disable the script too - not like it'll be needed anyway...
//            // Actually it DOES matter
//            // The script should now work again if the level restarts
//            //enabled = false;
//        }
//        else
//        {
//            // If that's not the case then let the "fun" begin!
//            if (Input.GetMouseButtonDown(0) && booleanAllowed)
//            {
//                BooleanStuff();
//            }
//        }
//    }

//    // For deciding on boolean operations based on object type
//    // Takes: nothing
//    // Returns: nothing
//    public void BooleanStuff()
//    {

//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);


//        // This whole block is to ensure that no errors pop up if the function can't find anything under the cursor
//        try
//        {

//            objectToBoolean = hit.collider.gameObject;  // For passing onto the other functions

//            if (objectToBoolean.tag == "altPos")
//            {
//                objectToBoolean = objectToBoolean.transform.parent.gameObject;
//            }

//            // What is it?
//            // If the object has no fields defined in BooleanObject.cs, then the script will not do anything
//            switch (objectToBoolean.GetComponent<BooleanObject>().booleanType)
//            {
//                case "Enemy":   // Rotate heading, swap their type and/or switch patrol route
//                    if (changeEnemyRotation)
//                    {
//                        objectToBoolean.GetComponent<BooleanObject>().RotateObject();
//                    }
//                    if (changeEnemyType)
//                    {
//                        try
//                        {
//                            // MorphObject(objectToBoolean);
//                        }
//                        catch (NullReferenceException)
//                        {
//                            Debug.Log("This enemy doesn't have any alternate types to transform into!");
//                        }
//                    }
//                    if (changeEnemyPatrolRoute)
//                    {
//                        objectToBoolean.GetComponent<BooleanObject>().SwitchObject();
//                    }
//                    break;
//                case "Pickup":  // Relocate us elsewhere and/or swap our "stored" weapon
//                    if (relocatePickup)
//                    {
//                        try
//                        {
//                            objectToBoolean.GetComponent<BooleanObject>().RelocateObject();
//                        }
//                        catch (NullReferenceException)
//                        {
//                            Debug.Log("This pickup doesn't have an alternate position to swap with!");
//                        }
//                    }
//                    if (swapPickupType)
//                    {
//                        try
//                        {
//                            objectToBoolean.GetComponent<BooleanObject>().RefactorObject();
//                        }
//                        catch (NullReferenceException)
//                        {
//                            Debug.Log("This pickup doesn't know what to swap into.");
//                        }
//                    }
//                    break;
//                case "Doodad":  // Hide/reveal either us or our counterpart
//                    try
//                    {
//                        objectToBoolean.GetComponent<BooleanObject>().SetObject();
//                    }
//                    catch (NullReferenceException)
//                    {
//                        Debug.Log("There's no alternate doodad defined.");
//                    }
//                    break;
//                default:        // Do nothing
//                    Debug.Log("This is not a valid object.");
//                    break;
//            }

//            soundManager.PlayBooleanSound(); // Play audio for switching
//        }
//        catch (NullReferenceException)
//        {
//            Debug.Log("There's nothing under the mouse cursor.");
//        }
//    }

//    // Swap enemy type to a different one (weaker, stronger, etc.)
//    // Takes: GameObject
//    // Returns: nothing
//    // void MorphObject(GameObject obj)
//    // {
//    //     try
//    //     {
//    //         if (obj.GetComponent<BooleanObject>().transformsInto != null)
//    //         {
//    //             // Who are we transforming into?
//    //             GameObject altEnemy = obj.GetComponent<BooleanObject>().transformsInto;

//    //             if (!obj.GetComponent<BooleanObject>().isTransformed)
//    //             {
//    //                 // Store our own stats first for swapping back to later
//    //                 obj.GetComponent<BooleanObject>().aWeap = obj.GetComponent<Enemy>().weapon2;
//    //                 obj.GetComponent<BooleanObject>().aHP = obj.GetComponent<Enemy>().health;
//    //                 obj.GetComponent<BooleanObject>().aFov = obj.GetComponent<Enemy>().fovAngle;
//    //                 obj.GetComponent<BooleanObject>().aSpd = obj.GetComponent<Enemy>().speed;

//    //                 // Then store the linked weapon's stats
//    //                 obj.GetComponent<BooleanObject>().cWeap = altEnemy.GetComponent<Enemy>().weapon2;
//    //                 obj.GetComponent<BooleanObject>().cHP = altEnemy.GetComponent<Enemy>().health;
//    //                 obj.GetComponent<BooleanObject>().cFov = altEnemy.GetComponent<Enemy>().fovAngle;
//    //                 obj.GetComponent<BooleanObject>().cSpd = altEnemy.GetComponent<Enemy>().speed;

//    //                 // Now apply all of its stats to this one
//    //                 obj.GetComponent<Enemy>().weapon2 = obj.GetComponent<BooleanObject>().cWeap;
//    //                 obj.GetComponent<Enemy>().health = obj.GetComponent<BooleanObject>().cHP;
//    //                 obj.GetComponent<Enemy>().fovAngle = obj.GetComponent<BooleanObject>().cFov;
//    //                 obj.GetComponent<Enemy>().speed = obj.GetComponent<BooleanObject>().cSpd;

//    //                 // Toggle flag
//    //                 obj.GetComponent<BooleanObject>().isTransformed = true;

//    //                 Debug.Log("Enemy type has been changed to " + altEnemy.name + "!");
//    //             }
//    //             else if (obj.GetComponent<BooleanObject>().isTransformed)
//    //             {
//    //                 // Revert our stats back to the initial enemy
//    //                 obj.GetComponent<Enemy>().weapon2 = obj.GetComponent<BooleanObject>().aWeap;
//    //                 obj.GetComponent<Enemy>().health = obj.GetComponent<BooleanObject>().aHP;
//    //                 obj.GetComponent<Enemy>().fovAngle = obj.GetComponent<BooleanObject>().aFov;
//    //                 obj.GetComponent<Enemy>().speed = obj.GetComponent<BooleanObject>().aSpd;
//    //                 // Reset flag
//    //                 obj.GetComponent<BooleanObject>().isTransformed = false;
//    //                 Debug.Log("Reverted back to the original " + obj.name + ".");
//    //             }
//    //         }
//    //     }
//    //     catch (UnassignedReferenceException)
//    //     {
//    //         Debug.Log("There aren't any enemy variants assigned to this dude.");
//    //     }
//    // }


//}
