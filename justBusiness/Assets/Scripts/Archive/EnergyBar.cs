using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
======================================================================

	Purpose:
		Contolrs Energy bar functionality

	Parameters:
		None

	Dependencies:

	Author:
		Phill

	Changelog:

	Notes:
		Is temp needed?

======================================================================
*/

public class EnergyBar : MonoBehaviour
{
    //[Range(0.0f, 30.0f)]
    public float maxEnergy = 30.0f; //The maximum amount of energy the player can have.
    public float timeMulti = 1; //The multiplier that changes how fast the bar drains.
    public float temp = 0;
    public Text totalEnergy; //This represents the text in the UI element that displays percentage of energy left


    [HideInInspector]
    private float energy; //e value used in code. Should only be altered by other code.
    private GameManager gameManager;
    private bool paused;

    // UI Variables
    private Image bar; //This represents the UI element that will tick down/deplete
    private int energyToDisplay; //This is the new energy value
    private float energyPercent; //This is the energy value represented as a percentage

    // Properties
    public float Energy { get { return energy; } set { energy = value; } }
    public bool Paused { get { return paused; } set { paused = value; } }
    public float TimeMulti { get { return timeMulti; } set { timeMulti = value; } }

    void Start()
    {
        energy = maxEnergy;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        paused = false;
        bar = GetComponent<Image>();
    }

    void Update()
    {
        if (!paused)
        {
            temp += Time.deltaTime * 10;
            if (temp >= 0.2f)
            {
                Reduce();
            }

            SetEnergyBar();

            //If the combo bar is empty the player is dead, load the game over scene
            if (energy == 0)
            {
                StartCoroutine(gameManager.GameOver());
            }
        }
    }

    /// <summary>
    /// Reduces the energy by a set amount
    /// </summary>
    public void Reduce()
    {
        energy = Mathf.Clamp(energy - Time.deltaTime * timeMulti, 0.0f, maxEnergy);
        temp = 0;
    }

    /// <summary>
    /// Increases the energy bar by a set amount
    /// </summary>
    private void SetEnergyBar()
    {
        if (energy != energyToDisplay) //Checks if update is needed
        {
            energyToDisplay = (int)energy;

            energyPercent = (float)energyToDisplay / maxEnergy;

        }

        totalEnergy.text = string.Format("{0}%", Mathf.RoundToInt(energyPercent * 100));
        bar.fillAmount = energyPercent; //Sprite has to be the 'filled' type
    }
}
