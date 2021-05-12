using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class None_Actor : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    CapsuleCollider2D cc;

    GameObject WeaponCollider;

    public Status status;       // 플레이어 스탯(체력,공격력,방어력 등)

    Image HP;


    public float Move_Speed;    // 스피드
    public int SeeingDirection;      // 바라보는 방향   -1 / +1

    Vector2 basicOffset;

    bool isJumping;
    bool isGround;

    float Jump_Overlap_ban_Timer; // 점프 중복 방지 시간. 점프 후 일정 시간동안 점프 감지 X
    float Attack_Timer;           // 공격 시간. 공격 후 일정시간동안만 피격시 데미지 인정.
    void Awake()
    {
        // 필요한 컴포넌트 불러오기
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CapsuleCollider2D>();

        anim = GetComponent<Animator>();
        status = GetComponent<Status>();
        HP = gameObject.transform.GetChild(1).transform.GetChild(2).gameObject.GetComponent<Image>();

        WeaponCollider = gameObject.transform.GetChild(0).gameObject;

        // 기본 설정
        basicOffset = new Vector2(cc.offset.x, cc.offset.y);

        Move_Speed = 5f;
    }

    void Update()
    {
        HP.fillAmount = (status.current_HP * (100 / status.Max_HP)) / 100;  // 체력바 이미지 변경
        //Move();
        CoolTimer();
    }
    void Move()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * Move_Speed, rb.velocity.y);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            sr.flipX = false;
            anim.SetBool("isWalk", true);
            anim.SetBool("isIdle", false);
            SeeingDirection = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            sr.flipX = true;
            anim.SetBool("isWalk", true);
            anim.SetBool("isIdle", false);
            SeeingDirection = 1;
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isIdle", true);
        }


        if (Input.GetKey(KeyCode.UpArrow))
            if (!isJumping && isGround && Jump_Overlap_ban_Timer == 0)  // 점프시 
            {
                Jump_Overlap_ban_Timer = 0.5f;          // 점프 중복 방지 시간 설정
                cc.offset = new Vector2(cc.offset.x, -cc.offset.y);  // 점프시 플레이어 충돌 범위 조정
                Jump();

            }

        if (isJumping && isGround && Jump_Overlap_ban_Timer == 0)   // 점프 이후 땅에 착지시
        {
            cc.offset = basicOffset;                    // 플레이어 충돌 범위 원상복구
            NotJump();
        }


        if (Input.GetKeyDown(KeyCode.Space))
            Attack();

    }

    void Jump()
    {
        isJumping = true;
        anim.SetBool("isJump", true);
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 10);
    }
    void NotJump()
    {
        isJumping = false;
        anim.SetBool("isJump", false);
    }


    void Attack()
    {
        anim.SetTrigger("Attack");
        //anim.Play("Char_Attack");
    }
    void WeaponColliderOn()
    {
        WeaponCollider.GetComponent<Hit>().IsDamagable = true;
    }
    void WeaponColliderOff()
    {
        WeaponCollider.GetComponent<Hit>().IsDamagable = false;
    }



    void CoolTimer()
    {
        if (Jump_Overlap_ban_Timer > 0)
            Jump_Overlap_ban_Timer -= Time.deltaTime;

        if (Jump_Overlap_ban_Timer < 0.1f)
            Jump_Overlap_ban_Timer = 0;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
            isGround = true;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
            isGround = false;
    }
    
}
