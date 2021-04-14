using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public string levelToLoad = "TL1";
    public SceneFader sceneFader;
    public VideoPlayer videoPlayer;
    private VideoSource videoSource;

    // Reference to text so we can disable them once the countdown has begun
    public Text pressAnyButton;
    public Text gameTip;
    public GameObject image1;
    public GameObject image2;
    // Also include Panels
    public GameObject panel;
    public GameObject gameTipPanel;
    // To ensure player cannot continue pressing any button to start video again
    private bool pressedAlready = false;

    private float Timer;

    public AudioClip countdown;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Class that lets us change scenes using the Unity editor
    public void LoadScene(string sceneName)
    {
        //check that scene name has been assigned
        if (sceneName == null)
            Debug.Log("<color=orange>" + gameObject.name + ": No Scene Name Was given for LoadScene function!</color>");
        //load the scene linked in editor
        SceneManager.LoadScene(sceneName); //load a scene
    }

    void Update()
    {
        if (Input.anyKey && !pressedAlready)
        {
            pressAnyButton.enabled = false;
            gameTip.enabled = false;
            image1.SetActive(false);
            image2.SetActive(false);
            panel.SetActive(false);
            gameTipPanel.SetActive(false);
            sceneFader.Fade();
            videoPlayer.Play();
            audioSource.PlayOneShot(countdown);
            // maybe insert sound here too
            //Timer += Time.deltaTime;
            InvokeRepeating("CheckTimer", 1f, 1f);
            pressedAlready = true;
            
        }
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

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Play()
    {
        sceneFader.FadeTo(levelToLoad);
    }
}
