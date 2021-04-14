using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewInventory : MonoBehaviour
{
    public BaseAttack weapon; // Set if starting with weapon
    protected GameObject pickup;
    protected bool canSwap;
    protected Animator animator;

    public BaseAttack Weapon { get { return weapon; } }

    public virtual void Start()
    {
        canSwap = true;
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Pickup a weapon
    /// </summary>
    public virtual void Pickup(GameObject g)
    {
        // Swap if we already have a weapon
        if (pickup != null)
        {
            if (canSwap)
            {
                canSwap = false;
                StartCoroutine("Swap", g);
            }
        }
        else
        {
            
            // Store pickup
            pickup = g;
            // Add component to player
            // If presets are wanted for gun types, can make scripts? Or functions
            weapon = (BaseAttack)gameObject.AddComponent(pickup.GetComponent<BaseAttack>().GetType());
            // Set Animator
            if (weapon.GetComponent<GunAttack>() != null)
            {
                animator.SetBool("hasGun", true);
            }
            // Set the ammo
            weapon.ammo = pickup.GetComponent<BaseAttack>().Ammo;

            // Hide the pickup
            //pickup.GetComponent<SpriteRenderer>().enabled = false;
			pickup.SetActive(false);
            // Disable POI
            pickup.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Drop a weapon
    /// </summary>
	public void Drop(GameObject g)
    {
        // Check if we have a weapon to drop
        if (pickup != null)
        {
            // Set the ammo
            pickup.GetComponent<BaseAttack>().Ammo = weapon.ammo;

            // Relocate the pickup and show it
            // pickup.transform.position = GetComponent<Entity>().Tile.Center;
            pickup.transform.position = g.transform.position; // Use tile instead
			pickup.GetComponent<SetTile>().pos = g.GetComponent<SetTile>().pos;
			pickup.GetComponent<SetTile>().Tile = g.GetComponent<SetTile>().Tile;
            
            //pickup.GetComponent<SpriteRenderer>().enabled = true;
			pickup.SetActive(true);
            // Enable POI
            pickup.transform.GetChild(0).gameObject.SetActive(true);

            // Remove references
            pickup = null;
            
            Destroy(weapon); // Doesn't work properly
        }
    }

    private IEnumerator Swap(GameObject g)
    {
        yield return new WaitForFixedUpdate();
        Drop(g);
        // yield return new WaitForFixedUpdate();
        Pickup(g);
        yield return new WaitForSecondsRealtime(1f);
        canSwap = true;
    }

    //public void Update()
   // {
     //   gameObject.GetComponent<SetTile>().Tile = GameObject.FindGameObjectWithTag("GameController").GetComponent<PathFind.Grid>().GetTile(transform.position);
   // }

    /// <summary>
    /// Destroys the current weapon and sets the UI to melee
    /// </summary>
    public virtual void Remove()
    {
        if (weapon != null)
        {
            // Set animator
            if (weapon.GetComponent<GunAttack>() != null)
            {
                animator.SetBool("hasGun", false);
            }
            // Remove component
            Destroy(weapon);
        }
    }
}
