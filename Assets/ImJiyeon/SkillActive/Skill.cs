using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    private GameObject player;

    protected PlayerDataModel playerDataModel;
    protected GameManager gameManager;

    public Image LookCoolTime;
    public bool isActived;

    [Header("Stat")]
    public float SkillAttack;
    public float CoolTime;


    private void Start()
    {
        // 임시 플레이어 데이터 모델 참조 코드, 필요 없을 시 삭제 예정
        player = GameObject.FindGameObjectWithTag("Player");
        playerDataModel = player.GetComponent<PlayerDataModel>();

        gameManager = GameManager.Instance.GetComponent<GameManager>();

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

                if (GameManager.Instance.StageInstance.CoolTimeReset)
                {
                    Cool = 0.1f;
                }

                yield return new WaitForFixedUpdate();
            }

            LookCoolTime.gameObject.SetActive(false);
            skillButton.interactable = true;

            Debug.Log("쿨타임 종료");
            isActived = true;
        }
    }
}
