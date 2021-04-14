//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class Melee : Weapon
//{
//    public MeleeSwing punch;
//    float currentCooldown = 0;
//    // Start is called before the first frame update
//    void Start()
//    {
//        
//    }
//
//	public override void Fire(Transform pos)
//    {
//        currentCooldown -= Time.deltaTime;
//
//
//        if (currentCooldown <= 0)
//        {
//            currentCooldown = fireRate;
//            Instantiate(punch, transform.position + new Vector3(0.5f, 0, 0), transform.rotation);
//
//        }
//
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//        
//    }
//}
