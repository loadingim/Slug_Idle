using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
             private bool isAttacked;           // 공격 상태 판단 유무
    [SerializeField] GameObject MonsterBullet;  // 몬스터 총알 오브젝트 (프리팹 자체 참조)
             private GameObject bullet;
    [SerializeField] Transform muzzlePoint;     // 몬스터의 총알이 나가는 기준점이 될 오브젝트


    #region State 클래스 선언
    private BaseState[] States = new BaseState[(int)MonsterState.Size];
    [SerializeField] MoveState moveState;
    [SerializeField] AttackState attackState;
    [SerializeField] DeadState deadState;
    #endregion

    [Header("Animation")]
    #region 애니메이션 Hash 선언
    [SerializeField] Animator monsterAnimator;
    [SerializeField] Animator muzzlePointAnimator;

    private int curAniHash;
    private static int monsterMoveHash = Animator.StringToHash("MonsterMove");
    private static int monsterAttackHash = Animator.StringToHash("MonsterAtk");
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

            // 다른 상태로 전환
            if (Model.MonsterHP < 0.01f) { Monster.ChangeState(MonsterState.Dead); }
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
                Monster.StartCoroutine(Monster.WaitingShot());
            }

            // 다른 상태로 전환
            if (Model.MonsterHP < 0.01f) { Monster.ChangeState(MonsterState.Dead); }
        }
    }

    #region 발사 코루틴
    IEnumerator WaitingShot()
    {
        // 인벤토리가 열려있지 않은 상황에서만 코루틴이 진행되도록 함
        yield return new WaitUntil(() => GameManager.Instance.IsOpenInventory == false);

        while (isAttacked)
        {
            AnimatorPlay();
            yield return null;
        }
    }

    void shot()
    {
        //Debug.Log("몬스터 총알 발사");
        bullet = Instantiate(MonsterBullet, muzzlePoint.transform.position, muzzlePoint.transform.rotation);

        MonsterShotBullet bulletScript = bullet.GetComponent<MonsterShotBullet>();
        bulletScript.Damage = monsterModel.MonsterAttack;
        bulletScript.Speed = monsterModel.MonsterAttackSpeed;
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
            Monster.AnimatorPlay();
        }
    }

    void Dead()
    {
        Debug.Log("몬스터 삭제됨");

        // 코인이 UI를 향해 빨려가는 애니메이션 재생
        PlayerDataModel.Money += monsterModel.DropGold;
        // 몬스터 자체를 오브젝트 풀 패턴으로 보관하고 있어도 좋을듯
        Destroy(gameObject);
    }

    //====================================================

    void AnimatorPlay()
    {
        int checkAniHash;
        monsterAnimator.SetFloat("MoveSpeed", 0.2f * monsterModel.MonsterMoveSpeed);
        monsterAnimator.SetFloat("AtkSpeed", 0.25f * monsterModel.MonsterAttackSpeed);

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

    void MuzzlePointAni() { muzzlePointAnimator.Play("BulletStart"); }
}
