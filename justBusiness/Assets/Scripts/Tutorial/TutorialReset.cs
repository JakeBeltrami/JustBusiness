using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialReset : MonoBehaviour
{
	public bool tutReset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(tutReset)
		{
			PlayerPrefs.SetInt("Tutorial",1);
		}
    }
}
