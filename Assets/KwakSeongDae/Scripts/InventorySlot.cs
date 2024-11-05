using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public enum SlotType
    {
        ActiveSlot, StorageSlot, Size
    }

    [Header("슬롯 종류 설정")]
    [Tooltip("ActiveSlot: 해당 슬롯에 들어온 아이템은 활성화\n" +
             "StorageSlot: 해당 슬롯에 들어온 아이템은 비활성화")]
    public SlotType slotType;
}
