using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public enum MonsterState { Move, Attack, Dead, Size }
    [SerializeField] MonsterState CurMonsterState = MonsterState.Move;

    [Header("Model")]
    [SerializeField] MonsterModel monsterModel;

    [Header("Player")]
    [SerializeField] GameObject Player;               // 플레이어 오브젝트
    [SerializeField] PlayerDataModel PlayerDataModel; // 플레이어 모델 참조

    [Header("Bullet")]
    [SerializeField] float CoolTime;            // 총알 발사에 걸리는 쿨타임
    [SerializeField] GameObject MonsterBullet;  // 몬스터 총알 오브젝트 (프리팹 자체 참조)
    [SerializeField] Transform muzzlePoint;     // 몬스터의 총알이 나가는 기준점이 될 오브젝트
    private bool isAttacked;                    // 공격 상태 판단 유무

    // 추후 플레이어 오브젝트에서 model을 GetComponent하여 참조 후, 재화 추가와 데미지 값을 입력받을 예정 
    // 추후 스테이지 화면 구성 시, 화면에 화면 안밖을 구분하는 콜라이더를 참조하여 피격 여부를 확인할 예정

    #region State 클래스 선언
    private BaseState[] States = new BaseState[(int)MonsterState.Size];
    [SerializeField] MoveState moveState;
    [SerializeField] AttackState attackState;
    [SerializeField] DeadState deadState;
    #endregion

    [Header("Animation")]
    #region 애니메이션 Hash 선언
    [SerializeField] Animator monsterAnimator;
    private int curAniHash;
    private static int monsterMoveHash = Animator.StringToHash("MonsterMove");
    private static int monsterAttackHash = Animator.StringToHash("MonsterAttack");
    private static int monsterDeadHash = Animator.StringToHash("MonsterDead");
    #endregion


    private void Awake()
    {
        // 생성과 동시에 플레이어 오브젝트를 자동으로 참조한다.
        Player = GameObject.FindGameObjectWithTag("Player");
        // 플레이어 모델 자동참조
        PlayerDataModel = Player.GetComponent<PlayerDataModel>();
        // 그 외 자동 참조
        monsterModel = GetComponent<MonsterModel>();

        States[(int)MonsterState.Move] = moveState;
        States[(int)MonsterState.Attack] = attackState;
        States[(int)MonsterState.Dead] = deadState;
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
            if (Monster.isAttacked == true) { Monster.isAttacked = false; }

            // 몬스터는 생성 시 곧바로 앞을 향해 전진한다.
            Monster.AnimatorPlay();
            Monster.transform.position = Vector2.MoveTowards(Monster.transform.position, Monster.Player.transform.position, Model.MonsterMoveSpeed * Time.deltaTime);

            // 추후 if문을 통해, 특정 콜라이더 안(=화면 안)으로 진입하였을 경우
            // isDamaged를 on하여 총알과 상호작용 하게 할 수 있을 듯 싶다


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

        private Rigidbody2D rigid;
        private Transform TargetPlayer;

        public override void Update()
        {
            if (Monster.isAttacked == false)
            {
                Monster.isAttacked = true;
                Monster.shot();
            }

            // 다른 상태로 전환
            if (Model.MonsterHP < 0.01f) { Monster.ChangeState(MonsterState.Dead); }
        }
    }

    #region 발사 쿨타임 코루틴
    void shot()
    {
        StartCoroutine(WaitingShot());
    }

    IEnumerator WaitingShot()
    {
        while (isAttacked)
        {
            Debug.Log("사격 쿨타임 적용됨");
            AnimatorPlay();
            GameObject bullet = Instantiate(MonsterBullet, muzzlePoint.position, muzzlePoint.rotation);

            yield return new WaitForSeconds(CoolTime);     
        }
    }
    #endregion

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
            Monster.AnimatorPlay();
            Destroy(Monster.gameObject); // 몬스터 자체를 오브젝트 풀 패턴을 보관하고 있어도 좋을듯
            // 코인이 UI를 향해 빨려가는 애니메이션 재생
            Monster.PlayerDataModel.Money += Model.DropGold;
        }
    }

    //====================================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 콜라이더에 닿은 게임 오브젝트가 플레이어의 총알이었을 때,
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("몬스터가 플레이어 총알에 닿음");
            monsterModel.MonsterHP -= PlayerDataModel.Attack;
        }
    }

    // ==============================================

    void AnimatorPlay()
    {
        int checkAniHash;

        // Dead 상태
        if (monsterModel.MonsterHP < 0.01f) { checkAniHash = monsterDeadHash; }
        // Attack 상태
        else if (Vector2.Distance(transform.position, Player.transform.position) < monsterModel.AttackRange) { checkAniHash = monsterAttackHash; }
        // Move 상태
        else { checkAniHash = monsterMoveHash; }


        if (curAniHash != checkAniHash)
        {
            curAniHash = checkAniHash;
            monsterAnimator.Play(curAniHash);
        }
    }
}
