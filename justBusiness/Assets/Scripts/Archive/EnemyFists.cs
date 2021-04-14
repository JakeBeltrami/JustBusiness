using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFists : MonoBehaviour
{
    public Rigidbody2D fist;

    public float attackRate;

    public float currentCooldown = 0;

    private SpriteRenderer enemySprite;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = attackRate;
        enemySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown -= Time.deltaTime;
    }

    public void Attack(Vector2 shootPosition, Vector2 direction)
    {
        if (currentCooldown <= 0)
        {
            currentCooldown = attackRate;
            //Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            //Vector2 shootPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            //Vector2 direction = cursorPosition - shootPosition;
            Rigidbody2D fistProjectile = Instantiate(fist, shootPosition, Quaternion.identity) as Rigidbody2D;
            fistProjectile.velocity = direction * 10;
            DisplayDashSprite(direction.normalized);
        }

    }

    private void DisplayDashSprite(Vector2 direction)
    {
        string prefix = "Enemy/Dash/Regular_Grunt_Dash_";
        float x = direction.x;
        float y = direction.y;

        // if x is positive
        if (x > 0)
        {
            if (y > 0)
            {
                if (x >= y)
                {
                    prefix += 'L';
                }
                else
                {
                    prefix += 'B';
                }
            }
            else
            {
                if (x >= -y)
                {
                    prefix += 'L';
                }
                else
                {
                    prefix += 'F';
                }
            }
        }
        // if x is negative
        if (x < 0)
        {
            if (y > 0)
            {
                if (-x >= y)
                {
                    prefix += 'R';
                }
                else
                {
                    prefix += 'B';
                }
            }
            else
            {
                if (x >= y)
                {
                    prefix += 'R';
                }
                else
                {
                    prefix += 'F';
                }
            }
        }

        enemySprite.sprite = Resources.Load<Sprite>(prefix);
    }

}
