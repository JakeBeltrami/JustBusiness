using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MonoBehaviour
{

    // Edited by Trent, added currentWaitTime in order to preserve waitTime
    public float waitTime;
    private float currentWaitTime;

    [HideInInspector]
    public bool isOccupied = false;

    // Property for access
    public float CurrentWaitTime { get { return currentWaitTime; } }

    void Start()
    {

    }

    void Update()
    {
        if (isOccupied == true)
        {
            currentWaitTime -= Time.deltaTime;
            Debug.Log("wait time " + waitTime);
        }
        else
        {
            currentWaitTime = waitTime;
        }
    }
}
