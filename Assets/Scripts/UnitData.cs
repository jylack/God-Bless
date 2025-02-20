using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    public GameObject obj;//���� ������Ʈ ������ ���

    public UnitType unitType;//�������� �������� ������
    public List<SkillType> skills;
    public List<Vector3> patrolPoints;//���� ��ǥ
    
    public float chaseRange = 10f; //�߰� ����
    public float chaseSpeed = 5f; //�߰� �ӵ�
    public float patrolSpeed = 3.5f;//���� �ӵ�

    //�������ͽ�
    public int level;
    public int maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;
    public float speed;
    public bool isGate;
}
