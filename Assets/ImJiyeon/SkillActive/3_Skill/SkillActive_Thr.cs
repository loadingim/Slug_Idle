using System.Collections;
using UnityEngine;

public class SkillActive_Thr : MonoBehaviour
{
    [SerializeField] bool isActived;
    [SerializeField] int CoolTime;


    public void SkillOne()
    {
        StartCoroutine(SkillThrCoolTime());
    }

    IEnumerator SkillThrCoolTime()
    {
        while (isActived)
        {
            Debug.Log("세번째 스킬 사용됨");
            yield return new WaitForSeconds(CoolTime);
        }
    }
}
