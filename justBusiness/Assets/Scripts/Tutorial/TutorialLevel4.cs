using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialLevel4 : MonoBehaviour
{
    // Audio Sounds:
    AudioSource audioSource;
    public AudioClip tutorialSound;

    // Makes the game wait before popping up tutorial
    private float timeLeft = 3f;
    private int textBoxCount;
    private int counter = 0;

    // bools
    private bool UIpoppedup = false;

    // Camera Sprite
    public GameObject cameraSprite;
    // General textbox on bottom of screen
    public GameObject UItextbox;
    // Black UI Overlay;
    public GameObject blackUI;
    // Triangle
    public GameObject triangle;
    // Textbox containing string of instructions
    public GameObject textBox;
    // Tutorial text strings
    public string[] text;
    // Clapper 
    public GameObject clapper;

    // Extras?
    public GameObject realEnemy1;
    public GameObject realEnemy2;
    public GameObject fakeEnemy1;
    public GameObject fakeEnemy2;
    public GameObject playerCrouch;
    public GameObject sfx;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Tutorial") != 0)
        {
            // Disable all text/ images
            textBoxCount = text.Length;
            DisableAll();
            audioSource = GetComponent<AudioSource>();
            // Extras
            fakeEnemy1.SetActive(false);
            fakeEnemy2.SetActive(false);
            playerCrouch.SetActive(false);
            sfx.SetActive(false);
            // Start Coroutine
            StartCoroutine(InitialiseTut());
            // Clapper visibility
            clapper.SetActive(false);
        }
    }

    public void DisableAll()
    {
        cameraSprite.SetActive(false);
        UItextbox.SetActive(false);
        textBox.SetActive(false);
        blackUI.SetActive(false);
        triangle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || (PlayerPrefs.GetInt("Tutorial") == 0))
        {
            SkipTutorial();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && UIpoppedup == true)
        {
            if (counter == text.Length)
            {
                DisableAll();
                clapper.SetActive(true);
                audioSource.Stop();
            }
            else if (counter == 1)
            {
                // Spawn the player crouch sprite
                playerCrouch.SetActive(true);
                // Enable the SFX
                sfx.SetActive(true);
                NextTextBox(text[counter]);
            }
            else if (counter == 2)
            {
                // Disable the real enemy sprites
                realEnemy1.SetActive(false);
                realEnemy2.SetActive(false);
                // Enable the fake enemy sprites
                fakeEnemy1.SetActive(true);
                fakeEnemy2.SetActive(true);
                // Next box
                NextTextBox(text[counter]);
            }
            else if (counter == 3)
            {
                // Disable the fake enemies
                fakeEnemy1.SetActive(false);
                fakeEnemy2.SetActive(false);
                // Despawn the player crouch sprite
                playerCrouch.SetActive(false);
                // Enable the real enemies again
                realEnemy1.SetActive(true);
                realEnemy1.GetComponent<Animator>().SetBool("outlined", false);
                realEnemy1.GetComponent<Enemy>().SetInitialDirection();
                realEnemy2.SetActive(true);
                realEnemy2.GetComponent<Animator>().SetBool("outlined", false);
                realEnemy2.GetComponent<Enemy>().SetInitialDirection();
                // Next box
                NextTextBox(text[counter]);
            }
            else
            {
                if (counter < textBoxCount)
                {
                    NextTextBox(text[counter]);
                }
            }
        }
    }

    private IEnumerator InitialiseTut(float countdownValue = 4)
    {
        timeLeft = countdownValue;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timeLeft--;

            if (timeLeft < 3)
            {
                // Enable first tutorial speech
                audioSource.clip = tutorialSound;
                audioSource.Play();
                cameraSprite.SetActive(true);
                UItextbox.SetActive(true);
                blackUI.SetActive(true);
                triangle.SetActive(true);
                yield return new WaitForSeconds(0.85f);
                break;
            }
        }
        textBox.SetActive(true);
        UIpoppedup = true;
        NextTextBox(text[counter]);
    }

    private void SkipTutorial()
    {
        // Enable whatever you need here. Do this for all tutorial scripts
        realEnemy1.SetActive(true);
        realEnemy2.SetActive(true);
        clapper.SetActive(true);
        enabled = false;
        gameObject.SetActive(false);
    }

    private void NextTextBox(string text)
    {
        textBox.GetComponent<Text>().text = text;
        textBox.GetComponent<AnimatedText>().messageText = text;
        textBox.GetComponent<AnimatedText>().NextText();
        counter++;
    }
}

