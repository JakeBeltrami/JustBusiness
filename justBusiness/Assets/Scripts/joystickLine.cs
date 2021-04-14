using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickLine : MonoBehaviour
{


	public float speed = 5f;

	private Vector3 movement;
	private float shortestDistance = Mathf.Infinity; 
	private GameObject[] points;
	private Vector3 posPOI;

	[Tooltip("Value to limit the length of ray, could be different for each scene")]
	public float x1, x2, y1, y2;

    // Update is called once per frame
    void Update()
    {
		//Raycasting();
		Movement();
		NearestTarget();
    }

	private void NearestTarget()
	{
		points = GameObject.FindGameObjectsWithTag("POI Target");
		for(int i=0;i<points.Length;i++)
		{
			float distanceToTarget = Vector3.Distance(transform.position, points[i].transform.parent.position);
			if(distanceToTarget<shortestDistance)
			{
				shortestDistance=distanceToTarget;
				posPOI = points[i].transform.parent.position;
			}
		}
		shortestDistance = Mathf.Infinity;

		if(movement==new Vector3(0,0,0))
		{
			this.transform.position = posPOI;			
		}

	}

	private void Movement()
	{
		movement = new Vector3(Input.GetAxis("LeftJoystickX"), Input.GetAxis("LeftJoystickY"), 0f);
		//movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		transform.position += movement * speed * Time.deltaTime;
	
		//Need a way to limit length of ray
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, x1, x2),Mathf.Clamp(transform.position.y, y1, y2),0);
	}


}
