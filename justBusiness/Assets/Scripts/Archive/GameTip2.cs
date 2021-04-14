
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTip2 : MonoBehaviour
{

    [Header("RANDOM HINTS")]
    // Reference to the Text
    public Text hintsText;
    // List of strings - User can change the size of this list and add in game tips in inspector
    public List<string> HintList = new List<string>();
    public bool changeHintWithTimer;
    // How long each tip lasts for... Can change this in inspector
    public float hintTimerValue = 3;
    // [Range(0.1f, 1)] public float hintFadingSpeed = 1;
    private float Timer;
    public bool enableRandomHints = true;
    private bool isHintAlphaZero;

    // Start is called before the first frame update
    void Start()
    {
        if (enableRandomHints == true)
        {
            string hintChose = HintList[Random.Range(0, HintList.Count)];
            hintsText.text = hintChose;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enableRandomHints == true)
        {
            Timer += Time.deltaTime;

            if (Timer >= hintTimerValue && isHintAlphaZero == false)
            {
                string hintChose = HintList[Random.Range(0, HintList.Count)];
                hintsText.text = hintChose;
                Timer = 0.0f;
            }
        }
    }
}
