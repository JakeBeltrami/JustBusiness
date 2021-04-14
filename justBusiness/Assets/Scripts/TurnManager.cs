using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    private GameManager gameManager;
    private float timer;
    private float check;
    private int enemyCount;
    private bool playerActed = false;
    public float inputTime;
    private List<GameObject> snap;
    private GameObject pSnap;

    private Player player;
    private List<GameObject> enemies;
    private POI poi;
    private GameObject soundManager;
    private ShakeTest shakeTest;

    public enum State
    {
        Frozen,
        PlayerTurn,
        EnemyTurn,
        Waiting
    }

    private State state;

    public State GameState { get { return state; } set { state = value; } }
    public int EnemyCount { get { return enemyCount; } set { enemyCount = value; } }
    public bool PlayerActed { get { return playerActed; } set { playerActed = value; } }

    void Start()
    {
        //Can also be set in frozen time script
        state = State.Frozen;
        timer = 0;
        check = 0;
        enemyCount = 0;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = gameManager.Player;
        enemies = gameManager.Enemies;
        poi = gameManager.GetComponent<POI>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager");
        shakeTest = GameObject.FindGameObjectWithTag("Camera").GetComponent<ShakeTest>();

        //snap = new List<GameObject>();
        //snap.Add(GameObject.FindGameObjectWithTag("Snap").transform.GetChild(0).gameObject);
        //snap.Add(GameObject.FindGameObjectWithTag("Snap").transform.GetChild(1).gameObject);
        //snap.Add(GameObject.FindGameObjectWithTag("Snap").transform.GetChild(2).gameObject);

        // pSnap = player.transform.Find("pSnap").gameObject;
        // pSnap.GetComponentInChildren<MeshRenderer>().sortingOrder = 10;
    }

    void Update()
    {
        if (!gameManager.Planning)
        {
            // Debug.Log(state);
            switch (state)
            {
                case State.Frozen:
                    // Show POIs
                    // poi.ValidPOI(player.GetComponent<PlayerInput>().dashRange);
                    gameManager.Post.EnableVignette();
                    // check = timer;
                    timer += (Time.unscaledDeltaTime * 1f);
                    soundManager.GetComponent<SoundManager>().timerSound(timer);
                    player.Snap.StartAnimation();

                    // if (timer > inputTime / 3 - 0.5555556 && check < inputTime / 3 - 0.5555556)
                    // {
                    //     pSnap.GetComponent<Animator>().SetBool("Playing", true);
                    //     // snap[2].GetComponent<Animator>().SetTrigger("Start");
                    // }
                    // if (timer > inputTime / 3 * 2 - 0.5555556 && check < inputTime / 3 * 2 - 0.5555556)
                    // {
                    //     pSnap.GetComponent<Animator>().SetBool("Playing", true);
                    //     // snap[1].GetComponent<Animator>().SetTrigger("Start");
                    // }
                    // if (timer > inputTime - 0.5555556 && check < inputTime - 0.5555556)
                    // {
                    //     pSnap.GetComponent<Animator>().SetBool("Playing", true);
                    //     // snap[0].GetComponent<Animator>().SetTrigger("Start");
                    // }

                    if (timer < inputTime)
                    {
                        // Debug.Log(playerActed);
                        if (playerActed)
                        {
                            playerActed = false;
                            state = State.PlayerTurn;
                            // Debug.Log("<color=green>PLAYER TURN SET</color>");
                            timer = 0;
                        }
                    }
                    else
                    {
                        // Player did nothing
                        player.PlayerInput.Cover.SeekCover(player.Tile.Center);
                        if (player.IsInCover)
                        {
                            player.PlayerInput.Cover.EnterCover();
                        }
                        state = State.EnemyTurn;
                        timer = 0;
                    }
                    break;

                case State.PlayerTurn:
                    player.Snap.StopAnimation();
                    gameManager.Post.DisableVignette();
                    // Disable POIs
                    poi.DisableAllPOI();
                    //foreach (GameObject s in snap)
                    //{
                    //    s.SetActive(false);
                    //}
                    StartCoroutine(player.GetComponent<Player>().TakeTurn());
                    // End turn

                    state = State.Waiting;
                    break;

                case State.EnemyTurn:
                    player.Snap.StopAnimation();
                    gameManager.Post.DisableVignette();
                    // Disable POIs
                    poi.DisableAllPOI();
                    StartCoroutine(EnemyTurns());
                    state = State.Waiting;
                    break;

                case State.Waiting:
                    if (enemyCount >= enemies.Count && !shakeTest.Shaking)
                    {
                        enemyCount = 0;
                        state = State.Frozen;
                    }
                    break;
            }
        }
    }

    private IEnumerator EnemyTurns()
    {
        if ((enemies.Count != 0))
        {
            foreach (GameObject e in enemies)
            {
                if (e.GetComponent<Enemy>() != null)
                {
                    StartCoroutine(e.GetComponent<Enemy>().TakeTurn());
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    public void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.Post.DisableVignette();
        }
        if (player != null)
        {
            player.Snap.StopAnimation();
            // If the above doesn't work, disable or destroy when levelWon?
        }
    }
}
