using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillUseManager : MonoBehaviour
{
    [Header("Active Skills")]
    [SerializeField] Button[] ActiveSkill;

    [Header("Aoto")]
    [SerializeField] bool AutoOnOff;
    [SerializeField] Button ActiveAoto;


    private void Awake()
    {
        // 추후 스킬 사용 UI를 생성하며 비활성화로 돌릴 예정
        for (int i = 0; i < ActiveSkill.Length; i++)
        {
            ActiveSkill[i].interactable = true;
        }

        AutoOnOff = false;
        ColorChange();
    }


    // 클릭으로 오토버튼 활성화 - 비활성화 상태 전환 구현
    public void AutoClick()
    {
        // 활성화 - 비활성화
        if (AutoOnOff)
        {
            AutoOnOff = false;
        }
        // 비활성화 - 활성화
        else if (AutoOnOff == false)
        {
            AutoOnOff = true;
        }

        StartCoroutine(SkillAuto());
    }

    IEnumerator SkillAuto()
    {
        ColorChange();

        while (AutoOnOff)
        {
            Debug.Log("오토버튼 활성화중...");
            yield return new FixedUpdate();
        }

        while (AutoOnOff == false)
        {
            Debug.Log("오토버튼 비활성화");
            yield break;
        }
    }

    // ========

    // 버튼 색상 변경으로 활성화/비활성화 육안으로 확인할 수 있는 함수
    // 활성화 = 초록색 / 비활성화 = 빨간색
    void ColorChange()
    {
        ColorBlock colorBlock = ActiveAoto.colors;

        if (AutoOnOff == false)
        {
            colorBlock.selectedColor = Color.red;
            colorBlock.highlightedColor = colorBlock.selectedColor;
            colorBlock.normalColor = colorBlock.selectedColor;
        }
        else if (AutoOnOff)
        {
            colorBlock.selectedColor = Color.green;
            colorBlock.highlightedColor = colorBlock.selectedColor;
            colorBlock.normalColor = colorBlock.selectedColor;
        }

        ActiveAoto.colors = colorBlock;
    }
}
