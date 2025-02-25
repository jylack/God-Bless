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

    // [버스터 콜] 모든 헌터가 특정 몬스터를 향해 이동하도록 호출
    public void BusterCallClick()
    {
        if (gameObject.name == "Buster Button")
        {

            // 현재 존재하는 몬스터가 있는지 확인 
            if (GameManager.Instance.listMonster.Count == 0)
            {
                Debug.LogWarning("BusterCall: 타겟할 몬스터가 없습니다.");
                return;
            }

            GameObject targetMonster = GameManager.Instance.listMonster[0];
            Transform targetTransform = targetMonster.transform;

            // GOD 객체를 찾아 계약된 헌터들을 가져옴
            GOD god = FindObjectOfType<GOD>();
            if (god == null)
            {
                Debug.LogWarning("BusterCall: GOD 인스턴스를 찾을 수 없습니다.");
                return;
            }

            // GOD에 등록된 모든 헌터에게 타겟 설정 및 추격 상태 전환
            foreach (UnitCtrl unit in god.listUnit)
            {
                if (unit.data.unitType == UnitType.Hunter)
                {
                    unit.SetTarget(targetTransform);
                }
            }

            Debug.Log("BusterCall 실행: 모든 헌터가 타겟 몬스터로 호출되었습니다.");

        }
    }

    // [아이템 사용] 예시: 아이템 사용 관련 기능 호출
    public void ItemClick()
    {
        Debug.Log("ItemClick: 아이템 사용 기능 호출");
    }

    // [스킬 부여] 예시: 헌터에게 스킬을 부여하는 기능 호출
    public void SkillClick()
    {
        GOD god = FindObjectOfType<GOD>();
        if (god != null && god.godSkill != null)
        {
            // 예시: GOD의 스킬 목록에서 첫 번째 스킬을 첫 번째 헌터에게 부여
            if (god.listUnit.Count > 0 && god.listGrantSkill.Count > 0)
            {
                UnitCtrl unit = god.listUnit[0];
                Hunter hunterComp = unit.GetComponent<Hunter>();
                if (hunterComp != null)
                {
                    GrantSkill skill = god.listGrantSkill[0];
                    god.godSkill.AllocateSkill(god, hunterComp, skill);
                    Debug.Log("SkillClick: 첫 번째 헌터에게 첫 번째 스킬 부여");
                }
                else
                {
                    Debug.LogWarning("SkillClick: 해당 유닛은 헌터가 아닙니다.");
                }
            }
            else
            {
                Debug.LogWarning("SkillClick: 헌터 또는 스킬 목록이 부족합니다.");
            }
        }
        else
        {
            Debug.LogWarning("SkillClick: GOD 인스턴스 또는 스킬 관리 객체가 없습니다.");
        }
    }

    // [그룹 배정] 예시: 헌터 그룹 배정 기능 (UI 선택 후 그룹 이동 등)
    public void GroupClick()
    {
        Debug.Log("GroupClick: 헌터 그룹 배정 기능 호출");
    }

    // [헌터 목록 확인] 예시: 현재 계약된 헌터 목록을 UI에 표시
    public void HunterListClick()
    {
        GOD god = FindObjectOfType<GOD>();
        if (god != null)
        {
            Debug.Log("HunterListClick: 현재 계약된 헌터 목록");
            foreach (UnitCtrl unit in god.listUnit)
            {
                if (unit.Type == UnitType.Hunter)
                {
                    Debug.Log($"헌터: {unit.UnitName}");
                }
            }
        }
        else
        {
            Debug.LogWarning("HunterListClick: GOD 인스턴스를 찾을 수 없습니다.");
        }
    }

    // [뽑기] 버튼 클릭 시 호출하여 GOD의 스킬 목록에 스킬을 추가하는 기능
    public void DrawSkillClick()
    {
        GOD god = FindObjectOfType<GOD>();
        if (god != null && god.godSkill != null)
        {
            god.godSkill.DrawSkill(god);
        }
        else
        {
            Debug.LogWarning("DrawSkillClick: GOD 인스턴스 또는 godSkill을 찾을 수 없습니다.");
        }
    }
}
