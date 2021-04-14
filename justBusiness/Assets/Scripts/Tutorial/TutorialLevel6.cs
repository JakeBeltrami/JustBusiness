using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialLevel6 : MonoBehaviour
{
    // Audio Sounds:
    AudioSource audioSource;
    public AudioClip tutorialSound;
    public AudioClip reloadSound;
    public AudioClip heavySound;

    // Makes the game wait before popping up tutorial
    private float timeLeft = 3f;
    private int textBoxCount;
    private int counter = 0;

    // bools
    private bool UIpoppedup = false;
    private bool pressedGun = false;

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
    public GameObject ammoReload;
    public GameObject enemy;
    public GameObject sfx;

    // Start is called before the first frame update
    void Start()
    {
        // Added check because it interfered with enemy toggle display
        if (PlayerPrefs.GetInt("Tutorial") != 0)
        {
            // Disable all text/ images
            textBoxCount = text.Length;
            DisableAll();
            audioSource = GetComponent<AudioSource>();
            enemy.SetActive(false);
            ammoReload.SetActive(false);
            StartCoroutine(InitialiseTut());
            // Clapper visibility
            clapper.SetActive(false);
            sfx.SetActive(false);
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
            Debug.Log("Skipping");
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
                enemy.SetActive(true);
                enemy.GetComponent<Enemy>().SetInitialDirection();
                audioSource.PlayOneShot(heavySound);
                NextTextBox(text[counter]);
            }

            else if (counter == 2)
            {
                ammoReload.SetActive(true);
                sfx.SetActive(true);
                audioSource.PlayOneShot(reloadSound);
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

    private void SkipTutorial()
    {
        // Enable whatever you need here. Do this for all tutorial scripts
        clapper.SetActive(true);
        ammoReload.SetActive(true);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().SetInitialDirection();
        ammoReload.SetActive(true);
        gameObject.SetActive(false);
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

    private void NextTextBox(string text)
    {
        textBox.GetComponent<Text>().text = text;
        textBox.GetComponent<AnimatedText>().messageText = text;
        textBox.GetComponent<AnimatedText>().NextText();
        counter++;
    }
}

