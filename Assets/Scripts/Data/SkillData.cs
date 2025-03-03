using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game Data/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;       // 스킬 이름
    public float damageMultiplier; // 데미지 배율
    public float cooldown;         // 스킬 쿨타임
    public float effectDuration;   // 효과 지속 시간 (방어력 증가, 힐링 등)
    public bool isAreaEffect;      // 범위 공격 여부
}
