using UnityEngine;
public enum ItemType
{
    Weapon, // ����
    Armor   // ��
}

public enum WeaponType
{
    Sword,  // ��
    Bow,    // Ȱ
    Staff   // ������
}

public enum ArmorType
{
    Head,   // �Ӹ�
    Body,   // ��
    Legs,   // �ٸ�
    Hands   // ��
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
