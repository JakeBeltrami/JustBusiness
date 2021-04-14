using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		This script is applied onto objects that are considered 
        to be boolean-able. It stores all attributes checked to 
        handle boolean operations.

        Functions from BooleanSystem.cs will be relying on checks to see 
        whether this object has this component attached or not
        to carry out booleans.

	Parameters:
		(OPTIONAL) transformsInto [GameObject]
        (OPTIONAL) orientationLimits [enum]
        (OPTIONAL) alternatePosition [GameObject]
        (OPTIONAL) swappableWeaponTypes [GameObject]
        (OPTIONAL) linkedTo [GameObject]
        (OPTIONAL) isHiddenInitially [bool]

	Dependencies:
		BooleanSystem.cs
        Patrol.cs

	Author:
		William Tat

	Changelog:
		07-05-2019	Initial version
        08-07-2019  Added extra attributes
        12-05-2019  Added variables to handle enemy line-of-sight
        19-05-2019  Shifted over functionality from BooleanSystem.cs to support individual orientation limits

	Notes:
        

======================================================================
*/

//public class BooleanObject : MonoBehaviour
//{
//    [HideInInspector]
//    public string booleanType;          // For storing the tag to determine what we are (needed by BooleanSystem.cs)

//    [Header("FOR ENEMIES")]
//    [Tooltip("Determines what enemy it can 'transform' into.\n\nSimply link an enemy from any source and the script will take care of the rest.\n\nIf nothing is defined, the script will not 'transform' this enemy even if clicked on.")]
//    public GameObject transformsInto;

//    public enum enumDir
//    {
//        up,
//        down,
//        left,
//        right
//    }

//    [Tooltip("What is the initial facing direction of the enemy?")]
//    public enumDir initialDir = enumDir.down;

//    [Tooltip("What is the toggled facing direction of the enemy?")]
//    public enumDir alternateDir = enumDir.up;

//    [Tooltip("Does the enemy have a gun")]
//    public bool hasGun = false;

//    [Header("FOR PICKUPS")]
//    [Tooltip("Reference to a GameObject in the scene. This dummy GameObject's position will be used for relocating the pickup.\n\nIt can be anything but SHOULD preferably be an empty GameObject.\n\nIf no GameObject is defined, the script will not do anything to this object once it fires.")]
//    public GameObject alternatePosition;
//    [Tooltip("What can this weapon swap into?\n\nIf nothing is defined, then the script will not alter the pickup's weapon type.")]
//    public GameObject swappableWeaponType;

//    [Header("FOR DOODADS")]
//    [Tooltip("Reference to a GameObject in the scene. This other GameObject (tagged a 'Doodad') will be its counterpart for boolean operations.\n\nIf this GameObject is clicked on, the doodad in this field will have its state changed (unhidden if hidden).\n\n*** MAKE SURE THIS DOODAD'S COUNTERPART IS NOT LINKED TO ITSELF AND IS ALSO LINKED BOTH WAYS! ***")]
//    public GameObject linkedTo;
//    [Tooltip("This flag sets whether this doodad is hidden by default at the game's start.\n\nIf true, then it will be hidden once the level loads.")]
//    public bool isHiddenInitially = false;

//    // Internal variables
//    [HideInInspector]
//    public bool isFlipped;                      // [ENEMY] Has my heading been flipped yet?
//    [HideInInspector]
//    public bool isTransformed;                  // [ENEMY] Have I morphed into my linked type yet?
//    [HideInInspector]
//    public Quaternion facing;                   // [ENEMY] My position
//    [HideInInspector]
//    public bool isPatrollingAlternateRoute;     // [ENEMY] Am I using my alternate patrol route?
//    [HideInInspector]
//    public bool isRelocated;                    // [PICKUP] Has my position been swapped yet?
//    [HideInInspector]
//    public Vector3 originalPos;                 // [PICKUP] Where were we originally located?
//    [HideInInspector]
//    public Vector3 altPos;                      // [PICKUP] The coordinates of my alternatePosition dummy
//    [HideInInspector]
//    public bool isAltered;                      // [PICKUP] Have I been altered yet?
//    [HideInInspector]
//    public bool isHidden;                       // [DOODAD] Have I been hidden yet?

//    // ***************************************************
//    // Specifically for storing enemy stuff
//    // ***************************************************
//    [HideInInspector]
//    public float cHP, aHP, cFov, aFov, cSpd, aSpd;
//    [HideInInspector]
//    public GameObject cWeap, aWeap;
//    // ***************************************************
//    // Specifically for storing weapon stuff
//    // ***************************************************
//    [HideInInspector]
//    public Sprite cSprite, aSprite;
//    [HideInInspector]
//    public int cAmmo, aAmmo;
//    [HideInInspector]
//    public float cRate, aRate;
//    [HideInInspector]
//    public float cRange, aRange;
//    [HideInInspector]
//    public GameObject cBullet, aBullet;
//    [HideInInspector]
//    public float cBltSpd, aBltSpd;
//    // ***************************************************
//    // ***************************************************

//    // Added by Trent
//    // Reference the Game Manager to check if planning phase has ended
//    private bool planningPhase;

//    string SpriteForEnemy(enumDir direction, bool outlined, bool hasGun)
//    {
//        // R, B, F, L
//        string dirStr = "";
//        switch (direction)
//        {
//            case enumDir.left: dirStr = "L"; break;
//            case enumDir.right: dirStr = "R"; break;
//            case enumDir.up: dirStr = "B"; break;
//            case enumDir.down: dirStr = "F"; break;
//        }

//        if (hasGun && !outlined)
//        {
//            return "Enemy/Regular_Grunt_Idle_Gun_" + dirStr;
//        }
//        else
//        {
//            if (outlined && !hasGun)
//            {
//                return "Enemy/Highlighted/Regular_Grunt_Idle_" + dirStr + "_Outline";
//            }
//            else if (outlined && hasGun)
//            {
//                return "Enemy/Highlighted/Regular_Grunt_Idle_Gun_" + dirStr + "_Outline";
//            }
//            else
//            {
//                Debug.Log("No longer outlined");
//                return "Enemy/Regular_Grunt_Idle_" + dirStr;
//            }
//        }
//    }

//    public void DisableOutline()
//    {
//        planningPhase = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Planning;
//        string spriteName;
//        if (isFlipped)
//        {
//            spriteName = SpriteForEnemy(alternateDir, planningPhase, hasGun);
//        }
//        else
//        {
//            spriteName = SpriteForEnemy(initialDir, planningPhase, hasGun);
//        }

//        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spriteName);
//    }

//    void Start()
//    {
//        booleanType = tag;                                  // What are we?
//        originalPos = transform.position;                   // Here we are!
//        facing = transform.rotation;                        // What's our position (we only care about Y axis...for now)
//        planningPhase = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Planning;

//        // Initialise this object based on boolean settings
//        switch (booleanType)
//        {
//            // ******************************
//            // FOR ENEMIES
//            // ******************************
//            case "Enemy":
//                bool outlined = false;
//                Debug.Log("Planning Phase is: " + planningPhase);
//                if (planningPhase)
//                {
//                    outlined = initialDir != alternateDir;
//                    Debug.Log(initialDir);
//                    Debug.Log(alternateDir);
//                }
//                string spriteName = SpriteForEnemy(initialDir, outlined, hasGun);
//                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spriteName);

//                switch (initialDir)
//                {
//                    case enumDir.right:
//                        gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
//                        break;
//                    case enumDir.up:
//                        gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 90);
//                        break;
//                    case enumDir.left:
//                        gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 180);
//                        break;
//                    case enumDir.down:
//                        gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 270);
//                        break;
//                }

//                isFlipped = false;      // This enemy hasn't turned heading yet
//                isTransformed = false;  // Obviously haven't morphed yet
//                try
//                {
//                    // Current enemy stats storage
//                    cWeap = GetComponent<Enemy>().weapon;
//                    cHP = GetComponent<Enemy>().health;
//                    cSpd = GetComponent<Enemy>().Speed;

//                    // Edited by Trent
//                    // Needs to be checked if null
//                    // Ditto, but for the linked enemy
//                    if (transformsInto != null)
//                    {
//                        // aWeap = transformsInto.GetComponent<Enemy>().weapon2;
//                        aHP = transformsInto.GetComponent<Enemy>().health;
//                        aSpd = transformsInto.GetComponent<Enemy>().Speed;
//                    }

//                }
//                catch (UnassignedReferenceException)
//                {

//                }
//                break;
//            // ******************************
//            // FOR PICKUPS
//            // ******************************
//            case "Pickup":
//                isRelocated = false;    // We obviously haven't moved yet (...or have we?)
//                isAltered = false;      // Nor have we been changed
//                try
//                {
//                    altPos = alternatePosition.transform.position;      // Our dummy is located here

//                    // Current weapon stats storage
//                    cSprite = GetComponent<SpriteRenderer>().sprite;
//                    cAmmo = GetComponent<Weapon>().ammoCount;
//                    cRate = GetComponent<Weapon>().fireRate;
//                    cRange = GetComponent<Weapon>().range;
//                    cBullet = GetComponent<Weapon>().bullet;
//                    cBltSpd = GetComponent<Weapon>().bulletSpeed;

//                    // Ditto, but for the linked weapon
//                    // Edited by Trent
//                    // Added null check
//                    if (swappableWeaponType != null)
//                    {
//                        aSprite = GetComponent<SpriteRenderer>().sprite;
//                        aAmmo = swappableWeaponType.GetComponent<Weapon>().ammoCount;
//                        aRate = swappableWeaponType.GetComponent<Weapon>().fireRate;
//                        aRange = swappableWeaponType.GetComponent<Weapon>().range;
//                        aBullet = swappableWeaponType.GetComponent<Weapon>().bullet;
//                        aBltSpd = swappableWeaponType.GetComponent<Weapon>().bulletSpeed;
//                    }

//                }
//                catch (UnassignedReferenceException)
//                {

//                }
//                break;
//            // ******************************
//            // FOR DOODADS
//            // ******************************
//            case "Doodad":
//                // If isHidden flag has been toggled then set us to be hidden from the start
//                if (isHiddenInitially)
//                {
//                    gameObject.SetActive(false);
//                    isHidden = true;    // Don't forget to set this flag!
//                }
//                break;
//            default:
//                break;
//        }
//    }

//    // Rotate the enemy's facing position
//    // Affects their line-of-sight
//    // Takes: GameObject
//    // Returns: nothing
//    public void RotateObject()
//    {
//        // Have we been flipped yet?
//        if (!isFlipped)
//        {
//            bool outlined = false;
//            if (planningPhase)
//            {
//                outlined = initialDir != alternateDir;
//            }
//            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SpriteForEnemy(alternateDir, outlined, hasGun));

//            // If so, then adjust axis to opposite direction based on designer choice
//            switch (alternateDir)
//            {
//                case BooleanObject.enumDir.right:
//                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
//                    break;
//                case BooleanObject.enumDir.up:
//                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 90);
//                    break;
//                case BooleanObject.enumDir.left:
//                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 180);
//                    break;
//                case BooleanObject.enumDir.down:
//                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 270);
//                    break;
//            }
//            // Toggle flag
//            isFlipped = true;
//            Debug.Log("Enemy has been rotated!");
//        }
//        else if (isFlipped)
//        {
//            bool outlined = false;
//            if (planningPhase)
//            {
//                outlined = initialDir != alternateDir;
//            }
//            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SpriteForEnemy(initialDir, outlined, hasGun));

//            // First check to see what mode we're in so that we "revert" properly
//            // Then "reset" our rotation accordingly
//            switch (initialDir)
//            {
//                case BooleanObject.enumDir.right:
//                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
//                    break;
//                case BooleanObject.enumDir.up:
//                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 90);
//                    break;
//                case BooleanObject.enumDir.left:
//                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 180);
//                    break;
//                case BooleanObject.enumDir.down:
//                    gameObject.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 270);
//                    break;
//            }
//            // Reset the flag as well
//            isFlipped = false;
//            Debug.Log("This guy has been reverted to his original direction.");
//        }
//    }


//    // Switch the enemy's patrol route
//    // Takes: GameObject
//    // Returns: nothing
//    public void SwitchObject()
//    {
//        // If we already are patrolling the alternate route then toggle us back
//        if (isPatrollingAlternateRoute)
//        {
//            // If we already are patrolling the alternate route then toggle us back
//            isPatrollingAlternateRoute = false;
//            Debug.Log("This dude is patrolling his original route.");
//        }
//        else if (!isPatrollingAlternateRoute)
//        {
//            // Otherwise if we haven't swapped to our alternate route
//            // then toggle the flag to do so
//            isPatrollingAlternateRoute = true;
//            Debug.Log("This dude is patrolling his alternate route.");
//        }
//    }

//    // Reposition the pickup to somewhere else
//    // Takes: GameObject
//    // Returns: nothing
//    public void RelocateObject()
//    {
//        GameObject obj = gameObject;
//        try
//        {
//            if (alternatePosition != null)
//            {
//                // Where were we originally located?
//                Vector3 ori = originalPos;
//                // Get the dummy's position too
//                Vector3 dum = altPos;

//                // Have we moved yet?
//                if (isRelocated)
//                {
//                    // If yes, then relocate us back to our initial position
//                    obj.transform.position = new Vector3(ori.x, ori.y, ori.z);
//                    // Separate movement for collision
//                    //obj.GetComponent<Rigidbody2D>().position = new Vector2(ori.x, ori.y);

//                    // Then move the alternate object back to the alternate position
//                    alternatePosition.transform.position = new Vector2(dum.x, dum.y);

//                    // Reset flag
//                    isRelocated = false;

//                    Debug.Log("Pickup has returned to its original spot.");
//                }
//                else if (!isRelocated)
//                {
//                    // Otherwise move us to the dummy's location
//                    obj.transform.position = new Vector3(dum.x, dum.y, dum.z);
//                    // Ditto
//                    //obj.GetComponent<Rigidbody2D>().position = new Vector2(dum.x, dum.y);

//                    // Then move the alternate object to the original location
//                    alternatePosition.transform.position = new Vector2(ori.x, ori.y);

//                    // Toggle flag
//                    isRelocated = true;

//                    Debug.Log("Pickup has been relocated!");
//                }
//            }
//        }
//        catch (UnassignedReferenceException)
//        {
//            Debug.Log("This pickup doesn't have an alternate position defined.");
//        }
//    }

//    // Change the pickup to a different weapon
//    // Takes: GameObject
//    // Returns: nothing
//    public void RefactorObject()
//    {
//        GameObject obj = gameObject;
//        try
//        {
//            // What are we swapping into?
//            GameObject altWeap = swappableWeaponType;

//            if (!isAltered)
//            {
//                // Store our own stats first for swapping back to later
//                aSprite = obj.GetComponent<SpriteRenderer>().sprite;
//                aAmmo = obj.GetComponent<Weapon>().ammoCount;
//                aBltSpd = obj.GetComponent<Weapon>().bulletSpeed;
//                aBullet = obj.GetComponent<Weapon>().bullet;
//                aRange = obj.GetComponent<Weapon>().range;
//                aRate = obj.GetComponent<Weapon>().fireRate;

//                // Then store the linked weapon's stats
//                cSprite = altWeap.GetComponent<SpriteRenderer>().sprite;
//                cAmmo = altWeap.GetComponent<Weapon>().ammoCount;
//                cBltSpd = altWeap.GetComponent<Weapon>().bulletSpeed;
//                cBullet = altWeap.GetComponent<Weapon>().bullet;
//                cRange = altWeap.GetComponent<Weapon>().range;
//                cRate = altWeap.GetComponent<Weapon>().fireRate;

//                // Now apply all of its stats to this one
//                obj.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<BooleanObject>().cSprite;
//                obj.GetComponent<Weapon>().ammoCount = obj.GetComponent<BooleanObject>().cAmmo;
//                obj.GetComponent<Weapon>().bulletSpeed = obj.GetComponent<BooleanObject>().cBltSpd;
//                obj.GetComponent<Weapon>().bullet = obj.GetComponent<BooleanObject>().cBullet;
//                obj.GetComponent<Weapon>().range = obj.GetComponent<BooleanObject>().cRange;
//                obj.GetComponent<Weapon>().fireRate = obj.GetComponent<BooleanObject>().cRate;

//                // Toggle flag
//                isAltered = true;

//                Debug.Log("Weapon type has been changed to " + altWeap.name + "!");
//            }
//            else if (obj.GetComponent<BooleanObject>().isAltered)
//            {
//                // Revert our stats back to the initial weapon
//                obj.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<BooleanObject>().aSprite;
//                obj.GetComponent<Weapon>().ammoCount = obj.GetComponent<BooleanObject>().aAmmo;
//                obj.GetComponent<Weapon>().bulletSpeed = obj.GetComponent<BooleanObject>().aBltSpd;
//                obj.GetComponent<Weapon>().bullet = obj.GetComponent<BooleanObject>().aBullet;
//                obj.GetComponent<Weapon>().range = obj.GetComponent<BooleanObject>().aRange;
//                obj.GetComponent<Weapon>().fireRate = obj.GetComponent<BooleanObject>().aRate;
//                // Reset flag
//                isAltered = false;
//                Debug.Log("Reverted back to the original " + obj.name + ".");
//            }
//        }
//        catch (UnassignedReferenceException)
//        {
//            Debug.Log("Weapon type has not been changed!");
//        }
//    }

//    // Changes the visiblity state of the doodad(s)
//    // Takes: GameObject
//    // Returns: nothing
//    public void SetObject()
//    {
//        GameObject obj = gameObject;
//        try
//        {
//            // Unhide its counterpart...
//            linkedTo.SetActive(true);
//            // ...and toggle its isHidden flag
//            isHidden = false;
//            Debug.Log(linkedTo.name + " has been revealed!");
//            // ...but hide ourselves as well
//            obj.SetActive(false);
//            // Plus the flag
//            isHidden = true;
//            Debug.Log(obj.name + " has been hidden!");
//        }
//        catch (UnassignedReferenceException)
//        {
//            Debug.Log("This doodad doesn't have a counterpart defined.");
//        }
//    }

//}
