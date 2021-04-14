using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInTextLevelSelect : MonoBehaviour
{
    private float blinkDurationSecs = 1f;
    private float blinkProgress = 0f;
    private float blinkStep = 0.015f;

    private Text blinkingText;
    private Color origColour;
    private Color brown;

    void Start()
    {
        blinkingText = GetComponentInParent<Text>();
        origColour = blinkingText.color;
        brown = new Color(0.1226415F, 0.1226415F, 0.1226415F);
    }

    // Update is called once per frame
    void Update()
    {
        if ((blinkProgress > 1) || (blinkProgress < 0))
        {
            blinkStep *= -1f;
        }
        blinkProgress += blinkStep;
        blinkingText.color = Color.Lerp(origColour, brown, blinkProgress);// or whatever color you choose
    }
}