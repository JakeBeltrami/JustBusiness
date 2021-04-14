using UnityEngine;
using System.Collections;

/*
======================================================================

	Purpose:
		Shakes camera and stun player if got hit

	Parameters:
		None

	Dependencies:
		Player.cs
		
	Author:
		Kshitij Choudhary

	Changelog:
        05-05-2019  Default strength increased from 0.7f to 5f

	Notes:

======================================================================
*/
public class MicroCameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    private Vector3 camPos;

    // How long the object should shake for.
    public float shakeDuration = 1f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 5f;
    public float decreaseFactor = 1.0f;

    public float timer = 0f;
    // duration for which player is immune to camera shake
    public float shakeImmunity = 3f;

    public bool shakeTrue = false;

    public GameObject redImage;

    float originalShakeDuration;

    private Player player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
            camPos = new Vector3(camTransform.position.x, camTransform.position.y, camTransform.position.z);

        }
        redImage = GameObject.Find("EnergyBar/Canvas/Red");
    }

    void OnEnable()
    {
        originalShakeDuration = shakeDuration;
    }

    public void Update()
    {
        if (shakeTrue)
        {
            timer += Time.deltaTime;
            if (shakeDuration > 0)
            {
                GetComponent<CameraMovement>().enabled = false;
                //camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, originalPos + Random.insideUnitSphere * shakeAmount, Time.deltaTime*3);

                camTransform.localPosition = Vector3.Lerp(camTransform.position, camTransform.position + Random.insideUnitSphere * shakeAmount, Time.deltaTime * 3);

                shakeDuration -= Time.deltaTime * decreaseFactor;
                // player.InputLock = true;
            }
            else
            {
                camTransform.localPosition = Vector3.Lerp(camPos, camPos, Time.deltaTime * 3);
                //shakeDuration = originalShakeDuration;
                // player.InputLock = false;
                GetComponent<CameraMovement>().enabled = true;
                redImage.SetActive(false);

                //shakeTrue = false;
            }

            if (timer > shakeImmunity)
            {
                shakeDuration = originalShakeDuration;
                shakeTrue = false;
                timer = 0;
            }

        }

    }

    /*public IEnumerator shakecamera()
    {
		yield return StartCoroutine(CanShake());
		yield return StartCoroutine(ShakeImmune());
    }*/
}