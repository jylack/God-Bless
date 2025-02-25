using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [Header("유닛 기본 스탯")]
    public float MaxHP;
    public float HP;
    public float Atk;
    public float MAtk;
    public float Def;

    [Header("부여 스킬(스킬칸)")]
    public GrantSkill skill;

    [Header("게이트 내부에 있는지 여부")]
    public bool inGate = false;

    // 도시로 이동
    public void CityMove()
    {
        // TODO: NavMeshAgent 등을 사용해 도시 위치로 이동 로직
        Debug.Log($"{gameObject.name}이(가) 도시로 이동");
    }

    // 필드로 이동
    public void FieldMove()
    {
        // TODO: NavMeshAgent 등을 사용해 필드(몬스터가 있는 곳)로 이동 로직
        Debug.Log($"{gameObject.name}이(가) 필드로 이동");
    }

    // 게이트 입장
    public void InGate()
    {
        inGate = true;
        Debug.Log($"{gameObject.name}이(가) 게이트에 입장");
    }

    // 유닛이 스킬을 직접 사용하고 싶을 때
    public void UseSkill()
    {
        if (skill != null)
        {
            skill.ActivateSkill();
        }
    }

    // 전투/피격 처리 등
    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            Debug.Log($"{gameObject.name} 사망");
            // TODO: 유닛 제거 로직 (GOD 매니저에서 listUnit에서 제거 등)
        }
    }
}
