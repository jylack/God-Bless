using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Game Data/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public float damageMultiplier;
    public float cooldown;
    public float effectDuration;
}
