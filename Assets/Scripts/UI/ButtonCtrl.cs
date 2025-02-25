using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour
{
    public Sprite[] busterCallsprite = new Sprite[2];
    Image image;

    private void Start()
    {
        if (gameObject.name == "Buster Button")
        {
            image = transform.parent.GetChild(1).GetComponent<Image>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.name == "Buster Button")
        {
            image.sprite = busterCallsprite[1];
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.name == "Buster Button")
        {
            image.sprite = busterCallsprite[0];
        }
    }

    // [������ ��] ��� ���Ͱ� Ư�� ���͸� ���� �̵��ϵ��� ȣ��
    public void BusterCallClick()
    {
        if (gameObject.name == "Buster Button")
        {

            // ���� �����ϴ� ���Ͱ� �ִ��� Ȯ�� 
            if (GameManager.Instance.listMonster.Count == 0)
            {
                Debug.LogWarning("BusterCall: Ÿ���� ���Ͱ� �����ϴ�.");
                return;
            }

            GameObject targetMonster = GameManager.Instance.listMonster[0];
            Transform targetTransform = targetMonster.transform;

            // GOD ��ü�� ã�� ���� ���͵��� ������
            GOD god = FindObjectOfType<GOD>();
            if (god == null)
            {
                Debug.LogWarning("BusterCall: GOD �ν��Ͻ��� ã�� �� �����ϴ�.");
                return;
            }

            // GOD�� ��ϵ� ��� ���Ϳ��� Ÿ�� ���� �� �߰� ���� ��ȯ
            foreach (UnitCtrl unit in god.listUnit)
            {
                if (unit.data.unitType == UnitType.Hunter)
                {
                    unit.SetTarget(targetTransform);
                }
            }

            Debug.Log("BusterCall ����: ��� ���Ͱ� Ÿ�� ���ͷ� ȣ��Ǿ����ϴ�.");

        }
    }

    // [������ ���] ����: ������ ��� ���� ��� ȣ��
    public void ItemClick()
    {
        Debug.Log("ItemClick: ������ ��� ��� ȣ��");
    }

    // [��ų �ο�] ����: ���Ϳ��� ��ų�� �ο��ϴ� ��� ȣ��
    public void SkillClick()
    {
        GOD god = FindObjectOfType<GOD>();
        if (god != null && god.godSkill != null)
        {
            // ����: GOD�� ��ų ��Ͽ��� ù ��° ��ų�� ù ��° ���Ϳ��� �ο�
            if (god.listUnit.Count > 0 && god.listGrantSkill.Count > 0)
            {
                UnitCtrl unit = god.listUnit[0];
                Hunter hunterComp = unit.GetComponent<Hunter>();
                if (hunterComp != null)
                {
                    GrantSkill skill = god.listGrantSkill[0];
                    god.godSkill.AllocateSkill(god, hunterComp, skill);
                    Debug.Log("SkillClick: ù ��° ���Ϳ��� ù ��° ��ų �ο�");
                }
                else
                {
                    Debug.LogWarning("SkillClick: �ش� ������ ���Ͱ� �ƴմϴ�.");
                }
            }
            else
            {
                Debug.LogWarning("SkillClick: ���� �Ǵ� ��ų ����� �����մϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("SkillClick: GOD �ν��Ͻ� �Ǵ� ��ų ���� ��ü�� �����ϴ�.");
        }
    }

    // [�׷� ����] ����: ���� �׷� ���� ��� (UI ���� �� �׷� �̵� ��)
    public void GroupClick()
    {
        Debug.Log("GroupClick: ���� �׷� ���� ��� ȣ��");
    }

    // [���� ��� Ȯ��] ����: ���� ���� ���� ����� UI�� ǥ��
    public void HunterListClick()
    {
        GOD god = FindObjectOfType<GOD>();
        if (god != null)
        {
            Debug.Log("HunterListClick: ���� ���� ���� ���");
            foreach (UnitCtrl unit in god.listUnit)
            {
                if (unit.Type == UnitType.Hunter)
                {
                    Debug.Log($"����: {unit.UnitName}");
                }
            }
        }
        else
        {
            Debug.LogWarning("HunterListClick: GOD �ν��Ͻ��� ã�� �� �����ϴ�.");
        }
    }

    // [�̱�] ��ư Ŭ�� �� ȣ���Ͽ� GOD�� ��ų ��Ͽ� ��ų�� �߰��ϴ� ���
    public void DrawSkillClick()
    {
        GOD god = FindObjectOfType<GOD>();
        if (god != null && god.godSkill != null)
        {
            god.godSkill.DrawSkill(god);
        }
        else
        {
            Debug.LogWarning("DrawSkillClick: GOD �ν��Ͻ� �Ǵ� godSkill�� ã�� �� �����ϴ�.");
        }
    }
}
