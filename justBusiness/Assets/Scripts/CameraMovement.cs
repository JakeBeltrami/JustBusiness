using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

/*
======================================================================

	Purpose:
		Controls Camera movement

	Parameters:
		None

	Dependencies:
        Player.cs

	Author:
		Kshitij Choudhary

	Changelog:
        

	Attached:
        Main Camera
    
    Notes:
        Merged Camera Shake in (Trent)
        Will need to set HideRed here?
        

======================================================================
*/

public class CameraMovement : MonoBehaviour
{
    public static bool cameraMove = true;

    private Transform playerTransform; // Gameobject for player
    private Vector3 offset; // distance from player

    // How long the object should shake for.
    public float shakeDuration = 1f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 5f;

    private float timer = 0f;
    // duration for which player is immune to camera shake
    public float shakeImmunity = 3f;

    public bool shake = false;

    // public GameObject redImage;

    private Vector3 camPos;

    private float originalShakeDuration;

    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //Find player using tag
        // redImage = GameObject.Find("EnergyBar/Canvas/Red");
        originalShakeDuration = shakeDuration;
        camPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        offset = new Vector3(0, 0, 10);
        Time.timeScale = 1;
    }

    void Update()
    {
        // For testing purposes
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     shake = true;
        // }
        if (shake)
        {
            timer += Time.deltaTime;
            if (shakeDuration > 0)
            {
                //camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, originalPos + Random.insideUnitSphere * shakeAmount, Time.deltaTime*3);
                transform.localPosition = Vector3.Lerp(transform.position, transform.position + Random.insideUnitSphere * shakeAmount, Time.deltaTime * 3);

                shakeDuration -= Time.deltaTime;
                // NOTE: Change this to not be global variable
                // player.InputLock = true;
            }
            else
            {
                shakeDuration = originalShakeDuration;
                transform.localPosition = Vector3.Lerp(camPos, camPos, Time.deltaTime * 3); // What's this even do?
                // Change to not be global variable
                // player.InputLock = false;
                shake = false;
                AlignCamera();
                // redImage.SetActive(false);
            }

            if (timer > shakeImmunity)
            {
                Debug.Log("Shake immune");
                shakeDuration = originalShakeDuration;
                shake = false;
                timer = 0;
            }
        }
        else
        {
            AlignCamera();
        }
    }

    /// <summary>
    /// Aligns the camera with the player
    /// </summary>
    private void AlignCamera()
    {
        transform.position = playerTransform.position - offset;
    }
}