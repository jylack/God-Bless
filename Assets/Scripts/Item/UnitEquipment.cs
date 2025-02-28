using UnityEngine;

public class UnitEquipment : MonoBehaviour
{
    public ItemData equippedWeapon; // 무기
    public ItemData equippedHead;   // 머리 방어구
    public ItemData equippedBody;   // 몸 방어구
    public ItemData equippedLegs;   // 다리 방어구
    public ItemData equippedHands;  // 손 방어구

    public void EquipItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("아이템이 없습니다!");
            return;
        }

        switch (item.itemType)
        {
            case ItemType.Weapon:
                equippedWeapon = item;
                Debug.Log($"무기 장착: {item.itemName} (공격력: {item.attackPower})");
                break;

            case ItemType.Armor:
                switch (item.armorType)
                {
                    case ArmorType.Head:
                        equippedHead = item;
                        Debug.Log($"머리 방어구 장착: {item.itemName} (방어력: {item.defensePower})");
                        break;
                    case ArmorType.Body:
                        equippedBody = item;
                        Debug.Log($"몸 방어구 장착: {item.itemName} (방어력: {item.defensePower})");
                        break;
                    case ArmorType.Legs:
                        equippedLegs = item;
                        Debug.Log($"다리 방어구 장착: {item.itemName} (방어력: {item.defensePower})");
                        break;
                    case ArmorType.Hands:
                        equippedHands = item;
                        Debug.Log($"손 방어구 장착: {item.itemName} (방어력: {item.defensePower})");
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

