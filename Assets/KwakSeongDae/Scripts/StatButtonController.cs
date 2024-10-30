using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatButtonController : MonoBehaviour
{
    enum CoolTimeType
    {
        FixedTime,DynamicTime
    }
    [Header("버튼 감도 설정")]
    [Tooltip("버튼 클릭 시, 일정 시간 후에 반복 트리거 동작 실행")]
    [SerializeField] private float delayAfterButtonDown;
    [Tooltip("반복 트리거 시간 간격 설정")]
    [SerializeField] private float triggerCoolTime;
    [Tooltip("일정 시간 간격: 시간이 지나도 같은 시간 간격으로 스탯 구매\n 변동 시간 간격: 시간이 지나면 최소 시간 간격으로 스탯 구매, 시간 변동은 Lerp")]
    [SerializeField] private CoolTimeType coolTimeType;
    [Tooltip("최소 시간 간격까지 도달하는 시간")]
    [SerializeField] private float minCoolTimeDelay;
    [Tooltip("최소 시간 간격 설정")]
    [SerializeField] private float triggerMinCoolTime;


    [Header("클릭 시 실행할 이벤트")]
    [SerializeField] UnityEvent onClick;

    private Coroutine coroutine;
    public void ButtonDown()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(RefeatTriggerRoutine());
        }
    }

    public void ButtonUp()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    IEnumerator RefeatTriggerRoutine()
    {
        // 일회성 버튼 클릭 처리 및 딜레이 이후 반복 트리거 작업 실시
        onClick?.Invoke();

        yield return new WaitForSeconds(delayAfterButtonDown);
        var delay = new WaitForSeconds(triggerCoolTime);
        var minCoolDelay = minCoolTimeDelay;
        while (true)
        {
            if (coolTimeType == CoolTimeType.DynamicTime)
            {
                // 감소된 시간 비율에 따라 시간 간격 조정 ( 최소 시간 + (누른 시간 비율 * 최대 최소 시간 차이) )
                delay = new WaitForSeconds( triggerMinCoolTime + (minCoolDelay / minCoolTimeDelay * (triggerCoolTime - triggerMinCoolTime)));
            }
            
            onClick?.Invoke();
            
            yield return delay;
            minCoolDelay -= triggerCoolTime; 
        }
    }
}
