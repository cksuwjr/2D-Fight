using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerContact : MonoBehaviour
{
    float ContactStayTime;
    GameObject Enemy;
    void Update()
    {
        if (ContactStayTime > 0)
            ContactStayTime -=  Time.deltaTime;
        else if (ContactStayTime < 0)
            ContactStayTime = 0;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            ContactStayTime = 5f;
            Enemy = collision.gameObject;
            Debug.Log("닿았어!!");

        }
    }
    public float GetFollowTime()
    {
        return ContactStayTime;
    }
    public GameObject WhoisEnemy()
    {
        return Enemy;
    }
}
