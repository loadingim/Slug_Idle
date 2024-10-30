using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillActive_One : MonoBehaviour
{
    [SerializeField] Image LookCoolTime;
    [SerializeField] float CoolTime;


    public void SkillOne()
    {
        if (LookCoolTime.gameObject.activeSelf == false)
        {
            Debug.Log("첫번째 스킬 사용됨");

            StartCoroutine(SkillOneCoolTime(CoolTime));
        }
    }


    IEnumerator SkillOneCoolTime(float Cool)
    {
        LookCoolTime.gameObject.SetActive(true);
        gameObject.GetComponent<Button>().interactable = false;

        Debug.Log("쿨타임 시작");
        float MaxCool = Cool;

        while (Cool > 0.1f)
        {
            Cool -= Time.deltaTime;
            LookCoolTime.fillAmount = (Cool / MaxCool);

            yield return new WaitForFixedUpdate();
        }

        LookCoolTime.gameObject.SetActive(false);
        gameObject.GetComponent<Button>().interactable = true;

        Debug.Log("쿨타임 종료");
    }
}
