using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 자원 UI와 PlayerDataModel의 데이터와 연동시키기 위한 스크립트
/// </summary>
public class ResourceUIController : MonoBehaviour
{
    [Header("자원 UI 기본 설정")]
    [Tooltip("사용자의 기본 재화를 표기하는 TextMeshUI")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [Tooltip("동료 및 슬러그 강화 시에 사용되는 재화를 표기하는 TextMeshUI")]
    [SerializeField] private TextMeshProUGUI jewelText;

    private void Start()
    {
        if (PlayerDataModel.Instance != null)
        {
            PlayerDataModel.Instance.OnMoneyChanged += UpdateMoney;
            PlayerDataModel.Instance.OnJewelChanged += UpdateJewel;

            moneyText?.SetText(PlayerDataModel.Instance.Money.ToString());
            jewelText?.SetText(PlayerDataModel.Instance.Money.ToString());
        }
    }

    private void OnEnable()
    {
        if (PlayerDataModel.Instance != null)
        {
            PlayerDataModel.Instance.OnMoneyChanged += UpdateMoney;
            PlayerDataModel.Instance.OnJewelChanged += UpdateJewel;
        }
    }

    private void OnDisable()
    {
        if (PlayerDataModel.Instance != null)
        {
            PlayerDataModel.Instance.OnMoneyChanged -= UpdateMoney;
            PlayerDataModel.Instance.OnJewelChanged -= UpdateJewel;
        }
    }

    void UpdateMoney(long newMoney)
    {
        moneyText?.SetText(newMoney.ToString());
    }

    void UpdateJewel(long newJewel)
    {
        jewelText?.SetText(newJewel.ToString());
    }
}
