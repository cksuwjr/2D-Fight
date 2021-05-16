using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artificial_Intelligent : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    //CapsuleCollider2D cc;
    GameObject Weapon;
    GameObject AfterImage;

    int currentWeaponID;

    public Status status;       // 플레이어 스탯(체력,공격력,방어력 등)

    Image HP;


    public float Move_Speed;    // 스피드
    public int SeeingDirection;      // 바라보는 방향   -1 / +1

    Vector2 basicOffset;

    bool isJumping;
    bool isGround;

    float Jump_Overlap_ban_Timer; // 점프 중복 방지 시간. 점프 후 일정 시간동안 점프 감지 X
    float Stop_Timer;           // 멈칫: 시간이 지나면 멈칫 풀림, 외부에서 설정가능[SetStop(float)](public, ex 속박)
    bool Stop_Act;           // 멈칫: 특정 조건 만족시 멈칫 풀림, 외부에서 설정가능[SetStop(true/false)]


    int AI_Move_Pattern;
    float AI_Act_Time;

    void Awake()
    {
        // 필요한 컴포넌트 불러오기
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //cc = GetComponent<CapsuleCollider2D>();

        anim = GetComponent<Animator>();
        status = GetComponent<Status>();
        HP = transform.GetChild(1).GetChild(2).GetComponent<Image>();

        AfterImage = transform.GetChild(2).gameObject;

        // 기본 설정
        //basicOffset = new Vector2(cc.offset.x, cc.offset.y);

        Move_Speed = 5f;


        // 게임 설정

        Stop_Act = true;  // 행동가능 true

        SeeingDirection = -1;  // 바라보는 방향


        // 인공지능 행동 패턴, 지속시간
        AI_Move_Pattern = 0;
        AI_Act_Time = 0; // 움직임 지속시간
    }

    void Update()
    {
        HP.fillAmount = (status.current_HP * (100 / status.Max_HP)) / 100;  // 체력바 이미지 변경
        CoolTimer();


        if (AI_Act_Time == 0)
        {
            AI_Move_Pattern = Random.Range(1, 5);
            SetAct();
            Debug.Log("행동설정됨!" + AI_Act_Time + "초간 " + AI_Move_Pattern + "패턴!!");
        }

        if (Stop_Timer == 0 && Stop_Act && AI_Act_Time > 0)
        {
            if (AI_Move_Pattern == 1)
                Move("오");
            else if (AI_Move_Pattern == 2)
                Move("왼");
            else if (AI_Move_Pattern == 3)
                Move("None");
            else if (AI_Move_Pattern == 4)
                Move("Jump");
        }
        
        // Act();
        // 행동정지 없이 가능
        // No_ristrict_Act();

    }
    void Move(string direction)
    {
        if (direction == "왼")
        {
            sr.flipX = false;
            anim.SetBool("isWalk", true);
            anim.SetBool("isIdle", false);
            ChangeWeaponDirection(-1);
            SeeingDirection = -1;
            rb.velocity = new Vector2(-1 * Move_Speed, rb.velocity.y);
        }
        else if (direction == "오")
        {
            sr.flipX = true;
            anim.SetBool("isWalk", true);
            anim.SetBool("isIdle", false);
            ChangeWeaponDirection(1);
            SeeingDirection = 1;
            rb.velocity = new Vector2(1 * Move_Speed, rb.velocity.y);
        }
        else if(direction == "None")
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isIdle", true);
        }
        else if (direction == "Jump")
            if (!isJumping && isGround && Jump_Overlap_ban_Timer == 0)  // 점프시 
            {
                Jump_Overlap_ban_Timer = 0.5f;          // 점프 중복 방지 시간 설정
                //cc.offset = new Vector2(cc.offset.x, -cc.offset.y);  // 점프시 플레이어 충돌 범위 조정
                Jump();
                SetAct(0);
            }




        if (isJumping && isGround && Jump_Overlap_ban_Timer == 0)   // 점프 이후 땅에 착지시
        {
            //cc.offset = basicOffset;                    // 플레이어 충돌 범위 원상복구
            NotJump();
        }

    }

    void Act()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Attack(0); // 기본공격
        else if (Input.GetKeyDown(KeyCode.Q))
            Attack(1); // 활공격
    }
    void No_ristrict_Act()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isGround)
                BackStep();
        }
    }
    void ChangeWeaponDirection(int n)
    {
        if (Weapon != null)
            switch (currentWeaponID)
            {
                case 0: // 기본공격 방향 전환
                    Weapon.GetComponent<Hit>().ChangeWeaponSeeing(n);
                    break;
                case 1: // 활 방향 전환
                    Weapon.GetComponent<Bow>().ChangeWeaponSeeing(n);
                    break;
            }
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

    void BackStep()
    {
        SetStop(0.45f);
        AfterImage.SetActive(true);
        rb.velocity = new Vector2(0, rb.velocity.y);

        AfterImage.GetComponent<ParticleSystem>().gameObject.transform.localScale = new Vector2(-2 * SeeingDirection, 2); // 잔상 크기 방향 조절

        if (SeeingDirection == 1)
            rb.AddForce(new Vector2(-400, 0));
        else if (SeeingDirection == -1)
            rb.AddForce(new Vector2(400, 0));


    }

    void Attack(int what)
    {
        if (what != 0 && Weapon != null)
            Weapon.SetActive(false);

        Weapon = transform.GetChild(0).GetChild(what).gameObject;
        currentWeaponID = what;
        switch (what)
        {
            case 0: // 기본공격
                Weapon.GetComponent<Hit>().ChangeWeaponSeeing(SeeingDirection);
                anim.SetTrigger("Attack");
                SetStop(0.3f);
                //anim.Play("Char_Attack");
                break;
            case 1:  // 활공격
                Weapon.GetComponent<Bow>().ChangeWeaponSeeing(SeeingDirection);
                Weapon.SetActive(true);
                anim.SetTrigger("Shoot");
                SetStop(false);
                Weapon.GetComponent<Bow>().Charging();
                break;
        }

        //case 1:   케이스에 따라 WeaponCollider변경.

    }
    public void EndCharging()
    {
        anim.SetTrigger("EndCharging");
    }

    void WeaponColliderOn()
    {
        Weapon.GetComponent<Hit>().IsDamagable = true;
    }
    void WeaponColliderOff()
    {
        Weapon.GetComponent<Hit>().IsDamagable = false;
    }

    // 캐릭터 행동 제한
    public void SetStop(float second) // 초 단위    ex) CC적중시, 고정시간만큼의 멈칫이 필요할때.
    {
        Stop_Timer = second;
        //if (isGround)
        //    rb.velocity = new Vector2(0, rb.velocity.y);
    }
    public void SetStop(bool stop)  // 직접 제한    ex) 본인 스킬 사용시.
    {
        Stop_Act = stop;
        //if (isGround)
        //    rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void SetAct()
    {
        AI_Act_Time = Random.Range(1, 4);
    }
    void SetAct(float second)
    {
        AI_Act_Time = second;
    }

    void CoolTimer()
    {
        // 점프 중복방지 시간 타이머
        if (Jump_Overlap_ban_Timer > 0)
            Jump_Overlap_ban_Timer -= Time.deltaTime;

        if (Jump_Overlap_ban_Timer < 0)
            Jump_Overlap_ban_Timer = 0;

        // 멈칫 타이머
        if (Stop_Timer > 0)
            Stop_Timer -= Time.deltaTime;

        if (Stop_Timer < 0)
        {
            Stop_Timer = 0;
            if (AfterImage.activeSelf)   // 잔상효과 있을경우 끄기     // 
                AfterImage.SetActive(false);
        }

        // AI 행동 지속시간 타이머

        if (AI_Act_Time > 0)
            AI_Act_Time -= Time.deltaTime;

        if(AI_Act_Time < 0)
            AI_Act_Time = 0;

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
