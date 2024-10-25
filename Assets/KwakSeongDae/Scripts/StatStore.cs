using System;
using UnityEngine;

public enum PlayerData
{
    Health, Attack, AttackSpeed, Money, Size
}

public class StatStore : MonoBehaviour
{
    [Serializable]
    public struct StatChangeAmount<T>
    {
        public T value;
        public int cost;
    }

    [Header("Stat Change Amount Setting")]
    public StatChangeAmount<int> health;
    public StatChangeAmount<float> attack;
    public StatChangeAmount<float> attackSpeed;

    private PlayerDataModel playerDataModel;

    private void Start()
    {
        playerDataModel = PlayerDataModel.Instance;
    }

    /// <summary>
    /// 체력 스탯이 1씩 상승하는 함수
    /// </summary>
    public void HealthStatUp()
    {
        if (playerDataModel == null)
        {
            Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 체크
        if (playerDataModel.Money - health.cost < 0) return;
        playerDataModel.Money -= health.cost;
        playerDataModel.Health += health.value;
    }

    /// <summary>
    /// 공격력 스탯이 1씩 상승하는 함수
    /// </summary>
    public void AttackStatUp()
    {
        if (playerDataModel == null)
        {
            Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 체크
        if (playerDataModel.Money - attack.cost < 0) return;
        playerDataModel.Money -= attack.cost;
        playerDataModel.Attack += attack.value;
    }

    /// <summary>
    /// 공격속도 스탯이 1씩 상승하는 함수
    /// </summary>
    public void AttackSpeeedStatUp()
    {
        if (playerDataModel == null)
        {
            Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
        // 구매 가능 여부 체크
        if (playerDataModel.Money - attackSpeed.cost < 0) return;
        playerDataModel.Money -= attackSpeed.cost;
        playerDataModel.AttackSpeed += attackSpeed.value;
    }
}
