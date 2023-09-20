using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler // ���� �巡�� ���� �巡�� �� �巡�� ���� ��Ÿ���� ���� Ŭ����
{
    [Header("UI")]
    public Image image; //������ ����
    public Text countText; // ������ ������ �ؽ�Ʈ

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag; //�θ� ��ġ

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false; // �̹����� ����ĳ��Ʈ Ÿ�ٲ�
        parentAfterDrag = transform.parent; // �θ� ��ġ ����
        transform.SetParent(transform.root); // �θ� ��ġ �̵�
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
