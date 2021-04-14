// Edited by Lauren Stamp 18/05/19 
// Purpose: To create a Pause Screen that allows the player to resume, restart, return to menu or quit
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    private string backToMenu;

    public string restart;

    public bool isPaused;

    public GameObject pauseMenuCanvas;

    private GameManager gameManager;

    private bool gameOver;

    //Added by K.C. 21-05-2019 for arm and enemies 

    private GameObject arm;

    //public GameObject tutorialCanvas;

    private Player player;

    public GameObject clapper;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        arm = GameObject.Find("Player/arm");
        isPaused = false;
        pauseMenuCanvas = GameObject.FindGameObjectWithTag("PauseUI");
        pauseMenuCanvas.SetActive(false);
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        //tutorialCanvas = GameObject.FindGameObjectWithTag("TutorialCanvas");
        backToMenu = "MenuEdit";
    }

    // Update is called once per frame
    void Update()
    {
        gameOver = GameObject.Find("GameOver").GetComponent<GameOverTips>().isGameOver;
        //Debug.Log(gameOver);
        // If escape key is pressed down (again), game is no longer paused
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetButtonDown("Start")) && !gameOver)
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    private void Pause()
    {
        if (!gameOver)
        {
            gameManager.Paused = true;
            // activates the pause menu canvas (screen background, text and buttons appear)
            pauseMenuCanvas.SetActive(true);
            //tutorialCanvas.SetActive(false);
            // makes the game stop working (stops everything from running)
            Time.timeScale = 0f;
            // arm.GetComponent<Aiming>().lockmove = true;
            clapper.SetActive(false);
            isPaused = true;
        }
    }

    public void Resume()
    {
        // game is resumed
        gameManager.Paused = false;

        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        //tutorialCanvas.SetActive(true);
        clapper.SetActive(true);
        isPaused = false;
    }

    public void Restart()
    {
        // restarts the current scene
        Resume();
        PlayerPrefs.SetInt("Tutorial", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        // loads the menu scene
        SceneManager.LoadScene(backToMenu);
    }

    public void Quit()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}