using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fists : MonoBehaviour
{
	public Rigidbody2D fist;
 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

	}

	public void Attack(Vector2 shootPosition, Vector2 direction) 
	{
		//Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(mousePosition);
		//Vector2 shootPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		//Vector2 direction = cursorPosition - shootPosition;
		Rigidbody2D fistProjectile = Instantiate (fist, shootPosition, Quaternion.identity) as Rigidbody2D;
		fistProjectile.velocity = direction;
        StartCoroutine(WaitSeconds());
	}

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(2000f);
    }

}
