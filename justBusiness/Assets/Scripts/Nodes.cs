using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Nodes : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject n in nodes)
        {
            Gizmos.DrawWireSphere(n.transform.position, 0.25f);
        }

        // Added by W.T. (26-09-2019)
        // To support CoverNodes
        GameObject[] coverNodes = GameObject.FindGameObjectsWithTag("CoverNode");

        Gizmos.color = Color.green;
        foreach (GameObject c in coverNodes)
        {
            Gizmos.DrawCube(c.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
        }
    }
}
