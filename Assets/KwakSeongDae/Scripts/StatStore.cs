using System;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStatStoreData
{
    Health,HealthRegen, Attack, TouchAttack, AttackSpeed
}

public class StatStore : MonoBehaviour
{
    /// <summary>
    /// 스탯 증가량과 스탯 구매 비용, 스탯 업 버튼에 대해 정의된 자료형 타입
    /// </summary>
    /// <typeparam name="T"> 스탯의 자료형과 일치 </typeparam>
    [Serializable]
    public struct StatChangeAmount<T>
    {
        public T upValue;
        public int curCost;
        public Button buyButton;
    }

    [Header("스탯 설정")]
    public StatChangeAmount<int> health;
    public StatChangeAmount<float> healthRegen;
    public StatChangeAmount<float> attack;
    public StatChangeAmount<float> touchAttack;
    public StatChangeAmount<float> attackSpeed;

    private void OnEnable()
    {
        if (PlayerDataModel.Instance == null) return;
        // 소지금 변화에 따라 체력 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateHealthBuyState;
        // 소지금 변화에 따라 체력 재생 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateHealthRegenBuyState;
        // 소지금 변화에 따라 공격력 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateAttackBuyState;
        // 소지금 변화에 따라 터치공격력 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateTouchAttackBuyState;
        // 소지금 변화에 따라 공격속도 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateAttackSpeedBuyState;
    }
    private void Start()
    {
        // 소지금 변화에 따라 체력 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateHealthBuyState;
        // 소지금 변화에 따라 체력 재생 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateHealthRegenBuyState;
        // 소지금 변화에 따라 공격력 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateAttackBuyState;
        // 소지금 변화에 따라 터치공격력 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateTouchAttackBuyState;
        // 소지금 변화에 따라 공격속도 버튼 상태 업데이트
        PlayerDataModel.Instance.OnMoneyChanged += UpdateAttackSpeedBuyState;
    }

    private void OnDisable()
    {
        PlayerDataModel.Instance.OnMoneyChanged -= UpdateHealthBuyState;
        PlayerDataModel.Instance.OnMoneyChanged -= UpdateHealthRegenBuyState;
        PlayerDataModel.Instance.OnMoneyChanged -= UpdateAttackBuyState;
        PlayerDataModel.Instance.OnMoneyChanged -= UpdateTouchAttackBuyState;
        PlayerDataModel.Instance.OnMoneyChanged -= UpdateAttackSpeedBuyState;
    }

    #region 체력 관련 기능
    /// <summary>
    /// 체력 스탯이 upValue씩 상승하는 함수
    /// </summary>
    public void HealthStatUp()
    {
        if (PlayerDataModel.Instance == null)
        {
            Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 다시 체크
        if (PlayerDataModel.Instance.Money - health.curCost < 0) return;
        PlayerDataModel.Instance.Money -= health.curCost;
        PlayerDataModel.Instance.Health += health.upValue;
        // 체력은 최대체력과 현재체력이 동시에 증가
        PlayerDataModel.Instance.MaxHealth += health.upValue;

        // 레벨업에 따른 상태 갱신
        UpdateStatOnLevelUp(PlayerStatStoreData.Health);
    }
    /// <summary>
    /// 소지금 업데이트 시에 버튼의 활성화 여부 판단 함수
    /// </summary>
    void UpdateHealthBuyState(int newMoney)
    {
        // 해당 버튼 설정아 안되있는 경우 예외처리
        if (health.buyButton == null) return;

        // 구매 가능 여부에 따라 버튼 비활성화
        if (newMoney - health.curCost < 0)
        {
            health.buyButton.interactable = false;
        }
        else
        {
            health.buyButton.interactable = true;
        }
    }
    #endregion

    #region 체력재생 관련 기능
    /// <summary>
    /// 체력재생 스탯이 upValue씩 상승하는 함수
    /// </summary>
    public void HealthRegenStatUp()
    {
        if (PlayerDataModel.Instance == null)
        {
            Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 다시 체크
        if (PlayerDataModel.Instance.Money - healthRegen.curCost < 0) return;
        PlayerDataModel.Instance.Money -= healthRegen.curCost;
        PlayerDataModel.Instance.HealthRegen += healthRegen.upValue;

        // 레벨업에 따른 상태 갱신
        UpdateStatOnLevelUp(PlayerStatStoreData.HealthRegen);
    }
    /// <summary>
    /// 소지금 업데이트 시에 버튼의 활성화 여부 판단 함수
    /// </summary>
    void UpdateHealthRegenBuyState(int newMoney)
    {
        // 해당 버튼 설정아 안되있는 경우 예외처리
        if (healthRegen.buyButton == null) return;

        // 구매 가능 여부에 따라 버튼 비활성화
        if (newMoney - healthRegen.curCost < 0)
        {
            healthRegen.buyButton.interactable = false;
        }
        else
        {
            healthRegen.buyButton.interactable = true;
        }
    }
    #endregion

    #region 공격력 관련 기능
    /// <summary>
    /// 공격력 스탯이 upValue씩 상승하는 함수
    /// </summary>
    public void AttackStatUp()
    {
        if (PlayerDataModel.Instance == null)
        {
            Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 다시 체크
        if (PlayerDataModel.Instance.Money - attack.curCost < 0) return;
        PlayerDataModel.Instance.Money -= attack.curCost;
        PlayerDataModel.Instance.Attack += attack.upValue;

        // 레벨업에 따른 상태 갱신
        UpdateStatOnLevelUp(PlayerStatStoreData.Attack);
    }
    /// <summary>
    /// 소지금 업데이트 시에 버튼의 활성화 여부 판단 함수
    /// </summary>
    void UpdateAttackBuyState(int newMoney)
    {
        // 해당 버튼 설정아 안되있는 경우 예외처리
        if (attack.buyButton == null) return;

        // 구매 가능 여부에 따라 버튼 비활성화
        if (newMoney - attack.curCost < 0)
        {
            attack.buyButton.interactable = false;
        }
        else
        {
            attack.buyButton.interactable = true;
        }
    }
    #endregion

    #region 터치공격력 관련 기능
    /// <summary>
    /// 공격력 스탯이 upValue씩 상승하는 함수
    /// </summary>
    public void TouchAttackStatUp()
    {
        if (PlayerDataModel.Instance == null)
        {
            Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 다시 체크
        if (PlayerDataModel.Instance.Money - touchAttack.curCost < 0) return;
        PlayerDataModel.Instance.Money -= touchAttack.curCost;
        PlayerDataModel.Instance.TouchAttack += touchAttack.upValue;

        // 레벨업에 따른 상태 갱신
        UpdateStatOnLevelUp(PlayerStatStoreData.TouchAttack);
    }
    /// <summary>
    /// 소지금 업데이트 시에 버튼의 활성화 여부 판단 함수
    /// </summary>
    void UpdateTouchAttackBuyState(int newMoney)
    {
        // 해당 버튼 설정아 안되있는 경우 예외처리
        if (touchAttack.buyButton == null) return;

        // 구매 가능 여부에 따라 버튼 비활성화
        if (newMoney - touchAttack.curCost < 0)
        {
            touchAttack.buyButton.interactable = false;
        }
        else
        {
            touchAttack.buyButton.interactable = true;
        }
    }
    #endregion

    #region 공격속도 관련 기능
    /// <summary>
    /// 공격속도 스탯이 upValue씩 상승하는 함수
    /// </summary>
    public void AttackSpeedStatUp()
    {
        if (PlayerDataModel.Instance == null)
        {
            Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 다시 체크
        if (PlayerDataModel.Instance.Money - attackSpeed.curCost < 0) return;
        PlayerDataModel.Instance.Money -= attackSpeed.curCost;
        PlayerDataModel.Instance.AttackSpeed += attackSpeed.upValue;

        // 레벨업에 따른 상태 갱신
        UpdateStatOnLevelUp(PlayerStatStoreData.AttackSpeed);
    }
    /// <summary>
    /// 소지금 업데이트 시에 버튼의 활성화 여부 판단 함수
    /// </summary>
    void UpdateAttackSpeedBuyState(int newMoney)
    {
        // 해당 버튼 설정아 안되있는 경우 예외처리
        if (attackSpeed.buyButton == null) return;

        // 구매 가능 여부에 따라 버튼 비활성화
        if (newMoney - attackSpeed.curCost < 0)
        {
            attackSpeed.buyButton.interactable = false;
        }
        else
        {
            attackSpeed.buyButton.interactable = true;
        }
    }
    #endregion

    /// <summary>
    /// 매개변수의 데이터 타입과 관련된 변수를 업데이트하는 함수
    /// 현재 레벨, 다음 레벨업 비용, 레벨업 시 증가량을 업데이트
    /// </summary>
    void UpdateStatOnLevelUp(PlayerStatStoreData dataType)
    {
        switch (dataType)
        {
            case PlayerStatStoreData.Health:
                var nextHealthLevel = ++PlayerDataModel.Instance.HealthLevel;
                
                // StatusStoreCSV가 완료되면 기능 추가

                // 다음 레벨의 존재 여부 파악

                // 다음 레벨이 존재할 경우, 해당 스탯 수치와 스테이터스 가격으로 업데이트
                
                break;
            case PlayerStatStoreData.HealthRegen:
                var nextHealthRegenLevel = ++PlayerDataModel.Instance.HealthRegenLevel;

                // StatusStoreCSV가 완료되면 기능 추가

                // 다음 레벨의 존재 여부 파악

                // 다음 레벨이 존재할 경우, 해당 스탯 수치와 스테이터스 가격으로 업데이트
                break;
            case PlayerStatStoreData.Attack:
                var nextAttackLevel = ++PlayerDataModel.Instance.AttackLevel;

                // StatusStoreCSV가 완료되면 기능 추가

                // 다음 레벨의 존재 여부 파악

                // 다음 레벨이 존재할 경우, 해당 스탯 수치와 스테이터스 가격으로 업데이트
                break;
            case PlayerStatStoreData.TouchAttack:
                var nextTouchAttackLevel = ++PlayerDataModel.Instance.TouchAttackLevel;

                // StatusStoreCSV가 완료되면 기능 추가

                // 다음 레벨의 존재 여부 파악

                // 다음 레벨이 존재할 경우, 해당 스탯 수치와 스테이터스 가격으로 업데이트
                break;
            case PlayerStatStoreData.AttackSpeed:
                var nextAttackSpeedLevel = ++PlayerDataModel.Instance.AttackSpeedLevel;

                // StatusStoreCSV가 완료되면 기능 추가

                // 다음 레벨의 존재 여부 파악

                // 다음 레벨이 존재할 경우, 해당 스탯 수치와 스테이터스 가격으로 업데이트
                break;
            default:
                break;
        }
    }
}
