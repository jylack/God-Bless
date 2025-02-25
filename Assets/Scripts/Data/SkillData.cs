using UnityEngine;

public enum SkillType { Magic, Melee, Ranged, Healing }

[CreateAssetMenu(fileName = "NewSkill", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    [Header("스킬 모델")]
    public GameObject obj;//스킬 오브젝트

    [Header("스킬 설정")]
    public SkillType type;//스킬타입
    public string skillName;//스킬이름
    public int level; //스킬 중첩시 레벨업할 예정. 레벨업마다 스텟 가중치가 달라진다
    public float coolTime;//스킬 쿨타임.
    public float delay;//스킬 딜레이
    public float activeTime;//스킬 지속시간

    [Header("스테이터스 가중치")]
    public int maxHp;
    public int atk;
    public int magicAtk;
    public int def;
    public int magicDef;
    public float speed;

    [Header("인게임 내 설정용")]
    public Vector3 startPos;//본인 위치정보
    public Vector3 targetPos;//타겟 위치정보.
}
