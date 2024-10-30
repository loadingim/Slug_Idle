using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataModel : MonoBehaviour
{
    #region Data Variable
    [Header("플레이어 스탯 설정")]
    [Tooltip("플레이어 체력")]
    [SerializeField] private int health;
    public int Health
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
                health = value;
            }
            OnHealthChanged?.Invoke(health);
        }
    }
    public UnityAction<int> OnHealthChanged;

    [Tooltip("플레이어 체력재생")]
    [SerializeField] private int healthRegen;
    public int HealthRegen
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
    public UnityAction<int> OnHealthRegenChanged;

    [Tooltip("플레이어 기본 공격력")]
    [SerializeField] private float attack;
    public float Attack
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
            }
            OnAttackChanged?.Invoke(attack);
        }
    }
    public UnityAction<float> OnAttackChanged;

    [Tooltip("플레이어 터치 공격력")]
    [SerializeField] private float touchAttack;
    public float TouchAttack
    {
        get { return touchAttack; }
        set
        {
            // 터치 공격력의 예외상황 처리
            if (value < 0)
            {
                touchAttack = 0;
            }
            else
            {
                touchAttack = value;
            }
            OnTouchAttackChanged?.Invoke(touchAttack);
        }
    }
    public UnityAction<float> OnTouchAttackChanged;

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

    [Header("플레이어 보유 재화")]
    [Tooltip("플레이어가 보유한 기본 재화")]
    [SerializeField] private int money;
    public int Money
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
    public UnityAction<int> OnMoneyChanged;

    [Tooltip("플레이어가 보유한 동료 슬러그 강화 재화 (편의상, 크리스탈로 명칭)")]
    [SerializeField] private int crystal;
    public int Crystal
    {
        get { return crystal; }
        set
        {
            // 크리스탈의 예외상황 처리
            if (value < 0)
            {
                crystal = 0;
            }
            else
            {
                crystal = value;
            }
            OnCrystalChanged?.Invoke(crystal);
        }
    }
    public UnityAction<int> OnCrystalChanged;

    [Header("기타 스탯 설정")]
    [Tooltip("머지 공격력 수치")]
    [SerializeField] private float mergeAttack;
    public float MergeAttack
    {
        get { return mergeAttack; }
        set
        {
            // 머지 공격력의 예외상황 처리
            if (value < 0)
            {
                mergeAttack = 0;
            }
            else
            {
                mergeAttack = value;
            }
            OnMergeAttackChanged?.Invoke(mergeAttack);
        }
    }
    public UnityAction<float> OnMergeAttackChanged;

    [Tooltip("무기 공격력 수치")]
    [SerializeField] private float weaponAttack;
    public float WeaponAttack
    {
        get { return weaponAttack; }
        set
        {
            // 무기 공격력의 예외상황 처리
            if (value < 0)
            {
                weaponAttack = 0;
            }
            else
            {
                weaponAttack = value;
            }
            OnWeaponAttackChanged?.Invoke(weaponAttack);
        }
    }
    public UnityAction<float> OnWeaponAttackChanged;

    [Tooltip("스킬 공격력 수치")]
    [SerializeField] private float skillAttack;
    public float SkillAttack
    {
        get { return skillAttack; }
        set
        {
            // 스킬 공격력의 예외상황 처리
            if (value < 0)
            {
                skillAttack = 0;
            }
            else
            {
                skillAttack = value;
            }
            OnSkillAttackChanged?.Invoke(skillAttack);
        }
    }
    public UnityAction<float> OnSkillAttackChanged;


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
    }
}
