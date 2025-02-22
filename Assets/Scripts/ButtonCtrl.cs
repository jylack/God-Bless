using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public void BusterCallClick()
    {


    }

    public void ItemClick()
    {

    }

    public void SkillClick()
    {

    }

    public void GroupClick()
    {

    }

    public void HunterListClick()
    {

    }

}
