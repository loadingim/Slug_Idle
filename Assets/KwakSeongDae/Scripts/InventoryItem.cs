using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 아이템 (무기, 스킬 등)은 실제 아이템과 독립적
/// 무기나 스킬 스크립트를 포함한 
/// </summary>
/// <param name="ItemObject"> 실제 아이템에 대한 참조 (슬롯의 타입에 따라, 아이템 활성화 여부 결정) </param>
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // 슬롯의 종류(현재 장착 슬롯, 보관용 슬롯)에 따라 해당 게임 오브젝트 활성화 여부 체크
    public GameObject ItemObject;
    [HideInInspector] public Transform parentAfterDrag; // 드래그 후에 다시 원래 위치로 돌아갈 부모 트랜스폼

    private Image image;
    // 이미지UI가 없는 경우에 비정상적인 인벤토리 아이템이라 간주하고 비활성화
    void OnEnable()
    {
        if (TryGetComponent<Image>(out image) == false)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // 현재 자신의 슬롯 체크
            SlotCheck();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그할 아이템의 Raycast 가능 여부 해제 (다른 Ray와 상호작용되지 않도록)
        image.raycastTarget = false;
        // 일단 드래그 시작하면, 원래 부모의 트랜스폼을 저장 후, 부모 관계 끊기
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        SlotCheck();
    }

    /// <summary>
    /// 현재 자신이 속한 슬롯의 타입을 체크하여 실제 아이템을 컨트롤하는 함수
    /// </summary>
    public void SlotCheck()
    {
        if (transform.parent.TryGetComponent<InventorySlot>(out var slot))
        {
            switch (slot.slotType)
            {
                case InventorySlot.SlotType.ActiveSlot:
                    ItemObject.SetActive(true);
                    break;
                case InventorySlot.SlotType.StorageSlot:
                    ItemObject.SetActive(false);
                    break;
            }
        }
    }
}
