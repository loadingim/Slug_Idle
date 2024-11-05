using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 아이템 (무기, 스킬 등)은 실제 아이템과 독립적
/// 무기나 스킬 스크립트를 포함한 
/// </summary>
public class InventoryItem : MonoBehaviour
{
    [Header("인벤토리 아이템 기본 설정")]
    [Tooltip("실제 아이템 오브젝트")]
    public GameObject ItemObject;
    [Tooltip("인벤토리 아이템의 버튼")]
    [SerializeField] private Button button;
    [Tooltip("활성화 슬롯")]
    [SerializeField] private InventorySlot activeSlot;
    [Tooltip("슬롯을 보유하는 인벤토리 트랜스폼")]
    [SerializeField] private Transform inventory;
    [Tooltip("아이템 해금 여부")]
    public bool canUse;

    private Image image;                                // 이미지UI가 없는 경우에 비정상적인 인벤토리 아이템이라 간주하고 비활성화
    [HideInInspector] public Transform originParent;

    private void Awake()
    {
        // 원래 부모 저장
        originParent = transform.parent;
    }
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
    private void OnDisable()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void Update()
    {
        // 해금 여부 확인
        button.interactable = canUse;
    }

    void MoveToActiveSlot()
    {
        // 해금 여부 다시 확인
        if (canUse == false) return;

        // 현재 아이템이 슬롯에 있는지 체크
        if (transform.parent.TryGetComponent<InventorySlot>(out var slot) == false) return;

        // if 활성화 슬롯에 아이템이 있는 경우는 원래 슬롯으로
        if (slot.slotType == InventorySlot.SlotType.ActiveSlot)
        {
            transform.SetParent(originParent);
        }
        // else 스토리지 슬롯에 아이템이 있는 경우는 활성화 슬롯으로
        else
        {
            // 원래 활성화 슬롯에 있던 아이템을 원래 슬롯에 옮기기
            if (activeSlot.transform.childCount > 0)
            {
                var activeItem = activeSlot.transform.GetChild(0);
                if (activeItem.TryGetComponent<InventoryItem>(out var item))
                {
                    activeItem.SetParent(item.originParent);
                    item.SlotCheck();
                }
            }
            // 현재 슬롯의 아이템은 활성화 슬롯으로 옮기기
            transform.SetParent(activeSlot.transform);
        }

        // 어떤 작업이 수행되든지 현재 슬롯 체크 필요
        SlotCheck();
    }
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