using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_Slot : MonoBehaviour, IDropHandler
{
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
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
