﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    Animator anim;
    Player_Act My_Act;
    GameObject Arrow;
    int BowDirection;
    void Awake()
    {
        anim = GetComponent<Animator>();
        My_Act = GetComponentInParent<Player_Act>();
        gameObject.SetActive(false);
        Arrow = Resources.Load<GameObject>("Weapon/Arrow/Arrow") as GameObject;
    }
    public void ChangeWeaponSeeing(int n)
    {
        transform.localScale = new Vector2(-n, 1);
        BowDirection = n;
    }
    void ShootArrow()
    {
        My_Act.SetStop(true);
        // Instantiate
        GameObject spawnedArrow;
        spawnedArrow = Instantiate(Arrow, My_Act.transform.position, Quaternion.identity);
        spawnedArrow.GetComponent<Arrow>().WhoShootMe(My_Act, My_Act.status.AttackDamage, 14, BowDirection);
    } 
    public void Shoot()
    {
        anim.SetTrigger("Shoot");
    }
}
