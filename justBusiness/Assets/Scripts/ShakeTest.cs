using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ShakeTest : MonoBehaviour
{
    public float magnitude, roughness, fadeInTime, fadeOutTime;
    public float seconds;

    private CameraShakeInstance shake;
    private bool isShaking = false;

    private TurnManager turnManager;

    public bool Shaking { get { return isShaking; } }

    public void Awake()
    {
        turnManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnManager>();
    }

    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Tab))
    //     {
    //         StartCoroutine(ShakeForSeconds());
    //     }
    // }

    /*public IEnumerator ShakeForSeconds(float seconds)
    {
        if (!isShaking)
        { 
        	isShaking = true;
        	shake = CameraShaker.Instance.StartShake(magnitude, roughness, fadeInTime);
        	yield return new WaitForSecondsRealtime(seconds);
        	shake.StartFadeOut(fadeOutTime);
			gameManager.GetComponent<TurnManager>().EnemyCount++;
        	isShaking = false;
        }
    }*/

    public IEnumerator ShakeForSeconds()
    {
        if (!isShaking)
        {
            isShaking = true;
            shake = CameraShaker.Instance.StartShake(magnitude, roughness, fadeInTime);
            yield return new WaitForSeconds(seconds); // Affected by Slow Down
            shake.StartFadeOut(fadeOutTime);
            isShaking = false;
        }
    }

    public IEnumerator ShakeForSeconds(float magnitude, float roughness, float fadeInTime, float fadeOutTime, float seconds, bool increment = true)
    {
        if (!isShaking)
        {
            isShaking = true;
            shake = CameraShaker.Instance.StartShake(magnitude, roughness, fadeInTime);
            yield return new WaitForSeconds(seconds); // Affected by Slow Down
            shake.StartFadeOut(fadeOutTime);
            isShaking = false;
        }
    }
}
