using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
    public bool startTimer = false;
    public float slowTimeDuration = 1f;
    // Added by Jake, slowSpeed is how fast the game runs in slowmotion
    public float slowSpeed = 0.25f;
    // Changed by Jake, timeDecel is how fast the game speed decelerates to the slowSpeed (Currently removed to make time slow work better)
    // public float timeDecel = 0.3f;
    public float timeScaletest = 0;

    private float timer = 0;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeScaletest = Time.timeScale;

        if (startTimer)
        {
            timer += Time.unscaledDeltaTime;
        }

        if (timer >= slowTimeDuration)
        {
            startTimer = false;
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
            timer = 0;
        }
    }

    // Added by Jake, unlocks the player's movement once time is slowed
    // Call this function when you want to slow time
    public void SlowTime()
    {
        Time.timeScale = slowSpeed;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        startTimer = true;
    }

    public void ResumeTime()
    {
        if (startTimer)
        {
            timer = slowTimeDuration;
        }
    }
}
