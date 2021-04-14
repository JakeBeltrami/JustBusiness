using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(POI))]
public class LOSEditor : Editor
{
    // public void OnSceneGUI()
    // {
    //     POI poi = (POI)target;
    //     Handles.color = Color.red;
    //     foreach (GameObject g in poi.validPoints)
    //     {
    //         Handles.DrawLine(poi.player.transform.position, g.transform.parent.transform.position);
    //     }
    // }
}
