using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    // 몬스터의 화면 진입 여부 (화면 안팎을 구분하는 콜라이더와의 연결이 필요해보인다)
    [SerializeField] bool isInside;
    public bool IsInside { get { return isInside; } set { isInside = value; } }
     
    // 몬스터의 체력  (몬스터는 체력을 회복하지 않으므로, 최대 체력 값은 필요하지 않을 것으로 판단함)
    [SerializeField] float monsterHP;
    public float MonsterHP { get { return monsterHP; } set { monsterHP = value; } }

    // 몬스터 이동 속도
    [SerializeField] float monsterMoveSpeed;
    public float MonsterMoveSpeed { get { return monsterMoveSpeed; } set { monsterMoveSpeed = value; } }

    // 몬스터가 떨어트리는 재화 갯수
    [SerializeField] int dropGold;
    public int DropGold { get { return dropGold; } set { dropGold = value; } }



    [Header("Range")]
    // 몬스터가 플레이어를 공격하는 최소 거리 설정. 해당 거리 내로 들어오면 몬스터는 원거리 공격을 시행한다.
    [SerializeField] int attackRange;
    public int AttackRange { get { return attackRange; } set { attackRange = value; } }
}
