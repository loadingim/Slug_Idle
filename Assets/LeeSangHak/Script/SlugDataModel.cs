using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlugDataModel : MonoBehaviour
{
    #region Data Variable
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
    #endregion

    private void OnEnable()
    {

    }
}
