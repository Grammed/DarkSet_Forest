using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// IDropHandler: 마우스 드롭했을 때 변화를 주기 위해 이용하는 인터페이스
public class Inventory_Slot : MonoBehaviour, IDropHandler
{
    // 슬롯 공간 이미지
    public Image image;
    public Color SelectedColor, NotSelectedColor;

    private void Awake()
    {
        DeSelect();
    }

    public void Select()
    {
        image.color = SelectedColor;
    }

    public void DeSelect()
    {
        image.color= NotSelectedColor;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        // 아무것도 없는 슬롯인가?
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
