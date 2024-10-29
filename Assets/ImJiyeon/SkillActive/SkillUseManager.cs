using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUseManager : MonoBehaviour
{
    [SerializeField] Button ActiveOne;
    [SerializeField] Button ActiveTwo;
    [SerializeField] Button ActiveThr;
    [SerializeField] Button ActiveFur;

    private void Awake()
    {
        // 추후 스킬 사용 UI를 생성하며 비활성화로 돌릴 예정
        ActiveOne.interactable = true;
        ActiveTwo.interactable = false;
        ActiveThr.interactable = false;
        ActiveFur.interactable = false;
    }
}
