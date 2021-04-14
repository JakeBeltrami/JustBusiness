using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Snap : MonoBehaviour
{
    private int count;

    private Animator animator;
    private TextMeshPro textMesh;
    private SpriteRenderer spriteRenderer;
    private MeshRenderer mesh;

    AudioSource audioSource;
    public AudioClip clickingSound;

    void Start()
    {
        count = 3;

        animator = GetComponent<Animator>();
        textMesh = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mesh = GetComponentInChildren<MeshRenderer>();
        mesh.sortingOrder = 10;
        audioSource = GetComponent<AudioSource>();
    }

    public void StartAnimation()
    {
        animator.SetBool("Playing", true);
        ShowSnap();
    }

    public void StopAnimation()
    {
        animator.SetBool("Playing", false);
        HideSnap();
    }

    public void HideSnap()
    {
        spriteRenderer.enabled = false;
        DisableText();
    }

    public void ShowSnap()
    {
        spriteRenderer.enabled = true;
    }

    public void EnableText()
    {
        mesh.enabled = true;
    }

    public void DisableText()
    {
        mesh.enabled = false;
    }

    public void AnimationFinished()
    {
        count--;
        Debug.Log(count);
        if (count == 0)
        {
            count = 3;
            gameObject.SetActive(false);
        }
        else
        {
            textMesh.text = count.ToString();
            animator.SetBool("Playing", false);
        }
    }

    public void SnapSound()
    {
        audioSource.PlayOneShot(clickingSound);
    }

    public void ShowCount(string count)
    {
        textMesh.text = count;
    }
}
