using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Clapper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, ISelectHandler
{
    public Button clapperButton;
    public Sprite clapperOpen;
    public Sprite clapperHalf;
    public Sprite clapperClose;
    public Image actionText;
    Image myImageComponent;

    AudioSource audioSource;
    public AudioClip clapperHover;
    public AudioClip clapperPress;

    private float timeLeft;
    private bool clapperClicked = false;
    public GameObject filmEffect;
    // SFX
    public GameObject sfx;

    Vector3 oriSize;

    // Start is called before the first frame update
    void Start()
    {
        myImageComponent = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        actionText.enabled = false;
        sfx.SetActive(false);
        oriSize = transform.localScale;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Working");
        if (clapperClicked == false)
        {
            transform.localScale = new Vector3(oriSize.x, oriSize.y * 1.077f, oriSize.z);
            myImageComponent.sprite = clapperHalf;
            actionText.enabled = true;
            // Audio for when clapper is hovered
            audioSource.clip = clapperHover;
            audioSource.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clapperClicked == false)
        {
            transform.localScale = oriSize;
            myImageComponent.sprite = clapperOpen;
            actionText.enabled = false;
        }
    }

    //public void OnSelect(BaseEventData eventData)
    //{
    //myImageComponent.sprite = clapperClose;
    //actionText.enabled = false;
    //}

    public void WhenClicked()
    {
        transform.localScale = new Vector3(oriSize.x, oriSize.y * 0.872f, oriSize.z);
        myImageComponent.sprite = clapperClose;
        actionText.enabled = false;

        clapperClicked = true;
        // Audio for when clapper is pressed;
        audioSource.clip = clapperPress;
        audioSource.Play();
        filmEffect.SetActive(false);
        sfx.SetActive(true);
        StartCoroutine(WaitTime());
        // Also for Tutorial Level 1, it should only pop up after the last message
    }

    private IEnumerator WaitTime(float countdownValue = 2)
    {
        // Remove all Clapper sprites/ components (maybe wait a certain time before it goes away, 
        // otherwise we cannot see close sprite)
        timeLeft = countdownValue;
        while (timeLeft > 0)
        {
            // yield return new WaitForSeconds(1.0f);
            yield return new WaitForSeconds(0.1f);
            timeLeft--;

            if (timeLeft < 1)
            {
                clapperButton.enabled = false;
                actionText.enabled = false;
                myImageComponent.enabled = false;
                // Start the Action Phase
                GameObject.FindGameObjectWithTag("GameController").GetComponent<PlanningPhase>().EndPlanning();
            }
        }
    }
}
