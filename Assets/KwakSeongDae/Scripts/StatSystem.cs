using UnityEngine;
public enum Stat
{
    Health, Attack, AttackSpeed, Size
}

public class StatSystem : MonoBehaviour
{
    /// <summary>
    /// 스탯 업을 처리하는 스크립트
    /// 
    /// 각 스탯 업 수치는 일시적으로 1로 고정
    /// 
    /// </summary>
    private PlayerDataModel playerDataModel;

    private void Start()
    {
        playerDataModel = PlayerDataModel.Instance;
    }

    /// <summary>
    /// 선택한 스탯이 1씩 상승하는 함수
    /// https://github.com/NK-Studio/Visible-Enum-Attribute를 추가하지 않으면 Enum타입 매개변수를 이벤트 구독 시에, 인스펙터에서 찾을 수 없음
    /// </summary>
    //public void StatUp(Stat stat)
    //{
    //    if (playerDataModel == null)
    //    {
    //        Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
    //        return;
    //    }
    //    switch (stat)
    //    {
    //        case Stat.Health:
    //            playerDataModel.Health += 1;
    //            break;
    //        case Stat.Attack:
    //            playerDataModel.Attack += 1;
    //            break;
    //        case Stat.AttackSpeed:
    //            playerDataModel.AttackSpeed += 1;
    //            break;
    //        default:
    //            break;
    //    }
    //}

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
        playerDataModel.Health += 1;
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
        playerDataModel.Attack += 1;
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
        playerDataModel.AttackSpeed += 1;
    }
}
