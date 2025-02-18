using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public int maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;
    public List<string> skills;
    public List<Vector3> patrolPoints;//���� ��ǥ
    public float chaseRange = 10f; //�߰� ����
    public float chaseSpeed = 5f; //�߰� �ӵ�
    public float patrolSpeed = 3.5f;//���� �ӵ�
}
