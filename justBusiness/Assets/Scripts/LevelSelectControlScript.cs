using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectControlScript : MonoBehaviour
{

    public Button blevel2, blevel3, blevel4, blevel5, blevel6;
    int levelPassed;

    // Start is called before the first frame update
    void Start()
    {
        levelPassed = PlayerPrefs.GetInt("Level Passed");
        blevel2.interactable = false;
        blevel3.interactable = false;
        blevel4.interactable = false;
        blevel5.interactable = false;
        blevel6.interactable = false;

        switch (levelPassed)
        {
            case 1:
                blevel2.interactable = true;
                break;
            case 2:
                blevel2.interactable = true;
                blevel3.interactable = true;
                break;
            case 3:
                blevel2.interactable = true;
                blevel3.interactable = true;
                blevel4.interactable = true;
                break;
            case 4:
                blevel2.interactable = true;
                blevel3.interactable = true;
                blevel4.interactable = true;
                blevel5.interactable = true;
                break;
            case 5:
                blevel2.interactable = true;
                blevel3.interactable = true;
                blevel4.interactable = true;
                blevel5.interactable = true;
                blevel6.interactable = true;
                break;
        }
    }

    public void levelToLoad (int level)
    {
        // Int is based on order in the build
        SceneManager.LoadScene(level);
    }

    public void resetPlayerPrefs()
    {
        blevel2.interactable = false;
        blevel3.interactable = false;
        blevel4.interactable = false;
        blevel5.interactable = false;
        blevel6.interactable = false;
        PlayerPrefs.DeleteAll();
    }
}
