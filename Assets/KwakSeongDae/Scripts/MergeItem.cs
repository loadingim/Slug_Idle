using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MergeItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("머지 기본 설정")]
    [SerializeField] private int mergeLevel;
    [HideInInspector] public MergeSystem system;
    public int MergeLevel
    {
        get { return mergeLevel; }
        set
        {
            if (mergeLevel < value)
            {
                // 레벨 갱신되면서 레벨 텍스트도 수정
                mergeLevel = value;
                mergeLevelText?.SetText(mergeLevel.ToString());
            }
        }
    }
    [Tooltip("머지 레벨 표기할 텍스트")]
    [SerializeField] TextMeshProUGUI mergeLevelText;
    [Tooltip("더블 클릭 시간 간격 설정")]
    [SerializeField] private float doubleClickTIme;
    

    [HideInInspector] public Transform parentAfterDrag; // 드래그 후에 다시 원래 위치로 돌아갈 부모 트랜스폼
    private Image image;                                // 이미지UI가 없는 경우에 비정상적인 인벤토리 아이템이라 간주하고 비활성화

    void OnEnable()
    {
        if (TryGetComponent<Image>(out image) == false)
        {
            gameObject.SetActive(false);
        }
        else
        {
            mergeLevelText?.SetText(MergeLevel.ToString());
        }
    }

    #region 아이템 드래그 기능
    private Vector3 originPos;
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
    }
    #endregion

    #region 아이템 더블클릭 기능
    private float lastClickTime;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (system == null) return;

        if (Time.time - lastClickTime < doubleClickTIme)
        {
            if (system.MergeItemDictionary.ContainsKey(MergeLevel))
            {
                // 특정 머지를 삭제하면서 현재 머지 레벨 업
                for (int i = 0; i < system.MergeItemDictionary[MergeLevel].Count; i++)
                {
                    // 자기 자신끼리 머지되는 경우 방지
                    if (system.MergeItemDictionary[MergeLevel][i] == this) continue;
                    // 합성할 수 있는 머지가 없는 경우는 합성 종료
                    if (system.MergeItemDictionary[MergeLevel][i] == null) continue;
                    // 레벨업된 머지에 맞는 머지 딕셔너리에 추가
                    system.Merge(this,system.MergeItemDictionary[MergeLevel][i]);
                    system.UpdateMergeStatus();
                    // 머지 합성 종료
                    break;
                }
            }
        }
        lastClickTime = Time.time;
    }
    #endregion
}
