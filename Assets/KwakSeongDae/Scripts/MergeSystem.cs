using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MergeSystem : MonoBehaviour
{
    [Header("머지 시스템 기본 설정")]
    [SerializeField] private GameObject mergePrefab;
    [SerializeField] private int topMergeLevel;
    public int TopMergeLevel
    {
        get { return topMergeLevel; }
        set
        {
            if (topMergeLevel < value)
            {
                topMergeLevel = value;
                if (PlayerDataModel.Instance != null)
                    PlayerDataModel.Instance.BulletLevel = topMergeLevel;
            }
        }
    }
    [Tooltip("최소 레벨 보정(최대 레벨에서 해당 값만큼 뺀 레벨이 최소 레벨로 책정)")]
    [SerializeField] private int difLevel;

    [Header("자동 머지 관련 설정")]
    [Tooltip("머지 자동 생산 간격")]
    [SerializeField] private float mergeManufactureTime;
    [SerializeField] private bool IsAutoMerge;

    public Dictionary<int, List<MergeItem>> MergeItemDictionary;

    // 머지 슬롯들
    private List<Transform> slots;
    // 자동 머지 루틴
    private Coroutine autoMergeRoutine;
    // 현재 최소 머지 레벨
    private int curMinLevel;
    private void Start()
    {
        // 초기화
        MergeItemDictionary = new Dictionary<int, List<MergeItem>>();
        slots = new List<Transform>();
        curMinLevel = 1;

        //현재 머지 아이템이 있는지 확인해서 딕셔너리에 넣기
        foreach (var item in GetComponentsInChildren<MergeItem>())
        {
            AddItemMergeDictionary(item.MergeLevel, item);
        }

        //현재 머지 슬롯들 추가
        foreach (var slot in GetComponentsInChildren<MergeSlot>())
        {
            slots.Add(slot.transform);
        }

        //자동 머지 관련 루틴 실행
        autoMergeRoutine = StartCoroutine(AutoMergeCoroutine());
    }

    /// <summary>
    /// 새로운 머지 추가하는 함수
    /// </summary>
    public void AddMerge()
    {
        if (mergePrefab == null) return;

        int emptySlotIdx = CheckEmptySlot();

        if (emptySlotIdx > -1)
        {
            var obj = Instantiate(mergePrefab, slots[emptySlotIdx]);

            // 머지 레벨 갱신
            if (obj.TryGetComponent<MergeItem>(out var item))
            {
                TopMergeLevel = item.MergeLevel;

                // 현재 최소 레벨 업데이트
                UpdateCurrentMinLevel();

                item.MergeLevel = curMinLevel;

                AddItemMergeDictionary(item.MergeLevel, item);
                item.system = this;
            }
            else
            {
                //비정상 프리팹으로 판단하여 삭제
                Destroy(obj);
            }
        }
    }

    /// <summary>
    /// 머지 딕셔너리에 아이템 추가하는 함수
    /// </summary>
    /// <param name="key"> 머지 아이템의 레벨</param>
    /// <param name="item"> 머지 아이템 </param>
    void AddItemMergeDictionary(int key, MergeItem item)
    {
        if (MergeItemDictionary.ContainsKey(key) == false)
        {
            // 해당 레벨의 딕셔너리가 새로 추가되는 경우,새로운 리스트 추가
            MergeItemDictionary[key] = new List<MergeItem>();
        }

        MergeItemDictionary[key].Add(item);
    }

    /// <summary>
    /// 두 머지를 합성하는 함수, origin의 정보만 보존
    /// origin의 level이 1 상승
    /// </summary>
    public void Merge(MergeItem origin, MergeItem draging)
    {
        if (origin == null || draging == null) return;

        // draging 아이템 삭제
        Destroy(draging.gameObject);

        // origin의 정보 갱신
        var originLevel = origin.MergeLevel;
        origin.MergeLevel++;
        // 현재 머지 최고 레벨 체크, 원래 레벨보다 높은 머지 레벨인 경우 자동 갱신
        TopMergeLevel = origin.MergeLevel;

        // 현재 최소 레벨 업데이트
        UpdateCurrentMinLevel();
    }

    public void UpdateMergeStatus()
    {
        // 모든 총알을 순회하며, 할 수 있는 만큼 머지 후, 나머지는 버리기
        for (int level = 1; level < TopMergeLevel; level++)
        {
            if (MergeItemDictionary.ContainsKey(level))
            {
                // 다른 곳에서 삭제함에 따라, 유령 참조가 되고 있는 해당 요소를 리스트에서 삭제
                MergeItemDictionary[level].RemoveAll(item => item == null);

                // if 레벨이 최소 레벨보다 낮은 경우: 할 수 있을만큼 머지하고 나머지는 버림
                if (level < curMinLevel)
                {
                    // 1. 머지 대상 오브젝트 선택
                    int originMergeidx = -1;
                    // 3. 1 ~ 2 반복
                    for (int i = 0; i < MergeItemDictionary[level].Count; i++)
                    {
                        if (originMergeidx == -1)
                        {
                            originMergeidx = i;
                        }
                        else
                        {
                            // 2. 머지 진행
                            Merge(MergeItemDictionary[level][originMergeidx], MergeItemDictionary[level][i]);
                            // 레벨업된 딕셔너리에 추가
                            AddItemMergeDictionary(level + 1, MergeItemDictionary[level][originMergeidx]);
                            originMergeidx = -1;
                        }
                    }
                    // 4. 나머지 하나는 정리, originMergeidx가 남은 하나
                    if (originMergeidx > 0) DestroyImmediate(MergeItemDictionary[level][originMergeidx].gameObject);
                    // 리스트 정리
                    MergeItemDictionary[level].Clear();
                }
                // else 현재 리스트에 레벨이 맞지 않는 오브젝트는 갱신
                else
                {
                    // 실제 참조 되는 아이템들 중, 레벨이 맞지 않은 아이템은 갱신
                    for (int i = 0; i < MergeItemDictionary[level].Count; i++)
                    {
                        // 레벨 상태가 현재 리스트와 맞지 않은 경우, 갱신
                        if (MergeItemDictionary[level][i].MergeLevel != level)
                        {
                            int updateLevel = MergeItemDictionary[level][i].MergeLevel;
                            // 레벨업된 딕셔너리에 추가하고, 기존 리스트에서 삭제
                            AddItemMergeDictionary(updateLevel, MergeItemDictionary[level][i]);
                            MergeItemDictionary[level].RemoveAt(i);
                            //인덱스 조정
                            i--;
                        }
                    }
                }
            }
        }
    }

    void UpdateCurrentMinLevel()
    {
        // 최소 레벨 갱신
        if (TopMergeLevel - difLevel > curMinLevel)
        {
            curMinLevel = TopMergeLevel - difLevel;
        }
    }

    IEnumerator AutoMergeCoroutine()
    {
        var delay = new WaitForSeconds(mergeManufactureTime);
        while (true)
        {
            if (IsAutoMerge)
            {
                AddMerge();
                MergeItem m1 = null;
                MergeItem m2 = null;
                bool isSkip = false;
                // 모든 아이템 최고레벨부터 순회하며 병합 가능 조건인 경우 머지
                for (int i = TopMergeLevel; i > 0; i--)
                {
                    if (isSkip) break;
                    if (MergeItemDictionary.ContainsKey(i) == false) continue;

                    MergeItem originItem = null;

                    // 같은 레벨의 아이템 둘이 존재하면 머지 진행
                    // 한 턴에 한번 머지 진행됨, But, 최소 레벨보다 낮은 레벨 머지는 한번에 진행
                    foreach (var item in MergeItemDictionary[i])
                    {
                        if (isSkip) break;

                        if (originItem == null)
                        {
                            originItem = item;
                        }
                        else
                        {
                            m1 = originItem;
                            m2 = item;
                            isSkip = true;
                        }
                    }
                }
                // 머지 진행
                Merge(m1, m2);
                UpdateMergeStatus();
                yield return delay;
            }
            else yield return null;
        }
    }

    public void AutoMergeToggleButton()
    {
        IsAutoMerge ^= true;
    }

    /// <summary>
    /// 빈 머지 슬롯을 체크하는 함수
    /// </summary>
    /// <returns> 빈 머지 슬롯의 인덱스, -1은 빈 슬롯이 없는 경우 </returns>
    int CheckEmptySlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount < 1) return i;
        }
        return -1;
    }
}
