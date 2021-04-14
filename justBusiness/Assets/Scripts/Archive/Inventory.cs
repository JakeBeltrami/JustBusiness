using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/*
======================================================================

	Purpose:
		Core functions for player object's inventory

	Parameters:
		None

	Dependencies:
		Pickup.cs

	Author:
		William Tat

	Changelog:
		26-03-2019	Initial version
		02-04-2019	Prototype-ready changes added
		10-04-2019	Incorporated controls from Player.cs temporarily
        11-04-2019  Equip/Discard functions moved to Player.cs
        04-05-2019  Added Inspector tooltips
	Notes:


======================================================================
*/

public class Inventory : MonoBehaviour
{
    // These variables are reserved specifically for pickups
    private GameObject[] nearestPickups;        // Public for testing, private for later versions
    private Pickup pickup;
    private Player player;

    [Tooltip("This is a link to the Canvas UI element that contains the pickup icon.\n\nMust be defined or the player won't know they have something equipped!")]
    public GameObject equippedPickupUIAmmo;	// Change to array when additional pickup types are added

    [Tooltip("What pickup does the object currently have in their inventory?")]
    public GameObject equippedPickup;		    // This is the current equipped pickup

    [Tooltip("This flag is used to validate whether the object has something equipped in their inventory.\n\nShouldn't need to be touched unless you want to disable picking up objects deliberately.")]
    public bool hasPickupEquipped = false;

    public GameObject equippedPickupUIImage;

    private int ammo = 0;

    // Should have sprites from Asset/Sprites/Weapons/UI
    public Sprite pistolUISprite;
    public Sprite tommygunUISprite;
    public Sprite unarmedUISprite;


    void Awake()
    {
        player = GetComponent<Player>();
    }

    // Changed to not be an IEnumerator
    void Start()
    {
        // yield return null;

        // This is needed for showing/hiding the UI; change for actual prototype when unique pickups are added
        GameObject pickupUI = Instantiate(equippedPickupUIAmmo);
        GameObject pickupUIImage = Instantiate(equippedPickupUIImage);
    }

    // Modified by K.C. (11-04-2019)
    // Moved to Equip and Discard function call to Player.cs
    void Update()
    {
        if (hasPickupEquipped)
        {
            equippedPickupUIAmmo.GetComponent<Text>().text = (equippedPickup.GetComponent<Weapon>().ammoCount).ToString();
            if (equippedPickup.name == "Pistol" || equippedPickup.name == "TogglePistol")
            {
                equippedPickupUIImage.GetComponent<Image>().sprite = pistolUISprite;
            }
            else if (equippedPickup.name == "Tommy Gun")
            {
                equippedPickupUIImage.GetComponent<Image>().sprite = tommygunUISprite;
            }
            //equippedPickupUIImage.GetComponent<Image>().sprite = equippedPickup.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            equippedPickupUIAmmo.GetComponent<Text>().text = "∞/∞";
            equippedPickupUIImage.GetComponent<Image>().sprite = unarmedUISprite;
        }
    }

    // Equip the pickup, if the player already has something equipped then swap pickups instead
    // Takes: nothing
    // Returns: nothing
    public void EquipPickup()
    {
        // Search for nearby valid pickups
        nearestPickups = GameObject.FindGameObjectsWithTag("Pickup");

        // This needs to be optimised...
        for (int i = 0; i < nearestPickups.Length; i++)
        {
            // Continue searching for a valid pickup
            pickup = nearestPickups[i].GetComponent<Pickup>();

            // Only the nearest pickups apply
            if (pickup.canPickup == true)
            {
                // If the player isn't carrying anything then take it
                if (!hasPickupEquipped)
                {
                    hasPickupEquipped = true;
                    //Show(equippedPickupUIAmmo);
                    //Show(equippedPickupUIImage);
                    Equip(nearestPickups[i]);       // "Equip" the pickup
                    pickup.canPickup = false;   // This is a failsafe so that the OnCollisionExit2D event always reverts the pickup's status
                    Hide(equippedPickup);       // Hide the pickup
                                                //Debug.Log("You took pickup " + pickup);
                    break;
                }
                // Or if they have something already, then swap it out and drop the previous one
                else if (hasPickupEquipped)
                {
                    Drop(equippedPickup);       // Drop the last pickup
                    Show(equippedPickup);       // Unhide the previous pickup (this is a placeholder)
                    Equip(nearestPickups[i]);       // Swap the pickups
                    pickup.canPickup = false;   // Same as above for picking up stuff for the first time
                    Hide(equippedPickup);       // Hide the pickup
                                                //Debug.Log("You swapped your current pickup with " + pickup);
                    break;
                }
                else
                {
                    // Otherwise do nothing
                    Debug.Log("You can't take that");
                }
            }
            else
            {
                // Ditto
                //Debug.Log("There's nothing around to take");
            }
        }
    }

    // Ditch the pickup if something's already equipped and "drop" it from the player
    // Takes: nothing
    // Returns: nothing
    public void DiscardPickup()
    {
        if (hasPickupEquipped)
        {
            Debug.Log("Discard");
            hasPickupEquipped = false;
            Drop(equippedPickup);
            Show(equippedPickup);       // Show the last equipped pickup on the ground first (this is just a placeholder)
            equippedPickup = null;      // Reset the inventory's status
                                        //Hide(equippedPickupUIAmmo);
                                        //Hide(equippedPickupUIImage);
                                        //Debug.Log("Discarded pickup");
        }
    }

    // Change the currently equipped pickup and attach it to the player (the pickup remains hidden)
    // Takes: GameObject
    // Returns: nothing
    public void Equip(GameObject g)
    {
        //g.transform.parent = player.transform;
        equippedPickup = g;
        // ammo = equippedPickup.GetComponent<Weapon>().ammoCount;
    }

    // Show GameObject element
    // Takes: GameObject
    // Returns: nothing
    void Show(GameObject obj)
    {
        obj.SetActive(true);
    }

    // Hide GameObject element
    // Takes: GameObject
    // Returns: nothing
    void Hide(GameObject obj)
    {
        obj.SetActive(false);
    }

    // Drop GameObject element at Player's position
    // Takes: GameObject
    // Returns: nothing
    void Drop(GameObject obj)
    {
        Transform t1 = obj.GetComponent<Transform>();
        Transform t2 = player.GetComponent<Transform>();
        t1.transform.position = t2.transform.position;
    }

    // public void Throw(Vector2 cursorPosition, Vector2 shootPosition)
    // {
    // 	Vector2 direction = cursorPosition - shootPosition;
    // 	Rigidbody2D weaponProjectile = Instantiate (equippedPickup.GetComponent<Rigidbody2D>(), shootPosition, Quaternion.identity) as Rigidbody2D;
    // 	weaponProjectile.velocity = direction * 2f;

    // 	hasPickupEquipped = false;
    // 	Show(equippedPickup);		// Show the last equipped pickup on the ground first (this is just a placeholder)
    // 	equippedPickup.GetComponent<Pickup>().enabled = false;
    // 	equippedPickup = null;		// Reset the inventory's status
    // 	Hide(equippedPickupUIElement);
    // }

}
