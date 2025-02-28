using UnityEngine;
public enum ItemType
{
    Weapon, // 무기
    Armor   // 방어구
}

public enum WeaponType
{
    Sword,  // 검
    Bow,    // 활
    Staff   // 지팡이
}

public enum ArmorType
{
    Head,   // 머리
    Body,   // 몸
    Legs,   // 다리
    Hands   // 손
}


[CreateAssetMenu(fileName = "NewItemData", menuName = "Game Data/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;

    [Header("Weapon Stats")]
    public WeaponType weaponType;
    public int attackPower;

    [Header("Armor Stats")]
    public ArmorType armorType;
    public int defensePower;
}
