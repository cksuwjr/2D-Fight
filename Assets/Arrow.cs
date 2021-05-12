using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float Damage;
    float ShootingSpeed;
    int direction;

    Rigidbody2D rb;
    Player_Act My_Act;
    public void WhoShootMe(Player_Act itsme, float Damage, float ShootingSpeed, int direction)
    {
        My_Act = itsme;
        this.Damage = Damage;
        this.ShootingSpeed = ShootingSpeed;
        this.direction = direction;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3.5f);

    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(direction, 0) * ShootingSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && collision.gameObject != My_Act.gameObject)
        {
            collision.GetComponent<Status>().GetDamage(Damage, "AD");
            Destroy(gameObject);
        }
    }
}
