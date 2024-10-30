using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour/*, IDropHandler*/
{
    public enum SlotType
    {
        ActiveSlot, StorageSlot, Size
    }

    [Header("슬롯 종류 설정")]
    [Tooltip("ActiveSlot: 해당 슬롯에 들어온 아이템은 활성화\n" +
             "StorageSlot: 해당 슬롯에 들어온 아이템은 비활성화")]
    public SlotType slotType;
    /* 드롭 기능 비활성화
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            if (eventData.pointerDrag.TryGetComponent<InventoryItem>(out var Item) && Item.canUse)
            {
                Item.parentAfterDrag = transform;
            }
        }
        else if (transform.childCount == 1) // 이미 1개의 아이템을 지닌 경우에 서로 스왑
        {
            if (eventData.pointerDrag.TryGetComponent<InventoryItem>(out var dragItem) && dragItem.canUse)
            {
                var swapObject = transform.GetChild(0);
                // 바뀐 후, 스왑될 아이템에 대한 슬롯 체크 수행
                if (swapObject.TryGetComponent<InventoryItem>(out var swapItem) && swapItem.canUse)
                {
                    //원래 돌아가려고 했던 트랜스폼에 현재 자신이 가진 아이템을 넣기
                    swapObject.SetParent(dragItem.parentAfterDrag);
                    swapItem.SlotCheck();
                    //아이템이 들어갈곳을 parent로 설정
                    dragItem.parentAfterDrag = transform;
                }
            }
        }
    }
    */
}
