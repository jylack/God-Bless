using UnityEngine;

public enum SkillType { Magic, Melee, Ranged, Healing }

[CreateAssetMenu(fileName = "NewSkill", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    [Header("��ų ��")]
    public GameObject obj;//��ų ������Ʈ

    [Header("��ų ����")]
    public SkillType type;//��ųŸ��
    public string skillName;//��ų�̸�
    public int level; //��ų ��ø�� �������� ����. ���������� ���� ����ġ�� �޶�����
    public float coolTime;//��ų ��Ÿ��.
    public float delay;//��ų ������
    public float activeTime;//��ų ���ӽð�

    [Header("�������ͽ� ����ġ")]
    public int maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;
    public float speed;

    [Header("�ΰ��� �� ������")]
    public Vector3 startPos;//���� ��ġ����
    public Vector3 targetPos;//Ÿ�� ��ġ����.
}
