using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInText : MonoBehaviour
{
    float blinkDurationSecs = 0.5f;
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
        blinkingText.color = Color.Lerp(Color.grey, Color.white, blinkProgress);// or whatever color you choose
    }
}