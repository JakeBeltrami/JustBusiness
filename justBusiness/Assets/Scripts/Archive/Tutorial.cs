using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public GameObject gameManager;

    private float timer = 0;

    public float messageDuration = 5f;

    private GameObject[] enemies;

    private GameObject player;

    private EnergyBar energyBar;

    public GameObject energyBarParent;

    public GameObject postProcessing;

    public GameObject planningText;

    public List<GameObject> tutorialPopups = new List<GameObject>();

    private int popupCount;
    private GameObject currentPopup;


    // Start is called before the first frame update
    void Start()
    {
        popupCount = 0;
        if (tutorialPopups.Count > 0)
        {
            currentPopup = tutorialPopups[0];
        }
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.Find("Player");
        energyBar = GameObject.FindGameObjectWithTag("Energy").GetComponent<EnergyBar>();
        postProcessing = GameObject.Find("Post Processing");

        planningText = GameObject.Find("Planning Phase/Text");

        gameManager.GetComponent<PlanningPhase>().tut = true;

        InstructionON();
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        /*if (gameManager.GetComponent<FrozenTime>().spacePressed == true)
        {
            InstructionON();
			/*if(Input.GetKey(KeyCode.Space))
			{
				gameManager.GetComponent<FrozenTime>().spacePressed = false;
			}/
			timer += Time.unscaledDeltaTime;
			if(timer>messageDuration)
			{
				gameManager.GetComponent<FrozenTime>().spacePressed = false;
			}

        }

		if(Input.GetKeyDown(KeyCode.Space) && gameManager.GetComponent<FrozenTime>().spacePressed == true)
		{
			Debug.Log("space");
			gameManager.GetComponent<FrozenTime>().spacePressed = false;
		}*/

        if (Input.GetKeyDown(KeyCode.Mouse0) && !gameManager.GetComponent<GameManager>().Paused)
        {
            //gameManager.GetComponent<HandlePostProcessing>().EnablePost();
            if (popupCount < tutorialPopups.Count - 1)
            {
                NextInstruction();
                Debug.Log("Next UI");
                Debug.Log(popupCount);
                Debug.Log(tutorialPopups.Count);
            }
            else
            {
                postProcessing.SetActive(true);
                tutorialPopups[popupCount].SetActive(false);
                planningText.GetComponent<Text>().text = "PLANNING PHASE IN PROGRESS \n PRESS SPACE TO START";
                planningText.GetComponent<Text>().color = Color.white;
                gameManager.GetComponent<PlanningPhase>().tut = false;
                this.enabled = false;
                //InstructionOFF();
            }
        }

        // Added by Trent
        // Quick fix for Energy Bar display
        if (popupCount == 3)
        {
            energyBarParent.SetActive(true);
            energyBar.TimeMulti = 0f;
        }
        else
        {
            energyBarParent.SetActive(false);
        }

    }

    public void InstructionON()
    {
        planningText.GetComponent<Text>().text = "Tutorial \n Click left mouse to continue";
        planningText.GetComponent<Text>().color = Color.black;

        tutorialPopups[popupCount].SetActive(true);

        // Is this not already done in Planning Phase?
        player.GetComponent<PlayerInput>().enabled = false;
        player.GetComponent<Player>().enabled = false;

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().enabled = false;
            //enemies[i].GetComponent<Patrol>().enabled = false;
        }

        Time.timeScale = 0f;

        //gameManager.GetComponent<HandlePostProcessing>().DisablePost();
        postProcessing.SetActive(false);
    }

    public void NextInstruction()
    {
        tutorialPopups[popupCount].SetActive(false);
        popupCount += 1;
        tutorialPopups[popupCount].SetActive(true);
    }

    public void ResetInstruction()
    {
        popupCount = 0;
    }


    public void InstructionOFF()
    {

        tutorialPopups[popupCount].SetActive(false);

        Time.timeScale = 1f;

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().enabled = true;
            //enemies[i].GetComponent<Patrol>().enabled = true;
        }

        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<Player>().enabled = true;

        energyBar.GetComponent<EnergyBar>().TimeMulti = 20f;

        planningText.GetComponent<Text>().text = "PLANNING PHASE IN PROGRESS \n PRESS SPACE TO START";
        planningText.GetComponent<Text>().color = Color.white;
        energyBar.GetComponent<EnergyBar>().TimeMulti = 20f;
        //gameManager.GetComponent<FrozenTime>().spacePressed = false;
    }
}
