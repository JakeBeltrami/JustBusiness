using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{

    public int totalLevel = 0;
    public int unlockedLevel = 1;

    private LevelButton[] levelButtons;
    private int totalPage = 0;
    private int page = 0;
    private int pageItem = 3; //How many levels are inside the page

    public GameObject nextButton;
    public GameObject backButton;

    private void OnEnable()
    {
        levelButtons = GetComponentsInChildren<LevelButton>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void StartLevel(int level)
    {
        // show player a level info or start the level
        if(level==unlockedLevel)
        {
            unlockedLevel+=1;
            Refresh();
        }
    }

    public void ClickNext()
    {
        page += 1;
        Refresh();
    }

    public void ClickBack()
    {
        page -= 1;
        Refresh();
    }

    public void Refresh()
    {
        totalPage = totalLevel / pageItem;
        int index = page * pageItem;

        for(int i = 0; i<levelButtons.Length;i++)
        {
            int level = index + i + 1;

            if (level<=totalLevel)
            {
                levelButtons[i].gameObject.SetActive(true);
                levelButtons[i].Setup(level, level <= unlockedLevel);
            }
            else
                levelButtons[i].gameObject.SetActive(false);
        }

        CheckButton();
    }

    private void CheckButton()
    {
        backButton.SetActive(page > 0);
        nextButton.SetActive(page < totalPage);
    }
 
}
