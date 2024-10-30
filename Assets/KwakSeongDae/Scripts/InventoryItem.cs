using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 아이템 (무기, 스킬 등)은 실제 아이템과 독립적
/// 무기나 스킬 스크립트를 포함한 
/// </summary>
public class InventoryItem : MonoBehaviour/*, IBeginDragHandler, IDragHandler, IEndDragHandler */
{
    [Header("인벤토리 아이템 기본 설정")]
    [Tooltip("실제 아이템 오브젝트")]
    [SerializeField] private GameObject ItemObject;
    [Tooltip("인벤토리 아이템의 버튼")]
    [SerializeField] private Button button;
    [Tooltip("활성화 인벤토리 슬롯")]
    [SerializeField] private InventorySlot activeSlot;
    [Tooltip("아이템 해금 여부")]
    public bool canUse;
    /* [HideInInspector] public Transform parentAfterDrag; // 드래그 후에 다시 원래 위치로 돌아갈 부모 트랜스폼 */

    private Image image;                                // 이미지UI가 없는 경우에 비정상적인 인벤토리 아이템이라 간주하고 비활성화

    void OnEnable()
    {
        if (TryGetComponent<Image>(out image) == false || button == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            button.onClick.AddListener(MoveToActiveSlot);
            // 현재 자신의 슬롯 체크
            SlotCheck();
        }
    }

    void MoveToActiveSlot()
    {
        // 해금 여부 확인
        if (canUse == false) return;

        // 원래 활성화 슬롯에 있던 아이템을 현재 슬롯에 옮기기
        if (activeSlot.transform.childCount > 0)
        {
            var activeItem = activeSlot.transform.GetChild(0);
            activeItem.SetParent(transform.parent);
            if (activeItem.TryGetComponent<InventoryItem>(out var item))
            {
                item.SlotCheck();
            }
        }

        // 현재 슬롯의 아이템은 활성화 슬롯으로 옮기기
        transform.SetParent(activeSlot.transform);
        SlotCheck();
    }

    /* 아이템 드래그 기능
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canUse)
        {
            // 드래그할 아이템의 Raycast 가능 여부 해제 (다른 Ray와 상호작용되지 않도록)
            image.raycastTarget = false;
            // 일단 드래그 시작하면, 원래 부모의 트랜스폼을 저장 후, 부모 관계 끊기
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canUse)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canUse)
        {
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
            SlotCheck();
        }
    }
    */

    /// <summary>
    /// 현재 자신이 속한 슬롯의 타입을 체크하여 실제 아이템을 컨트롤하는 함수
    /// 모든 아이템은 OnEnable()에서 초기화 작업 필요
    /// </summary>
    public void SlotCheck()
    {
        if (transform.parent != null && transform.parent.TryGetComponent<InventorySlot>(out var slot))
        {
            if (ItemObject == null)
            {
                Debug.Log("현재 인벤토리 아이템에 실제 아이템 오브젝트가 할당되지 않았습니다.");
                return;
            }
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
