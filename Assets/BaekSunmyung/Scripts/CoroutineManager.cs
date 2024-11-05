using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance { get; private set; }


    private Dictionary<MonoBehaviour, Coroutine> newCoList = new Dictionary<MonoBehaviour, Coroutine>();
     
    public Dictionary<float, WaitForSeconds> _waitForSeconds = new Dictionary<float, WaitForSeconds>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
     
    /// <summary>
    /// WaitForSeconds 객체 반환
    /// </summary>
    /// <param name="time">지연 시간 Key</param>
    /// <returns></returns>
    public WaitForSeconds GetWaitForSeconds(float time)
    {
        if (!_waitForSeconds.ContainsKey(time))
        {
            _waitForSeconds[time] = new WaitForSeconds(time);
        }

        return _waitForSeconds[time];
    }
     

    /// <summary>
    /// 코루틴 시작
    /// </summary>
    /// <param name="value">실행할 코루틴</param>
    /// <param name="key">코루틴 이름</param>
    public IEnumerator ManagerCoroutineStart(Coroutine value, MonoBehaviour key)
    {
        //동일한 이름이 존재하는지 Dictionary 검사
        if (newCoList.ContainsKey(key))
        {
            if (newCoList.TryGetValue(key, out Coroutine copy))
            {
                Debug.Log("코루틴 중지");
                StopCoroutine(copy);
                newCoList.Remove(key);
            }
        }

        Debug.Log("리스트에 코루틴 할당");
        newCoList[key] = value; 
        IEnumerator enumerator = newCoList.GetEnumerator();

        while (enumerator.MoveNext())
        {
            //인벤토리가 열린 상태면 일시 중지
            if (GameManager.Instance.IsOpenInventory)
            {
                yield return new WaitUntil(() => !GameManager.Instance.IsOpenInventory);
            }
            else
            {
                Debug.Log("코루틴 반복");
                //현재 코루틴에서 사용중인 wait값으로 대기
                yield return enumerator.Current;
            } 
        }
    }

    /// <summary>
    /// 코루틴 강제 종료
    /// </summary>
    /// <param name="coName">코루틴 이름</param>
    public void ManagerCoroutineStop(MonoBehaviour key)
    { 
        if (newCoList.ContainsKey(key))
        {
            Debug.Log("코루틴 강제 종료");
            StopCoroutine(newCoList[key]);
            newCoList.Remove(key); 
        }
    } 
}
