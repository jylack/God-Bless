using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [Header("���� �⺻ ����")]
    public float MaxHP;
    public float HP;
    public float Atk;
    public float MAtk;
    public float Def;

    [Header("�ο� ��ų(��ųĭ)")]
    public GrantSkill skill;

    [Header("����Ʈ ���ο� �ִ��� ����")]
    public bool inGate = false;

    // ���÷� �̵�
    public void CityMove()
    {
        // TODO: NavMeshAgent ���� ����� ���� ��ġ�� �̵� ����
        Debug.Log($"{gameObject.name}��(��) ���÷� �̵�");
    }

    // �ʵ�� �̵�
    public void FieldMove()
    {
        // TODO: NavMeshAgent ���� ����� �ʵ�(���Ͱ� �ִ� ��)�� �̵� ����
        Debug.Log($"{gameObject.name}��(��) �ʵ�� �̵�");
    }

    // ����Ʈ ����
    public void InGate()
    {
        inGate = true;
        Debug.Log($"{gameObject.name}��(��) ����Ʈ�� ����");
    }

    // ������ ��ų�� ���� ����ϰ� ���� ��
    public void UseSkill()
    {
        if (skill != null)
        {
            skill.ActivateSkill();
        }
    }

    // ����/�ǰ� ó�� ��
    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            Debug.Log($"{gameObject.name} ���");
            // TODO: ���� ���� ���� (GOD �Ŵ������� listUnit���� ���� ��)
        }
    }
}
