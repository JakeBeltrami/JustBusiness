using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================
	Purpose:
		Displays player aiming
	Parameters:
		None
	Dependencies:
        Player Sprite Renderer
        Player Script
	Author:
		Trent Williams
	Changelog:
		08-05-2019: Rotates arm to track mouse
		21-05-2019: Updates for pistol aiming
	Notes:
        Needs to be adjusted to work for both player and enemies
======================================================================
*/

[RequireComponent(typeof(SpriteRenderer))]
public class Aiming : MonoBehaviour
{
    public Vector2 frontArmPosition;
    public Vector2 backArmPosition;
    public Vector2 leftArmPosition;
    public Vector2 rightArmPosition;

    private string direction;
    private int sortingOrder;
    private SpriteRenderer armSpriteRenderer;
    private SpriteRenderer parentSpriteRenderer;
    private bool aiming;
    private EntityInput myInput;
    private Animator animator;

    public string prefix; // Path to folder in Resources

    void Start()
    {
        armSpriteRenderer = GetComponent<SpriteRenderer>();
        parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        sortingOrder = parentSpriteRenderer.sortingOrder;
        armSpriteRenderer.enabled = false;
        aiming = false;
        myInput = GetComponentInParent<EntityInput>();
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        aiming = animator.GetBool("aiming");
        if (myInput.Target != null)
        {
            Aim(myInput.Target.transform.position);
        }
    }

    // WIP rework to allow use for all Entities
    public void Aim(Vector2 target)
    {
        // Change to check if aiming from Entity?
        if (aiming)
        {
            armSpriteRenderer.enabled = true;

            // Fetch Entity sprite name
            string spriteName = parentSpriteRenderer.sprite.name;
            // Trim string to last character to get direction as string
            direction = spriteName.Substring(spriteName.Length - 1);
            // Set sprite (Based on gun somehow?)
            armSpriteRenderer.sprite = Resources.Load<Sprite>(prefix + spriteName + "_Arm_Pistol");


            // Change arm position
            switch (direction)
            {
                case "U":
                    transform.localPosition = backArmPosition;
                    armSpriteRenderer.sortingOrder = sortingOrder;
                    break;
                case "D":
                    transform.localPosition = frontArmPosition;
                    armSpriteRenderer.sortingOrder = sortingOrder + 1;
                    break;
                case "L":
                    transform.localPosition = leftArmPosition;
                    armSpriteRenderer.sortingOrder = sortingOrder;
                    break;
                case "R":
                    transform.localPosition = rightArmPosition;
                    armSpriteRenderer.sortingOrder = sortingOrder + 1;
                    break;
            }

            FaceTarget(target);
        }
        else
        {
            armSpriteRenderer.enabled = false;
        }
    }

    private void FaceTarget(Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;
        transform.up = direction;
    }
}
