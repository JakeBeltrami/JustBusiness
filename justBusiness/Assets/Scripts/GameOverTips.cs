using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameOverTips : MonoBehaviour
{
    public string[] gameOverTips =
        {
            "TIP 1",
            "TIP 2",
            "TIP 3",
            "TIP 4",
            "TIP 5",
            "TIP 6",
            "TIP 7",
            "TIP 8",
            "TIP 9",
            "TIP 10"
        };

    public uint timeToNextHint = 6;
    public Text hintText;
    public Text restartInstruction;
    public AudioClip nextHintSFX;
    public AudioClip countdown;

    [HideInInspector]
    public bool isGameOver;

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private VideoSource videoSource;
    private string hintToDisplay;
    private float currentTime;
    private int count = 0;
    private bool playerDead;
    private bool cycleSound = true;
    private bool restartCalled = false;
    private GameObject canvasUI;
    private GameObject canvasChild;

    // Added by LS extra functionality
    public GameObject mainBackground;
    public Image backgroundGameTip;
    public Image blackBackground;
    public float animationWait = 2f;
    private bool hasPlayed = false;


    // Start is called before the first frame update
    public void Init()
    {
        currentTime = timeToNextHint;
        Shuffle();
        hintToDisplay = gameOverTips[0];
        hintText.text = hintToDisplay;
        hintText.enabled = false;
        restartInstruction.enabled = false;

        GetComponent<AudioSource>().playOnAwake = false;
        canvasChild = gameObject.transform.GetChild(0).gameObject;
        canvasUI = GameObject.Find("Game UI");
        playerDead = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().playerDead;
        audioSource = GetComponent<AudioSource>();
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.targetCamera = Camera.main;
        videoPlayer.Prepare();
        isGameOver = false;

        // To stop the main music from playing:
        AudioSource mainMusic = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();

        /*
        for (int i = 0; i < gameOverTips.Length; i++)
        {
            Debug.Log(gameOverTips[i]);
        }
        */

        // Added LS Panels

        mainBackground.SetActive(false);
        // Images
        backgroundGameTip.enabled = false;
        blackBackground.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerDead = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().playerDead;
        if (playerDead && !restartCalled)
        {
            //Debug.Log("He's dead Jim");
            //Debug.Log(currentTime -= Time.deltaTime);
            isGameOver = true;
            currentTime -= Time.deltaTime;
            Invoke("DelayedAction", animationWait);


            if (currentTime <= 0 || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && cycleSound == true)
            {
                if (currentTime >= 0)
                {
                    audioSource.clip = nextHintSFX;
                    audioSource.Play();
                }
                currentTime = timeToNextHint;
                NextHint();
                currentTime = timeToNextHint;
            }

            if (currentTime <= 0 || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && cycleSound == true)
            {
                if (currentTime >= 0)
                {
                    audioSource.clip = nextHintSFX;
                    audioSource.Play();
                }
                NextHint(-1);
                currentTime = timeToNextHint;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                restartCalled = true;
                //Debug.Log("RESTART CALLED");
                cycleSound = false;

                videoPlayer.Play();
                // audioSource.clip = countdown;
                //audioSource.Play();
                StartCoroutine(Wait());

                //Debug.Log("VIDEO AND AUDIO HAVE PLAYED");
                StartCoroutine(WaitAndLoad(videoPlayer));
            }


        }

    }

    void LoadScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator WaitAndLoad(VideoPlayer vp)
    {
        yield return new WaitForSeconds((float)vp.length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene("TUT3");
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds((float)0.2);
        canvasUI.SetActive(false);
        canvasChild.SetActive(false);

        // added LS
        mainBackground.SetActive(false);
        // Images
        backgroundGameTip.enabled = false;
        blackBackground.enabled = false;

        hintText.enabled = false;
        restartInstruction.enabled = false;
    }

    void Shuffle()
    {
        for (int i = 0; i < gameOverTips.Length; i++)
        {
            string temp = gameOverTips[i];
            int j = Random.Range(i, gameOverTips.Length);
            gameOverTips[i] = gameOverTips[j];
            gameOverTips[j] = temp;

        }

    }

    public void DelayedAction()
    {
        //Debug.Log("Waiting for Death Animation");
        AudioSource mainMusic = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();


        // Enables projector game over sound
        //knockedOut.GetComponent<AudioSource>().clip = projectorSound;\

        //knockedOut.GetComponent<AudioSource>().Play();

        hintText.enabled = true;
        restartInstruction.enabled = true;

        // Added LS

        mainBackground.SetActive(true);
        backgroundGameTip.enabled = true;
        blackBackground.enabled = true;
        mainMusic.GetComponent<AudioSource>().Stop();
        //knockedOut.GetComponent<AudioSource>().Play();
    }




    void NextHint(int multi = 1)
    {
        count = (count + 1 * multi) % (gameOverTips.Length - 1);

        if (count < 0)
        {
            count = (gameOverTips.Length - 1);
        }

        hintToDisplay = gameOverTips[count];
        hintText.text = hintToDisplay;
    }

}
