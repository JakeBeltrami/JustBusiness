using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class ButtonSounds : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    // Added by LS 21.05.19 - Allows sounds to be played when the menu buttons are pressed or hovered...
    // This script is attatched to the camera in menu, game over and win scene

    public AudioClip buttonhover;
    public AudioClip buttonPress;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // When mouse hovers over button play sound
        audioSource.clip = buttonhover;
        audioSource.Play();

    }

    public void OnSelect(BaseEventData eventData)
    {
        // On mouse click on button play sound
        audioSource.clip = buttonPress;
        audioSource.Play();
    }
}