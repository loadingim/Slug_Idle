using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static StatStore;

public class PlayerDataModel : MonoBehaviour
{
    #region Data Variable
    [Header("플레이어 스탯 설정")]
    #region 체력 관련
    [Header("플레이어 체력")]
    [Tooltip("플레이어 현재 체력")]
    [SerializeField] private long health;
    public long Health
    {
        get { return health; }
        set
        {
            // 체력의 예외상황 처리
            if (value < 0)
            {
                health = 0;
            }
            else
            {
                if (value > MaxHealth)
                {
                    health = MaxHealth;
                }
                else health = value;
            }
            OnHealthChanged?.Invoke(health);
        }
    }
    public UnityAction<long> OnHealthChanged;

    [Tooltip("플레이어 최대 체력")]
    [SerializeField] private long maxHealth;
    public long MaxHealth
    {
        get { return maxHealth; }
        set
        {
            // 체력의 예외상황 처리
            if (value < 0)
            {
                maxHealth = 0;
            }
            else
            {
                maxHealth = value;
            }
            OnMaxHealthChanged?.Invoke(health);
        }
    }
    public UnityAction<long> OnMaxHealthChanged;

    [Tooltip("플레이어 체력 레벨")]
    [SerializeField] private int healthLevel;
    public int HealthLevel
    {
        get { return healthLevel; }
        set
        {
            // 체력 레벨의 예외상황 처리
            if (value < 0)
            {
                healthLevel = 0;
            }
            else
            {
                healthLevel = value;
                // 플레이어 체력 레벨에 비례해서 체력 증가량만큼 현재 체력, 최대 체력 증가
                if (StoreCSV.Instance != null &&  StoreCSV.Instance.downloadCheck == true)
                {
                    long stat = DemicalDataFromStoreCSV(healthLevel, healthMinIndex, healthMaxIndex);
                    // 최대 체력은 csv데이터로 갱신
                    long difHealth = stat - MaxHealth;
                    MaxHealth = stat;
                    Health += difHealth;
                }
                // 체력 레벨에 따라 CSV데이터를 참고해서 동기화 작업 필요
            }
            OnHealthLevelChanged?.Invoke(healthLevel);
        }
    }
    public UnityAction<int> OnHealthLevelChanged;

    [Header("플레이어 체력재생")]
    [Tooltip("플레이어 체력재생")]
    [SerializeField] private long healthRegen;
    public long HealthRegen
    {
        get { return healthRegen; }
        set
        {
            // 체력 재생의 예외상황 처리
            if (value < 0)
            {
                healthRegen = 0;
            }
            else
            {
                healthRegen = value;
            }
            OnHealthRegenChanged?.Invoke(healthRegen);
        }
    }
    public UnityAction<long> OnHealthRegenChanged;

    [Tooltip("플레이어 체력재생 레벨")]
    [SerializeField] private int healthRegenLevel;
    public int HealthRegenLevel
    {
        get { return healthRegenLevel; }
        set
        {
            // 체력 재생 레벨의 예외상황 처리
            if (value < 0)
            {
                healthRegenLevel = 0;
            }
            else
            {
                healthRegenLevel = value;
                if (StoreCSV.Instance != null && StoreCSV.Instance.downloadCheck == true)
                {
                    long stat = DemicalDataFromStoreCSV(healthRegenLevel, healthRegenMinIndex, healthRegenMaxIndex);
                    HealthRegen = stat;
                }
            }
            OnHealthRegenLevelChanged?.Invoke(healthRegenLevel);
        }
    }
    public UnityAction<int> OnHealthRegenLevelChanged;
    #endregion

    #region 공격 관련
    [Header("플레이어 기본 공격력")]
    [Tooltip("플레이어 기본 공격력")]
    [SerializeField] private long attack;
    public long Attack
    {
        get { return attack; }
        set
        {
            // 공격력의 예외상황 처리
            if (value < 0)
            {
                attack = 0;
            }
            else
            {
                attack = value;
                // 공격력 계산 시스템
                var attackStat = DemicalDataFromStoreCSV(attackLevel, attackMinIndex, attackMaxIndex);
                attack = attackStat + (long)BulletAttack;
            }
            OnAttackChanged?.Invoke(attack);
        }
    }

    public UnityAction<long> OnAttackChanged;

    [Tooltip("플레이어 기본 공격력 레벨")]
    [SerializeField] private int attackLevel;
    public int AttackLevel
    {
        get { return attackLevel; }
        set
        {
            // 기본 공격력 레벨의 예외상황 처리
            if (value < 0)
            {
                attackLevel = 0;
            }
            else
            {
                attackLevel = value;
                // 공격력 갱신
                Attack = Attack;
            }
            OnAttackLevelChanged?.Invoke(attackLevel);
        }
    }
    public UnityAction<int> OnAttackLevelChanged;

    [Header("플레이어 기본 공격속도")]
    [Tooltip("플레이어 기본 공격속도")]
    [SerializeField] private float attackSpeed;
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            // 공격속도의 예외상황 처리
            if (value < 0)
            {
                attackSpeed = 0;
            }
            else
            {
                attackSpeed = value;
            }
            OnAttackSpeedChanged?.Invoke(attackSpeed);
        }
    }
    public UnityAction<float> OnAttackSpeedChanged;

    [Tooltip("플레이어 기본 공격속도 레벨")]
    [SerializeField] private int attackSpeedLevel;
    public int AttackSpeedLevel
    {
        get { return attackSpeedLevel; }
        set
        {
            // 기본 공격력 레벨의 예외상황 처리
            if (value < 0)
            {
                attackSpeedLevel = 0;
            }
            else
            {
                attackSpeedLevel = value;
                if (StoreCSV.Instance != null && StoreCSV.Instance.downloadCheck == true)
                {
                    float stat = FloatDataFromStoreCSV(attackSpeedLevel, attackSpeedMinIndex, attackSpeedMaxIndex);
                    AttackSpeed = stat;
                }
            }
            OnAttackSpeedLevelChanged?.Invoke(attackSpeedLevel);
        }
    }
    public UnityAction<int> OnAttackSpeedLevelChanged;

    [Header("플레이어 총알 공격력")]
    [Tooltip("플레이어 기본 공격속도")]
    [SerializeField] private float bulletAttack;
    public float BulletAttack
    {
        get { return bulletAttack; }
        set
        {
            // 공격속도의 예외상황 처리
            if (value < 0)
            {
                bulletAttack = 0;
            }
            else
            {
                bulletAttack = value;
            }
            OnBulletAttackChanged?.Invoke(attackSpeed);
        }
    }
    public UnityAction<float> OnBulletAttackChanged;

    [Tooltip("플레이어 총알 최고 레벨")]
    [SerializeField] private int bulletLevel;
    public int BulletLevel
    {
        get { return bulletLevel; }
        set
        {
            // 기본 공격력 레벨의 예외상황 처리
            if (value < 0)
            {
                bulletLevel = 0;
            }
            else
            {
                bulletLevel = value;
                if (StoreCSV.Instance != null && StoreCSV.Instance.downloadCheck == true)
                {
                    float stat = FloatDataFromStoreCSV(bulletLevel, attackSpeedMinIndex, attackSpeedMaxIndex);
                    AttackSpeed = stat;
                }
                // 공격력 갱신
                Attack = Attack;
            }
            OnBulletLevelChanged?.Invoke(bulletLevel);
        }
    }
    public UnityAction<int> OnBulletLevelChanged;
    #endregion

    #region 재화 관련
    [Header("플레이어 보유 재화")]
    [Tooltip("플레이어가 보유한 기본 재화")]
    [SerializeField] private long money;
    public long Money
    {
        get { return money; }
        set
        {
            // 기본 재화의 예외상황 처리
            if (value < 0)
            {
                money = 0;
            }
            else
            {
                money = value;
            }
            OnMoneyChanged?.Invoke(money);
        }
    }
    public UnityAction<long> OnMoneyChanged;

    [Tooltip("플레이어가 보유한 동료 슬러그 강화 재화 (편의상, 크리스탈로 명칭)")]
    [SerializeField] private long jewel;
    public long Jewel
    {
        get { return jewel; }
        set
        {
            // 크리스탈의 예외상황 처리
            if (value < 0)
            {
                jewel = 0;
            }
            else
            {
                jewel = value;
            }
            OnJewelChanged?.Invoke(jewel);
        }
    }
    public UnityAction<long> OnJewelChanged;

    #endregion

    /// <summary>
    /// 스킬 해금 여부 판단하기 위한 플래그 열거형 타입 선언
    /// </summary>
    [Flags]
    public enum SkillType
    {
        None = 0,
        FirstSkill = 1 << 0,
        SecondSkill = 1 << 1,
        ThirdSkill = 1 << 2,
        FourthSkill = 1 << 3,
    }

    /// <summary>
    /// 무기 해금 여부 판단하기 위한 플래그 열거형 타입 선언
    /// </summary>
    [Flags]
    public enum WeaponType
    {
        None = 0,
        FirstWeapon = 1 << 0,
        SecondWeapon = 1 << 1,
        ThirdWeapon = 1 << 2,
        FourthWeapon = 1 << 3,
    }


    [Header("인벤토리 아이템 관련 설정")]
    [Tooltip("스킬 해금 여부 플래그")]
    [SerializeField] private SkillType canUseSkill;
    public SkillType CanUseSkill
    {
        get { return canUseSkill; }
        set { canUseSkill = value; OnCanUseSkillChanged?.Invoke(canUseSkill); }

    }
    public UnityAction<SkillType> OnCanUseSkillChanged;

    [Tooltip("무기 해금 여부 플래그")]
    [SerializeField] private WeaponType canUseWeapon;
    public WeaponType CanUseWeapon
    {
        get { return canUseWeapon; }
        set { canUseWeapon = value; OnCanUseWeaponChanged?.Invoke(canUseWeapon); }

    }

    public UnityAction<WeaponType> OnCanUseWeaponChanged;
    #endregion

    // CSV로부터 받아올 데이터 인덱스 정보
    [Header("CSV 관련 설정")]
    [SerializeField] int attackMinIndex;
    [SerializeField] int attackMaxIndex;
    [SerializeField] int attackSpeedMinIndex;
    [SerializeField] int attackSpeedMaxIndex;
    [SerializeField] int healthMinIndex;
    [SerializeField] int healthMaxIndex;
    [SerializeField] int healthRegenMinIndex;
    [SerializeField] int healthRegenMaxIndex;
    [SerializeField] int bulletMinIndex;
    [SerializeField] int bulletMaxIndex;

    // 싱글톤
    public static PlayerDataModel Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeValue();
    }

    long DemicalDataFromStoreCSV(int level, int minIndex, int maxIndex)
    {
        if (StoreCSV.Instance.downloadCheck == false)
        {
            print("CSV 로드 안됨");
            return -1;
        }
        int currentIndex = minIndex + level - 1;
        if (currentIndex <= maxIndex && currentIndex >= minIndex)
        {
            var store = StoreCSV.Instance.Store;
            return (long)(store[currentIndex].StatusStore_satatusNum * Mathf.Pow(10, store[currentIndex].StatusStore_satatusUnit));
        }
        print($"비정상적인 인덱스 level: {level} minIndex: {minIndex} maxIndex: {maxIndex}");

        return -1;
    }

    float FloatDataFromStoreCSV(int level, int minIndex, int maxIndex)
    {
        if (StoreCSV.Instance.downloadCheck == false)
        {
            print("CSV 로드 안됨");
            return -1;
        }
        int currentIndex = minIndex + level - 1;

        if (currentIndex <= maxIndex && currentIndex >= minIndex)
        {
            var store = StoreCSV.Instance.Store;
            return store[currentIndex].StatusStore_satatusNum * Mathf.Pow(10, store[currentIndex].StatusStore_satatusUnit);
        }
        print($"비정상적인 인덱스 level: {level} minIndex: {minIndex} maxIndex: {maxIndex}");
        return -1;
    }

    float FloatDataFromBulletCSV(int level, int minIndex, int maxIndex)
    {
        if (BulletCSV.Instance == null || BulletCSV.Instance.downloadCheck == false)
        {
            print("CSV 로드 안됨");
            return -1;
        }

        int currentIndex = minIndex + level - 1;
        if (currentIndex <= maxIndex && currentIndex >= minIndex)
        {
            var bullet = BulletCSV.Instance.Bullet;

            return bullet[currentIndex].Bullet_num * Mathf.Pow(10, bullet[currentIndex].Bullet_unit);
        }
        print($"비정상적인 인덱스 level: {level} minIndex: {minIndex} maxIndex: {maxIndex}");
        return -1;
    }

    void InitializeValue()
    {
        healthLevel = 1;
        healthRegenLevel = 1;
        attackLevel = 1;
        attackSpeedLevel = 1;
        bulletLevel = 1;

        health = DemicalDataFromStoreCSV(healthLevel, healthMinIndex, healthMaxIndex);
        maxHealth = health;
        healthRegen = DemicalDataFromStoreCSV(healthRegenLevel, healthRegenLevel, healthRegenMaxIndex);
        attack = DemicalDataFromStoreCSV(attackLevel, attackMinIndex, attackMaxIndex);
        attackSpeed = DemicalDataFromStoreCSV(attackSpeedLevel, attackSpeedMinIndex, attackSpeedMaxIndex);
        bulletAttack = DemicalDataFromStoreCSV(bulletLevel, bulletMinIndex, bulletMaxIndex);

        OnHealthChanged?.Invoke(health);
        OnMaxHealthChanged?.Invoke(maxHealth);
        OnHealthRegenChanged?.Invoke(healthRegen);
        OnAttackChanged?.Invoke(attack);
        OnAttackSpeedChanged?.Invoke(attackSpeed);
        OnBulletAttackChanged?.Invoke(bulletAttack);
    }
}
