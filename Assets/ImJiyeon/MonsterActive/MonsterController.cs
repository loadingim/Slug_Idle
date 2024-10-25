using UnityEngine;

public class MonsterController : MonoBehaviour
{
    /*
        기능의 순서
        1. 몬스터 생성 (해당 기능은 다른 스크립트에서 진행 예정)
        2. 생성된 몬스터가 앞으로 움직임 (걸어다니는 몬스터가 더 많으므로 걸어다니는 몬스터를 디폴트로 설정)
        3. 앞으로 움직일 때, 공격 범위 내로 플레이어의 콜라이더가 들어올 때 공격을 개시
        4. 체력이 0이 될 경우, 개체는 삭제되며 동시에 재화를 드랍. (추후 플레이어 재화와 연동하여 등록된 값을 ++ 하는 식으로 구현을 예상하고 있음)

        화면 밖에 있는 몬스터는 트리거가 on이 되어 맞지 않도록 함
     */

    public enum MonsterState { Move, Attack, Dead, Size }
    [SerializeField] MonsterState CurMonsterState = MonsterState.Move;

    [Header("Model")]
    [SerializeField] MonsterModel monsterModel;

    [Header("Player")]
    [SerializeField] GameObject Player;         // 플레이어 오브젝트
    [SerializeField] GameObject PlayerBullet;   // 플레이어 총알 오브젝트 (프리팹 자체를 참조 예정)
    // 추후 플레이어 오브젝트에서 model을 GetComponent하여 참조 후, 재화 추가와 데미지 값을 입력받을 예정 


    #region State 클래스 선언
    private BaseState[] States = new BaseState[(int)MonsterState.Size];
    [SerializeField] MoveState moveState;
    [SerializeField] AttackState attackState;
    [SerializeField] DeadState deadState;
    #endregion

    #region 애니메이션 Hash 선언 (추후 추가 예정)


    #endregion


    private void Awake()
    {
        // 생성과 동시에 플레이어 오브젝트를 자동으로 참조한다.
        Player = GameObject.FindGameObjectWithTag("Player");

        States[(int)MonsterState.Move] = moveState;
        States[(int)MonsterState.Attack] = attackState;
        States[(int)MonsterState.Dead] = deadState;

        // 그 외 자동 참조
        monsterModel = GetComponent<MonsterModel>();
    }

    #region Start, OnDestroy, Update 함수
    private void Start()
    {
        States[(int)CurMonsterState].Enter();
    }

    private void OnDestroy()
    {
        States[(int)CurMonsterState].Exit();
    }

    private void Update()
    {
        States[(int)CurMonsterState].Update();
    }
    #endregion


    //====================================================

    public void ChangeState(MonsterState nextState)
    {
        States[(int)CurMonsterState].Exit();
        CurMonsterState = nextState;
        States[(int)CurMonsterState].Enter();
    }

    //====================================================

    [System.Serializable]
    private class MoveState : BaseState
    {
        [SerializeField] MonsterController Monster;
        [SerializeField] MonsterModel Model;

        public override void Update()
        {
            // Move 행동 구현
            // 몬스터는 생성 시 곧바로 앞을 향해 전진한다.
            Monster.transform.position = Vector2.MoveTowards(Monster.transform.position, Monster.transform.forward, Model.MonsterMoveSpeed * Time.deltaTime);

            // 추후 if문을 통해, 특정 콜라이더 안(=화면 안)으로 진입하였을 경우 isDamaged를 on하여 총알과 상호작용 하게 할 수 있을 듯 싶다

            // 다른 상태로 전환
            // 몬스터의 체력이 0 이하일 경우, 몬스터는 삭제된다.
            if (Model.MonsterHP < 0.01f) { Monster.ChangeState(MonsterState.Dead); }
            // 일정 거리에 플레이어가 존재할 경우, 몬스터는 공격을 개시한다.
            else if (Vector2.Distance(Monster.transform.position, Monster.Player.transform.position) < Model.AttackRange) { Monster.ChangeState(MonsterState.Attack); }
        }
    }

    //====================================================

    [System.Serializable]
    private class AttackState : BaseState
    {
        [SerializeField] MonsterController Monster;
        [SerializeField] MonsterModel Model;

        public override void Update()
        {
            // Attack 행동 구현
            // 기획팀에 문의 후 코드 구성 예정
            // 원거리 공격으로 알고있음

            // 다른 상태로 전환
            if (Model.MonsterHP < 0.01f) { Monster.ChangeState(MonsterState.Dead); }
        }
    }

    //====================================================

    [System.Serializable]
    private class DeadState : BaseState
    {
        [SerializeField] MonsterController Monster;
        [SerializeField] MonsterModel Model;

        public override void Update()
        {
            // Dead 행동 구현
            Debug.Log("몬스터 삭제됨");
            // 몬스터는 삭제된다. (혹은 오브젝트 풀 패턴 사용)
            Destroy(Monster.gameObject);
            // 이후 코인이 UI를 향해 빨려가는 애니메이션 재생
            // 마지막으로 플레이어의 재화를 보관하고 있는 자료형 += Model.DropGold;
        }
    }

    //====================================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerBullet = collision.gameObject;

        // 콜라이더에 닿은 게임 오브젝트가 플레이어의 총알이었을 때,
        if (collision.gameObject == PlayerBullet)
        {
            Debug.Log("총알에 닿음");
            float PlayerDamage = 10f; // 임시 데미지값
            monsterModel.MonsterHP -= PlayerDamage;
        }
    }
}
