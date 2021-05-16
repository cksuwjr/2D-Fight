using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float AD;
    float ShootingSpeed;
    int direction;

    float Damage;

    Rigidbody2D rb;

    Player_Act My_Act;
    Animator Bow_anim;
    bool OnCharging;

    float HoldedTime; // 홀드 지속 시간 (최대 2초)  ==>  투사체속도 및 데미지 최대 0.5(기본) + 2.0배(홀드)까지 증가.

    public void OnCharge(Player_Act itsme, float AD, float ShootingSpeed, int direction, Animator anim)
    {
        My_Act = itsme;
        this.AD = AD;
        this.ShootingSpeed = ShootingSpeed;
        this.direction = direction;

        Bow_anim = anim;

        OnCharging = true;

        transform.localScale = new Vector2(-direction, 1);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        OnCharging_Manage();
        HoldTimer();
    }
    void HoldTimer()
    {
        if (OnCharging)
            HoldedTime += Time.deltaTime;

        if (HoldedTime > 2)
            HoldedTime = 2f;

    }
    void OnCharging_Manage()
    {
        if (OnCharging)
        {
            transform.localPosition = My_Act.transform.localPosition;
            if (!Input.GetKey(KeyCode.Q))
            {
                OnCharging = false;
                Damage += (AD * (0.5f + HoldedTime));
                ShootingSpeed *= (0.5f + HoldedTime * 0.5f);


                Destroy(gameObject, 0.5f + HoldedTime * 0.5f); // 화살 지속시간 0.5 + (0 ~ 2)(홀드) 초
                My_Act.SetStop(true);
                Bow_anim.SetTrigger("EndCharging");
                My_Act.EndCharging();
            }
        }
        else
            rb.velocity = new Vector2(direction, 0) * ShootingSpeed;

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!OnCharging)
            if ((collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("Monster")) && collision.gameObject != My_Act.gameObject)
            {
                collision.GetComponent<Status>().GetDamage(Damage, "AD");
                Destroy(gameObject);
            }
    }
}
