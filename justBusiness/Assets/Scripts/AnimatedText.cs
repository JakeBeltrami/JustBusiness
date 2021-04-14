using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Made 21.05 LS - Purpose: To have animated text on Menu Screen
public class AnimatedText : MonoBehaviour
{
    //The time for each letter to appear - The lower it is, the faster letters appear
    private float letterPauseRate;
    //Message that will be displayed
    public string messageText;
    //Text for the message to display
    public Text assignedText;

    void Awake()
    {
        //Get text component
        assignedText = GetComponent<Text>();
    }

    void Start()
    {
        //Get text component
        assignedText = GetComponent<Text>();
        Time.timeScale = 1;
        letterPauseRate = 0.01f;
        //messageText will display assignedText
        messageText = assignedText.text;
        //Calls the function
        StartCoroutine(TypeText());
    }

    public IEnumerator TypeText()
    {
        //Sets the assignedText to be blank
        assignedText.text = "";

        //Split each char into a char array
        foreach (char letter in messageText.ToCharArray())
        {
            //Add 1 letter each time
            assignedText.text += letter;
            yield return 0;
            // Waits for seconds assigned in letterPauseRate (eg. 0.5f)
            yield return new WaitForSeconds(letterPauseRate); // Multiply by Time.deltaTime?
        }
    }

    public void NextText()
    {
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }
}