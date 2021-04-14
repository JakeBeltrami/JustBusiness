using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetPrompt : MonoBehaviour
{
    // Camera Sprite
    private GameObject cameraSprite;
    // General textbox on bottom of screen
    private GameObject UItextbox;
    // Black UI Overlay;
    private GameObject blackUI;
    // Textbox containing string of instructions
    private GameObject textBox;
    // Text prompt
    public string text;
    private bool UIpoppedup = false;

    void Start()
    {
        cameraSprite = transform.GetChild(1).gameObject;
        UItextbox = transform.GetChild(0).gameObject;
        blackUI = transform.GetChild(2).gameObject;
        textBox = transform.GetChild(4).gameObject;
        cameraSprite.SetActive(false);
        UItextbox.SetActive(false);
        textBox.SetActive(false);
        blackUI.SetActive(false);
    }

    public IEnumerator Prompt(float timer = 4, float delay = 1f)
    {
        yield return new WaitForSeconds(delay);
        while (timer > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timer--;

            if (timer < 3)
            {
                // Enable first tutorial speech
                // audioSource.clip = tutorialSound;
                // audioSource.Play();
                cameraSprite.SetActive(true);
                UItextbox.SetActive(true);
                blackUI.SetActive(true);
            }
        }
        textBox.SetActive(true);
        // UIpoppedup = true;

        // Show prompt
        textBox.GetComponent<Text>().text = text;
        textBox.GetComponent<AnimatedText>().messageText = text;
        textBox.GetComponent<AnimatedText>().NextText();
    }
}
