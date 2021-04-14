using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluePOI : MonoBehaviour
{
    Vector3 size;
    UIPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        size = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<UIPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("POI/POI Active");
        transform.localScale = size * 1.15f;
    }

    void OnMouseExit()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("POI/POI");
        transform.localScale = size;
    }

    void OnMouseDown()
    {
        player.Move(transform.position);
    }
}
