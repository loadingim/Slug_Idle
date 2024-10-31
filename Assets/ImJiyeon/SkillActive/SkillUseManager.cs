using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillUseManager : MonoBehaviour
{
    [SerializeField] List<Skill> ActiveSkill = new();

    [Header("Auto")]
    [SerializeField] bool AutoOnOff;
    [SerializeField] Button ActiveAuto;
    [SerializeField] TextMeshProUGUI AutoOnText;
    [SerializeField] TextMeshProUGUI AutoOffText;

    [Header("Toggle Move")]
    [SerializeField] Animator ani;
    private int curHash;
    private static int EnableHash = Animator.StringToHash("Enable");
    private static int DisableHash = Animator.StringToHash("Disable");

    // ========

    void AniPlay()
    {
        int checkAniHash;

        if (AutoOnOff == false) { checkAniHash = DisableHash; }
        else if (AutoOnOff) { checkAniHash = EnableHash; }
        else return;

        if (curHash != checkAniHash)
        {
            curHash = checkAniHash;
            ani.Play(curHash);
        }
    }

    // ========

    private void Awake()
    {
        // 추후 스킬 사용 UI를 생성하며 비활성화로 돌릴 예정
        for (int i = 0; i < ActiveSkill.Count; i++)
        {
            ActiveSkill[i].gameObject.GetComponent<Button>().interactable = true;
        }

        AutoOnOff = false;
        ColorChange();
    }


    // 클릭으로 오토버튼 활성화 - 비활성화 상태 전환 구현
    public void AutoClick()
    {
        // 비활성화 - 활성화
        if (AutoOnOff == false) { AutoOnOff = true; }

        // 활성화 - 비활성화
        else if (AutoOnOff) { AutoOnOff = false; }

        StartCoroutine(SkillAuto());
    }


    IEnumerator SkillAuto()
    {
        ColorChange();
        AniPlay();

        while (AutoOnOff)
        {
            Debug.Log("자동 액티브 사용 활성화");

            for (int i = 0; i < ActiveSkill.Count; i++)
            {
                if (ActiveSkill[i].isActived == true)
                {
                    Debug.Log($"{i}번째 스킬 실행중...");
                    ActiveSkill[i].Activate();
                }
            }
            // 자동 액티브 사용 방식 : 프레임 하나 당 실행하여, 각 스킬의 쿨타임이 끝날 때 마다 바로 실행될 수 있도록 함
            yield return new WaitForFixedUpdate();
        }

        while (AutoOnOff == false)
        {
            Debug.Log("자동 액티브 사용 비활성화");
            yield break;
        }
    }

    // ========

    // 버튼 색상 변경으로 활성화/비활성화 육안으로 확인할 수 있는 함수
    // 활성화 = 초록색 / 비활성화 = 빨간색
    void ColorChange()
    {
        ColorBlock colorBlock = ActiveAuto.colors;

        if (AutoOnOff == false)
        {
            colorBlock.selectedColor = new Color32(185, 0, 25, 255);
            colorBlock.highlightedColor = colorBlock.selectedColor;
            colorBlock.normalColor = colorBlock.selectedColor;

            AutoOnText.gameObject.SetActive(false);
            AutoOffText.gameObject.SetActive(true);
        }

        else if (AutoOnOff)
        {
            colorBlock.selectedColor = new Color32(85, 210, 0, 255);
            colorBlock.highlightedColor = colorBlock.selectedColor;
            colorBlock.normalColor = colorBlock.selectedColor;

            AutoOnText.gameObject.SetActive(true);
            AutoOffText.gameObject.SetActive(false);
        }

        ActiveAuto.colors = colorBlock;
    }
}
