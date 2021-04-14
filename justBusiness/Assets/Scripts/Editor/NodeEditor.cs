using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Nodes))]
public class NodeEditor : Editor
{
    // public void OnSceneGUI()
    // {
    //     Nodes nodes = (Nodes)target;
    //     Handles.color = Color.cyan;
    //     GameObject[] gs = GameObject.FindGameObjectsWithTag("Node");
    //     foreach (GameObject n in gs)
    //     {
    //         Handles.DrawWireArc(n.transform.position, Vector3.forward, Vector3.right, 360, 0.25f);
    //     }
    // }
}
