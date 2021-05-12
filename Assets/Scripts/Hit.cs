using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    Player_Act My_Act;
    GameObject Enemy;

    public bool IsTouched;
    public bool IsDamagable;
    void Awake()
    {
        IsDamagable = false;
        My_Act = GetComponentInParent<Player_Act>();
    }

    void Update()
    {
        transform.localPosition = new Vector2(0.5f * My_Act.SeeingDirection, 0);
        if (IsTouched && IsDamagable && Enemy != null)
        {
            Enemy.GetComponent<Status>().GetDamage(My_Act.status.AttackDamage, "AD");
            IsDamagable = false;
        }
    }
    public void ChangeWeaponSeeing(int n)
    {
        transform.localPosition = new Vector2(0.5f * n, 0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if ( (collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("Monster") )&& collision.gameObject != My_Act.gameObject)
        {
            Enemy = collision.gameObject;
            IsTouched = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("Monster")) && collision.gameObject != My_Act.gameObject)
        {
            Enemy = null;
            IsTouched = false;
        }
    }
}
