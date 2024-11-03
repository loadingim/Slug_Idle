using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;

    private Dictionary<string, IEnumerator> coList = new Dictionary<string, IEnumerator>();

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
    /// 코루틴 시작
    /// </summary>
    /// <param name="co">실행할 코루틴</param>
    /// <param name="coName">코루틴 이름</param>
    public void ManagerCoroutineStart(IEnumerator co, string coName)
    {
        //동일한 이름이 존재하는지 Dictionary 검사
        if (coList.ContainsKey(coName))
        {
            //동일한 이름이 있다면 Value를 가져올 수 있는지 확인
            if (coList.TryGetValue(coName, out IEnumerator copyCo))
            {
                //Value를 가져왔다면 코루틴이 실행중인 상태이니 중지하고 삭제
                StopCoroutine(copyCo);
                coList.Remove(coName);
            }
        }

        coList[coName] = co;
        StartCoroutine(StartMyCoroutine(co, coName));
    }
 
    /// <summary>
    /// 코루틴 강제 종료
    /// </summary>
    /// <param name="coName">코루틴 이름</param>
    public void ManagerCoroutineStop(string coName)
    {
        if (coList.ContainsKey(coName))
        {
            StopCoroutine(coList[coName]);
            coList.Remove(coName);
        }
    }
  
    public IEnumerator StartMyCoroutine(IEnumerator co, string coName)
    { 
        //각 코루틴 조건별로 실행이 되고 있는 상태인지 확인
        while (co.MoveNext())
        { 
            //인벤토리가 열린 상태면 일시 중지
            if (GameManager.Instance.IsOpenInventory)
            {
                yield return new WaitUntil(() => !GameManager.Instance.IsOpenInventory);
            }
            else
            { 
                //현재 코루틴에서 사용중인 wait값으로 대기
                yield return co.Current;
            }
        }

        //코루틴이 종료됐으면 제거 
        if (coList.ContainsKey(coName))
        {
            coList.Remove(coName);
        }
    }


}
