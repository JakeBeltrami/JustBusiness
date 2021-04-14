using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinToCredits : MonoBehaviour
{
    public float secondsBeforeLoadingCredits = 3f;
    public string winFromCredits = "Credits";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("DelayedAction", secondsBeforeLoadingCredits); 
    }

    public void DelayedAction()
    {
        SceneManager.LoadScene(winFromCredits);
    }

   
}
