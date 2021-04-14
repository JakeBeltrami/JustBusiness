using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInTextWin : MonoBehaviour
{
    float blinkDurationSecs = 1.0f;
    float blinkProgress = 0f;
    public float blinkStep = 0.025f;
    //Color txtColor = Color.black;
    Text blinkingText;
    // Use this for initialization
    void Start()
    {
        blinkingText = GetComponentInParent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((blinkProgress > 1) || (blinkProgress < 0))
        {
            blinkStep *= -1f;
        }
        blinkProgress += blinkStep;
        blinkingText.color = Color.Lerp(Color.blue, Color.cyan, blinkProgress);// or whatever color you choose
    }
}