using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollow : MonoBehaviour
{
    private GameObject player;
    private RectTransform rt;
    private RectTransform canvasRT;
    private Vector3 roboScreenPos;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3).gameObject;

        rt = GetComponent<RectTransform>();
        canvasRT = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        // roboScreenPos = Camera.main.WorldToViewportPoint(player.transform.TransformPoint(player.transform.position));
        // rt.anchorMax = roboScreenPos;
        // rt.anchorMin = roboScreenPos;
        // Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position);
        // rt.anchoredPosition = screenPoint - canvasRT.sizeDelta / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position);
        // roboScreenPos = Camera.main.WorldToViewportPoint(player.transform.TransformPoint(player.transform.position));
        // rt.anchorMax = roboScreenPos;
        // rt.anchorMin = roboScreenPos;
        // rt.anchoredPosition = screenPoint - canvasRT.sizeDelta / 2f;
        rt.anchoredPosition = (Vector2)player.transform.position - canvasRT.sizeDelta / 2f;
    }



}
