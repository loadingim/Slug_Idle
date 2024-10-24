using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataModel : MonoBehaviour
{
    #region Data Variable
    /// <summary>
    /// 플레이어 체력
    /// </summary>
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

    /// <summary>
    /// 공격력 수치
    /// </summary>
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

    /// <summary>
    /// 공격속도 수치
    /// </summary>
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

    /// <summary>
    /// 보유한 자본
    /// </summary>
    [SerializeField] private int money;
    public int Money
    {
        get { return money; }
        set
        {
            // 자본의 예외상황 처리
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
