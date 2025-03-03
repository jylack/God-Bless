using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game Data/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;       // ��ų �̸�
    public float damageMultiplier; // ������ ����
    public float cooldown;         // ��ų ��Ÿ��
    public float effectDuration;   // ȿ�� ���� �ð� (���� ����, ���� ��)
    public bool isAreaEffect;      // ���� ���� ����
}
