using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public enum SlotType
    {
        ActiveSlot, StorageSlot, Size
    }

    [Header("슬롯 종류 설정")]
    [Tooltip("ActiveSlot: 해당 슬롯에 들어온 아이템은 활성화\n" +
             "StorageSlot: 해당 슬롯에 들어온 아이템은 비활성화")]
    public SlotType slotType;
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            if (eventData.pointerDrag.TryGetComponent<InventoryItem>(out var Item))
            {
                Item.parentAfterDrag = transform;
            }
        }
        else if (transform.childCount == 1) // 이미 1개의 아이템을 지닌 경우에 서로 스왑
        {
            if (eventData.pointerDrag.TryGetComponent<InventoryItem>(out var Item))
            {
                //원래 돌아가려고 했던 트랜스폼에 현재 자신이 가진 아이템을 넣기
                var swapObject = transform.GetChild(0);
                swapObject.SetParent(Item.parentAfterDrag);
                // 바뀐 후, 드래그 안되는 아이템에 대해서 슬롯 수동 체크
                if (swapObject.TryGetComponent<InventoryItem>(out var Item2))
                {
                    Item2.SlotCheck();
                }

                //아이템이 들어갈곳을 parent로 설정
                Item.parentAfterDrag = transform;
            }
        }
    }
}
