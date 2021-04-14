using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                // Check if it has a Toggleable component
                if (result.gameObject.tag == "Enemy")
                {
                    if (result.gameObject.GetComponent<Image>().sprite == Resources.Load<Sprite>("ControlsScreen/Grunt_D"))
                    {
                        result.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("ControlsScreen/Grunt_U");
                        result.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("ControlsScreen/Grunt_D");
                    }
                    else
                    {
                        result.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("ControlsScreen/Grunt_D");
                        result.gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("ControlsScreen/Grunt_U");
                    }
                }
                else if (result.gameObject.tag == "Environment")
                {
                    Vector2 oriPos = result.gameObject.transform.position;
                    result.gameObject.transform.position = result.gameObject.transform.GetChild(0).gameObject.transform.position;
                    result.gameObject.transform.GetChild(0).gameObject.transform.position = oriPos;
                }
                else if (result.gameObject.tag == "altPos")
                {
                    Vector2 oriPos = result.gameObject.transform.parent.gameObject.transform.position;
                    result.gameObject.transform.parent.gameObject.transform.position = result.gameObject.transform.position;
                    result.gameObject.transform.position = oriPos;
                }
            }
        }
    }
}
