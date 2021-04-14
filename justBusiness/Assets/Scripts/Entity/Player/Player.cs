using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
======================================================================

	Purpose:
		Controls Player Movement

	Parameters:
		None

	Dependencies:

	Author:
		Kshitij Choudhary

	Changelog:
		30-04-2019  Changed getkey to getaxis, E for pick and drop
        05-05-2019  Now supports multiple forms of input (designer's discretion)
        13-05-2019  Removed redundant functions related to M&E system
        09-09-2019  Added support for cover system
        01-10-2019  More support for cover system; stores what directions player can be hit from

	Notes:

======================================================================
*/

public class Player : Entity
{
    // Variables for sprite assets

    private AudioSource audioSource;
    private Snap snapUI;
    private PlayerInput playerInput;

    // Added by W.T. (05-05-2019)
    // For Inspector options so that designers can choose what input to test with
    public enum InputType
    {
        KBM,         // Keyboard and mouse controls
        Controller   // Controller controls (PS, Xbox, etc.)
    }
    [Tooltip("Select the input type for the player's movement. Default is always set to mouse only for input.\n\n" +
        "[Mouse Buttons Only] Left-click to dash + move, Right-click to shoot (with weapon)\n\n" +
        "[Controller] Controls TBD")]
    public InputType inputOption;
    public bool playerDead = false;
    public AudioClip deathRattle;

    // Added by W.T. (01-09-2019)
    // Used to detect whether the player is hiding behind cover or not
    private bool isInCover = false;
    // And what directions they're shielded from
    [SerializeField]
    private List<string> directionsCantBeHitFrom = new List<string>();
    public bool IsInCover { get { return isInCover; } set { isInCover = value; } }
    public List<string> BulletsCantHitFromTheseDirections { get => directionsCantBeHitFrom; set => directionsCantBeHitFrom = value; }
    public Snap Snap { get { return snapUI; } }
    public PlayerInput PlayerInput { get { return playerInput; } }

    public override void Start()
    {
        base.Start();
        playerInput = GetComponent<PlayerInput>();
        snapUI = GetComponentInChildren<Snap>();
        audioSource = GetComponent<AudioSource>();
    }

    public override IEnumerator TakeTurn()
    {
        //Debug.Log(myInput.Action);
        switch (myInput.Action)
        {
            case ("Move"):
                yield return (StartCoroutine(myInput.Move(myInput.Target.GetComponent<IHasTile>().Tile)));
                break;

            case ("Roll"):
                yield return (StartCoroutine(myInput.Roll(MyInput.Target.GetComponentInParent<IHasTile>().Tile)));
                break;

            case ("Melee"):
                melee.Attack(myInput.Target.GetComponent<BoxCollider2D>().bounds.center, myInput.Mask);
                break;

            case ("MeleeMove"):
                yield return (StartCoroutine(myInput.MeleeMove(myInput.Target.GetComponent<IHasTile>().Tile)));
                break;

            case ("Shoot"):
                myInput.Fire(myInput.Target);
                yield return null;
                break;
        }
    }

    // private void FaceMouse()
    // {
    //     SetDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    // }

    /*
     public override void TakeDamage(int amount, Vector2 source, string type = "melee")
     {
         base.TakeDamage(amount, source, type);

     }
     */
    public override void TakeDamage(int amount, Vector2 source, string type = "none")
    {
        // Shake camera
        // Play animation and sound

        base.TakeDamage(amount, source, type);
        gameManager.DisplayRed();
    }

    public override void Die()
    {
        // Play Death animation
        animator.SetBool("dead", true);
        // Disable Enemies from tracking you
        foreach (GameObject e in gameManager.Enemies)
        {
            e.GetComponentInChildren<FieldOfView>().enabled = false;
        }
        transform.position += Vector3.up * 0.1f;
        // GetComponent<SpriteRenderer>().sortingOrder -= 1;
        shadow.enabled = false;
        playerDead = true;
        audioSource.clip = deathRattle;
        audioSource.Play();
        PlayerPrefs.SetInt("Tutorial", 0);
        //StartCoroutine(gameManager.GameOver());
    }
}
