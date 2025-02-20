using UnityEngine;

public enum SkillType { Magic, Melee, Ranged, Healing }

[CreateAssetMenu(fileName = "NewSkill", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public GameObject obj;//��ų ������Ʈ

    public SkillType type;//��ųŸ��
    public string skillName;//��ų�̸�
    public int level; //��ų ��ø�� �������� ����. ���������� ���� ����ġ�� �޶�����

    public float coolTime;//��ų ��Ÿ��.
    public float delay;//��ų ������
    public float activeTime;//��ų ���ӽð�
    public Vector3 targetPos;//Ÿ�� ��ġ����.
    

    //�Ʒ��� ���� ����ġ �󸶳� �ٰųĿ� ���� ����
    public int maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;
    public float speed;
}
