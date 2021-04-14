using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedPOI : MonoBehaviour
{
    Vector3 size;
    UIPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        size = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<UIPlayer>();
    }

    private void OnMouseEnter()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("POI/Shoot POI Active");
        player.SwapImage();
        transform.localScale = size * 1.15f;
    }

    void OnMouseOver()
    {

    }

    void OnMouseExit()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("POI/Shoot POI Inactive");
        player.SwapImage();
        transform.localScale = size;
    }

    void OnMouseDown()
    {
        player.Shoot(transform.position);
    }
}
