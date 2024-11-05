using System;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStatStoreData
{
    Health,HealthRegen, Attack, AttackSpeed
}

public class StatStore : MonoBehaviour
{
    /// <summary>
    /// 스탯 증가량과 스탯 구매 비용, 스탯 업 버튼에 대해 정의된 자료형 타입
    /// </summary>
    [Serializable]
    public struct FloatStatChangeAmount
    {
        public float upValue;
        public long curCost;
        public Button buyButton;
        public Image inActiveImage;
    }
    [Serializable]
    public struct DemicalStatChangeAmount
    {
        public long upValue;
        public long curCost;
        public Button buyButton;
        public Image inActiveImage;
    }

    [Header("스탯 설정")]
    public DemicalStatChangeAmount health;
    public DemicalStatChangeAmount healthRegen;
    public DemicalStatChangeAmount attack;
    public FloatStatChangeAmount attackSpeed;

    [Header("스탯 상점 CSV 관련 설정")]
    [SerializeField] int attackMinIndex;
    [SerializeField] int attackMaxIndex;
    [SerializeField] int attackSpeedMinIndex;
    [SerializeField] int attackSpeedMaxIndex;
    [SerializeField] int healthMinIndex;
    [SerializeField] int healthMaxIndex;
    [SerializeField] int healthRegenMinIndex;
    [SerializeField] int healthRegenMaxIndex; 

    // 해당 변수가 준비된 경우에만, 스탯 관련 기능 활성화
    private bool isInit;
    private void OnEnable()
    {
        isInit = true;
    }
    private void OnDisable()
    {
        PlayerDataModel.Instance.OnMoneyChanged -= UpdateHealthBuyState;
        PlayerDataModel.Instance.OnMoneyChanged -= UpdateHealthRegenBuyState;
        PlayerDataModel.Instance.OnMoneyChanged -= UpdateAttackBuyState;
        PlayerDataModel.Instance.OnMoneyChanged -= UpdateAttackSpeedBuyState;
    }

    
    private void Update()
    {
        if (StoreCSV.Instance == null || PlayerDataModel.Instance == null) return;

        // CSV 데이터 다운될 때, 초기화 진행
        if (StoreCSV.Instance.downloadCheck && isInit)
        {
            if (PlayerDataModel.Instance == null) return;
            // 소지금 변화에 따라 체력 버튼 상태 업데이트
            PlayerDataModel.Instance.OnMoneyChanged += UpdateHealthBuyState;
            // 소지금 변화에 따라 체력 재생 버튼 상태 업데이트
            PlayerDataModel.Instance.OnMoneyChanged += UpdateHealthRegenBuyState;
            // 소지금 변화에 따라 공격력 버튼 상태 업데이트
            PlayerDataModel.Instance.OnMoneyChanged += UpdateAttackBuyState;
            // 소지금 변화에 따라 공격속도 버튼 상태 업데이트
            PlayerDataModel.Instance.OnMoneyChanged += UpdateAttackSpeedBuyState;

            // 초기 업데이트 진행
            UpdateHealthBuyState(PlayerDataModel.Instance.Money);
            UpdateHealthRegenBuyState(PlayerDataModel.Instance.Money);
            UpdateAttackBuyState(PlayerDataModel.Instance.Money);
            UpdateAttackSpeedBuyState(PlayerDataModel.Instance.Money);

            isInit = false;
        }
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
        if (health.curCost < 0 || PlayerDataModel.Instance.Money - health.curCost < 0) return;
        PlayerDataModel.Instance.Money -= health.curCost;

        // 레벨업에 따른 상태 갱신
        UpdateStatOnLevelUp(PlayerStatStoreData.Health);
    }
    /// <summary>
    /// 소지금 업데이트 시에 버튼의 활성화 여부 판단 함수
    /// </summary>
    void UpdateHealthBuyState(long newMoney)
    {
        // 해당 버튼 설정아 안되있는 경우 예외처리
        if (health.buyButton == null) return;

        // 구매 가능 여부에 따라 버튼 비활성화
        if (health.curCost < 0 || newMoney - health.curCost < 0)
        {
            health.buyButton.interactable = false;
            health.inActiveImage.gameObject.SetActive(true);
        }
        else
        {
            health.buyButton.interactable = true;
            health.inActiveImage.gameObject.SetActive(false);
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
        if (StoreCSV.Instance.downloadCheck == false)
        {
            Debug.Log("스탯 상점 관련 데이터가 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 다시 체크
        if (healthRegen.curCost < 0 || PlayerDataModel.Instance.Money - healthRegen.curCost < 0) return;
        PlayerDataModel.Instance.Money -= healthRegen.curCost;
        PlayerDataModel.Instance.HealthRegen += healthRegen.upValue;

        // 레벨업에 따른 상태 갱신
        UpdateStatOnLevelUp(PlayerStatStoreData.HealthRegen);
    }
    /// <summary>
    /// 소지금 업데이트 시에 버튼의 활성화 여부 판단 함수
    /// </summary>
    void UpdateHealthRegenBuyState(long newMoney)
    {
        // 해당 버튼 설정아 안되있는 경우 예외처리
        if (healthRegen.buyButton == null) return;

        // 구매 가능 여부에 따라 버튼 비활성화
        if (healthRegen.curCost < 0 || newMoney - healthRegen.curCost < 0)
        {
            healthRegen.buyButton.interactable = false;
            healthRegen.inActiveImage.gameObject.SetActive(true);
        }
        else
        {
            healthRegen.buyButton.interactable = true;
            healthRegen.inActiveImage.gameObject.SetActive(false);
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
        if (StoreCSV.Instance.downloadCheck == false)
        {
            Debug.Log("스탯 상점 관련 데이터가 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 다시 체크
        if (attack.curCost < 0 || PlayerDataModel.Instance.Money - attack.curCost < 0) return;
        PlayerDataModel.Instance.Money -= attack.curCost;

        // 레벨업 상태 갱신
        UpdateStatOnLevelUp(PlayerStatStoreData.Attack);
    }
    /// <summary>
    /// 소지금 업데이트 시에 버튼의 활성화 여부 판단 함수
    /// </summary>
    void UpdateAttackBuyState(long newMoney)
    {
        // 해당 버튼 설정아 안되있는 경우 예외처리
        if (attack.buyButton == null) return;
        // 구매 가능 여부에 따라 버튼 비활성화
        if (attack.curCost < 0 || newMoney - attack.curCost < 0)
        {
            attack.buyButton.interactable = false;
            attack.inActiveImage.gameObject.SetActive(true);
        }
        else
        {
            attack.buyButton.interactable = true;
            attack.inActiveImage.gameObject.SetActive(false);
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
        if (StoreCSV.Instance.downloadCheck == false)
        {
            Debug.Log("스탯 상점 관련 데이터가 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 다시 체크
        if ( attackSpeed.curCost < 0 || PlayerDataModel.Instance.Money - attackSpeed.curCost < 0) return;
        PlayerDataModel.Instance.Money -= attackSpeed.curCost;

        // 레벨업에 따른 상태 갱신
        UpdateStatOnLevelUp(PlayerStatStoreData.AttackSpeed);
    }
    /// <summary>
    /// 소지금 업데이트 시에 버튼의 활성화 여부 판단 함수
    /// </summary>
    void UpdateAttackSpeedBuyState(long newMoney)
    {
        // 해당 버튼 설정아 안되있는 경우 예외처리
        if (attackSpeed.buyButton == null) return;

        // 구매 가능 여부에 따라 버튼 비활성화
        if (attackSpeed.curCost < 0 || newMoney - attackSpeed.curCost < 0)
        {
            attackSpeed.buyButton.interactable = false;
            attackSpeed.inActiveImage.gameObject.SetActive(true);
        }
        else
        {
            attackSpeed.buyButton.interactable = true;
            attackSpeed.inActiveImage.gameObject.SetActive(false);
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
                PlayerDataModel.Instance.HealthLevel = UpdateDemicalStat(PlayerDataModel.Instance.HealthLevel, healthMinIndex, healthMaxIndex,ref health);
                break;
            case PlayerStatStoreData.HealthRegen:
                PlayerDataModel.Instance.HealthRegenLevel = UpdateDemicalStat(PlayerDataModel.Instance.HealthRegenLevel, healthRegenMinIndex, healthRegenMaxIndex,ref healthRegen);
                break;
            case PlayerStatStoreData.Attack:
                PlayerDataModel.Instance.AttackLevel = UpdateDemicalStat(PlayerDataModel.Instance.AttackLevel, attackMinIndex, attackMaxIndex,ref attack);
                break;
            case PlayerStatStoreData.AttackSpeed:
                PlayerDataModel.Instance.AttackSpeedLevel = UpdateFloatStat(PlayerDataModel.Instance.AttackSpeedLevel, attackSpeedMinIndex, attackSpeedMaxIndex,ref attackSpeed);
                break;
        }
    }
    int UpdateDemicalStat(int level, int minIndex, int maxIndex, ref DemicalStatChangeAmount stat)
    {
        int nextLevel = level + 1;
        int curIndex = minIndex + level - 1;
        // 레벨이 이미 최대 레벨이면 현재 레벨을 반환
        if (curIndex + 1 > maxIndex)
        {
            stat.upValue = -1;
            stat.curCost = -1;
            return level;
        }
        else
        {
            var store = StoreCSV.Instance.Store;
            // 현재 스탯과 다음 스탯과의 증가량 계산
            long curUpStat = (long)(store[curIndex].StatusStore_satatusNum * Mathf.Pow(10, store[curIndex].StatusStore_satatusUnit));
            long nxtUpStat = (long)(store[curIndex+1].StatusStore_satatusNum * Mathf.Pow(10, store[curIndex+1].StatusStore_satatusUnit));
            long nxtPriceGold = (long)(store[curIndex].StatusStore_priceGoldNum * Mathf.Pow(10, store[curIndex].StatusStore_priceGoldUnit));

            stat.upValue = nxtUpStat - curUpStat;
            stat.curCost = nxtPriceGold;
            return nextLevel;
        }
    }

    int UpdateFloatStat(int level, int minIndex, int maxIndex, ref FloatStatChangeAmount stat)
    {
        int nextLevel = level + 1;
        int currentIndex = minIndex + nextLevel - 1;
        // 레벨이 이미 최대 레벨이면 현재 레벨을 반환
        if (currentIndex + 1 > maxIndex)
        {
            stat.upValue = -1;
            stat.curCost = -1;
            return nextLevel;
        }
        else
        {
            var store = StoreCSV.Instance.Store;
            // 현재 스탯과 다음 스탯과의 증가량 계산
            float curUpStat = store[currentIndex].StatusStore_satatusNum * Mathf.Pow(10, store[currentIndex].StatusStore_satatusUnit);
            float nxtUpStat = store[currentIndex + 1].StatusStore_satatusNum * Mathf.Pow(10, store[currentIndex + 1].StatusStore_satatusUnit);
            long nxtPriceGold = (long)(store[currentIndex].StatusStore_priceGoldNum * Mathf.Pow(10, store[currentIndex].StatusStore_priceGoldUnit));

            stat.upValue = nxtUpStat - curUpStat;
            stat.curCost = nxtPriceGold;
            return nextLevel;
        }
    }
}
