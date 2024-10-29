using System.Collections;
using UnityEngine;

public class SkillActive_One : MonoBehaviour
{
    [SerializeField] bool isActived;
    [SerializeField] int CoolTime;


    public void SkillOne()
    {
        StartCoroutine(SkillOneActive());
    }

    IEnumerator SkillOneActive()
    {
        while (isActived)
        {
            Debug.Log("첫번째 스킬 사용됨");
            yield return new WaitForSeconds(CoolTime);
        }
    }
}
