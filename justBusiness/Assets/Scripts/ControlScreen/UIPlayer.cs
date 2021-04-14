using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    private Vector2 oriPos;
    private Vector3 size;
    private Vector2 bulletSpawn;
    [HideInInspector]
    public Image playerImage;
    public Image aimingImage;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        oriPos = transform.position;
        size = transform.localScale;
        playerImage = GetComponent<Image>();
        aimingImage.enabled = false;
        bulletSpawn = bullet.transform.position;
        bullet.SetActive(false);
    }

    public void Move(Vector2 target)
    {
        StopAllCoroutines();
        transform.position = oriPos;
        playerImage.sprite = Resources.Load<Sprite>("ControlsScreen/Dash");
        transform.localScale = new Vector3(size.x * 2.08f, size.y * 0.87f, size.z);
        StartCoroutine(Moving(target));
    }

    private IEnumerator Moving(Vector2 target)
    {
        Vector2 tarPos = target;
        float totalDist = Vector2.Distance(transform.position, target);
        float ratio;
        float speed = 10;

        while ((Vector2)transform.position != tarPos)
        {
            float distance = Vector2.Distance(transform.position, tarPos);

            if (distance / totalDist > 0.8)
            {
                // Ease in
                ratio = distance / totalDist + 0.1f;
            }
            else if (distance / totalDist > 0.3)
            {
                // Max speed
                ratio = 1;
            }
            else
            {
                // Ease out
                ratio = distance / totalDist + 0.3f;
            }

            transform.position = Vector2.MoveTowards(transform.position, tarPos, ratio * speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        playerImage.sprite = Resources.Load<Sprite>("ControlsScreen/Idle");
        transform.localScale = size;

        yield return new WaitForSeconds(1);

        transform.position = oriPos;
    }

    public void Shoot(Vector2 target)
    {
        StopAllCoroutines();
        // Reset the bullet position
        bullet.transform.position = bulletSpawn;
        StartCoroutine(Shooting(target));
    }

    private IEnumerator Shooting(Vector2 target)
    {
        float totalDist = Vector2.Distance(bullet.transform.position, target);
        float speed = 15;
        // Show bullet
        bullet.SetActive(true);

        while ((Vector2)bullet.transform.position != target)
        {
            bullet.transform.position = Vector2.MoveTowards(bullet.transform.position, target, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // Reset bullet position and disable
        bullet.transform.position = bulletSpawn;
        bullet.SetActive(false);

        yield return new WaitForSeconds(1);
    }

    public void SwapImage()
    {
        playerImage.enabled = !playerImage.enabled;
        aimingImage.enabled = !aimingImage.enabled;
    }
}
