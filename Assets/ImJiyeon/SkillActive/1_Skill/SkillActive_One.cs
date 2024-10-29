using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillActive_One : MonoBehaviour
{
    [SerializeField] bool isActived;
    [SerializeField] int CoolTime;


    public void SkillOne()
    {
        if (isActived == true)
        {
            Debug.Log("첫번째 스킬 사용됨");
            isActived = false;
        }

        StartCoroutine(SkillOneActive());
    }

    IEnumerator SkillOneActive()
    {
        gameObject.GetComponent<Button>().interactable = false;

        while (isActived == false)
        {
            for (int i = 0; CoolTime < i; CoolTime--)
            {
                Debug.Log($"쿨타임 적용중 : {i}");
            }

            yield return new WaitForSeconds(CoolTime);
        }

        isActived = true;
        gameObject.GetComponent<Button>().interactable = true;
    }
}
