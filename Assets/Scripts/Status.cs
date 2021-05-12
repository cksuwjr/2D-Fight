using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Status : MonoBehaviour
{
    public int Level { get; set; }      // 레벨
    public float Max_HP { get; set; }   // 최대체력
    public float current_HP { get; set; }   // 현재체력
    public float AttackDamage { get; set; } // AD
    public float AttackPower { get; set; }  // AP
    public float AD_Defense { get; set; }   // AD 방어
    public float AP_Defense { get; set; }   // AP 방어

    void Start()
    {
        // 캐릭터 스탯 설정
        Level = 1;

        Max_HP = 400 + 125 * Level;     // 기본 체력

        AttackDamage = 60;               // 기본 공격력
        AttackPower = 0;                // 기본 스킬 추가 공격력

        AD_Defense = 30;                // 기본 방어력
        AP_Defense = 30;                // 기본 스킬 방어력

        current_HP = Max_HP;

    }
    public void LevelUP()      // 레벨업시
    {
        Level++;

        Max_HP = 400 + 125 * Level;         // 최대 체력 증가
        current_HP += 125;                  // 현재 체력 회복

        AttackDamage += 7;                  // 기본 공격력 증가

        AD_Defense += 3;                // 기본 방어력 증가
        AP_Defense += 3;                // 기본 스킬 방어력 증가
    }

    public void GetDamage(float Damage, string DamageType)     // 데미지 입음
    {
        Debug.Log(ApplyDefenseOnDamage(Damage, DamageType) + "의 피해를 입음!");
        current_HP -= ApplyDefenseOnDamage(Damage, DamageType);
    }

    float ApplyDefenseOnDamage(float Damage, string DamageType)      // 데미지 방어력 적용 계산식
    {
        if (DamageType == "AD")
            return Damage - (Damage * (AD_Defense / (100 + AD_Defense)));
        else if (DamageType == "AP")
            return Damage - (Damage * (AP_Defense / (100 + AP_Defense)));
        else if (DamageType == "True")
            return Damage;
        else
            return -1; // 오류
    }
}
