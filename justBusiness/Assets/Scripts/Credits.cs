using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class Credits : MonoBehaviour

{

    public SceneFader sceneFader;
    public string levelToLoad = "MenuEdit";
    private float Timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InvokeRepeating("CheckTimer", 55f, 1f);
    }

    void CheckTimer()
    {
        Timer += 1f;
        if (Timer == 1f)
        {
            CancelInvoke("CheckTimer");
            Play();
        }
    }

    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }
}
