using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public Image pistolUI;
    public List<Image> ammoUI;
    private int bulletCount;
    private PlayerInventory playerInv;
    private Dictionary<string, Sprite> UIMap;

    void Start()
    {
        playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        UIMap = new Dictionary<string, Sprite>
        {
            { "No Weapon", Resources.Load<Sprite>("UI/Pistol_Icon_Outline_UI") },
            { "Weapon", Resources.Load<Sprite>("UI/Pistol_Icon_UI") },
            { "No Ammo", Resources.Load<Sprite>("UI/Bullet_Outline_UI") },
            { "Ammo", Resources.Load<Sprite>("UI/Bullet_UI") }
        };
        bulletCount = ammoUI.Count;
    }

    // Probably doesn't need to be called every frame but meh
    void Update()
    {
        // If the player has a weapon (Gun)
        if (playerInv.weapon != null)
        {
            // Show the pistol fill sprite
            pistolUI.sprite = UIMap["Weapon"];

            // Show the ammmo
            for (int i = 0; i < bulletCount; i++)
            {
                ammoUI[i].sprite = i < playerInv.weapon.Ammo ? UIMap["Ammo"] : UIMap["No Ammo"];
            }
        }
        else
        {
            // Show the pistol outline sprite
            pistolUI.sprite = UIMap["No Weapon"];

            // Show no bullets
            foreach (Image bullet in ammoUI)
            {
                bullet.sprite = UIMap["No Ammo"];
            }
        }
    }
}
