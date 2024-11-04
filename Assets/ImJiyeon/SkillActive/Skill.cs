using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    public bool isActived;
    public string SetCoolTimeCoroutineName = "SetCoolTime";


    private void Start()
    {
        isActived = true;
    }

    // 스킬의 발동 함수
    public abstract void Activate();


    // 스킬의 쿨타임을 걔산하는 함수 (해당 코드는 임시)
    public IEnumerator SetCurrentCooltime(float Cool, Image LookCoolTime, Button skillButton)
    {
        if (isActived == false)
        {
            LookCoolTime.gameObject.SetActive(true);
            skillButton.interactable = false;

            Debug.Log("쿨타임 시작");
            float MaxCool = Cool;

            while (Cool > 0.1f)
            {
                Cool -= Time.deltaTime;
                LookCoolTime.fillAmount = (Cool / MaxCool);

                yield return new WaitForFixedUpdate();
            }

            LookCoolTime.gameObject.SetActive(false);
            skillButton.interactable = true;

            Debug.Log("쿨타임 종료");
            isActived = true;
        }
    }
}
