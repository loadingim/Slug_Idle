using UnityEngine;
using UnityEngine.EventSystems;

public class MergeSlot : MonoBehaviour, IDropHandler
{
    [Header("머지 슬롯 기본 설정")]
    [SerializeField] MergeSystem system;

    public void OnDrop(PointerEventData eventData)
    {
        if (system == null) return;

        if (transform.childCount == 0)
        {
            if (eventData.pointerDrag.TryGetComponent<MergeItem>(out var Item))
            {
                Item.parentAfterDrag = transform;
            }
        }
        else if (transform.childCount == 1) // 이미 1개의 아이템을 지닌 경우에 머지 체크
        {
            if (eventData.pointerDrag.TryGetComponent<MergeItem>(out var dragItem))
            {
                var swapObject = transform.GetChild(0);
                if (swapObject.TryGetComponent<MergeItem>(out var swapItem))
                {
                    // 머지 레벨이 같은 경우 병합
                    if (dragItem.MergeLevel == swapItem.MergeLevel)
                    {
                        // 머지 레벨 상승 및 드래그 아이템은 삭제
                        system.Merge(swapItem,dragItem);
                        system.UpdateMergeStatus();
                    }
                    // 그렇지 않은 경우 스왑 진행
                    else
                    {
                        //원래 돌아가려고 했던 트랜스폼에 현재 자신이 가진 아이템을 넣기
                        swapObject.SetParent(dragItem.parentAfterDrag);
                        //아이템이 들어갈곳을 parent로 설정
                        dragItem.parentAfterDrag = transform;
                    }
                }
            }
        }
    }

}
