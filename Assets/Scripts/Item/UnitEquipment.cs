using UnityEngine;

public class UnitEquipment : MonoBehaviour
{
    public ItemData equippedWeapon; // ����
    public ItemData equippedHead;   // �Ӹ� ��
    public ItemData equippedBody;   // �� ��
    public ItemData equippedLegs;   // �ٸ� ��
    public ItemData equippedHands;  // �� ��

    public void EquipItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("�������� �����ϴ�!");
            return;
        }

        switch (item.itemType)
        {
            case ItemType.Weapon:
                equippedWeapon = item;
                Debug.Log($"���� ����: {item.itemName} (���ݷ�: {item.attackPower})");
                break;

            case ItemType.Armor:
                switch (item.armorType)
                {
                    case ArmorType.Head:
                        equippedHead = item;
                        Debug.Log($"�Ӹ� �� ����: {item.itemName} (����: {item.defensePower})");
                        break;
                    case ArmorType.Body:
                        equippedBody = item;
                        Debug.Log($"�� �� ����: {item.itemName} (����: {item.defensePower})");
                        break;
                    case ArmorType.Legs:
                        equippedLegs = item;
                        Debug.Log($"�ٸ� �� ����: {item.itemName} (����: {item.defensePower})");
                        break;
                    case ArmorType.Hands:
                        equippedHands = item;
                        Debug.Log($"�� �� ����: {item.itemName} (����: {item.defensePower})");
                        break;
                }
                break;
        }
    }


    public void UnequipItem(ItemType type, ArmorType? armorType = null)
    {
        switch (type)
        {
            case ItemType.Weapon:
                equippedWeapon = null;
                Debug.Log("Unequipped Weapon.");
                break;

            case ItemType.Armor:
                if (armorType.HasValue)
                {
                    switch (armorType.Value)
                    {
                        case ArmorType.Head: equippedHead = null; Debug.Log("Unequipped Head armor."); break;
                        case ArmorType.Body: equippedBody = null; Debug.Log("Unequipped Body armor."); break;
                        case ArmorType.Legs: equippedLegs = null; Debug.Log("Unequipped Leg armor."); break;
                        case ArmorType.Hands: equippedHands = null; Debug.Log("Unequipped Hand armor."); break;
                    }
                }
                break;
        }
    }
}

