using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class StageDifficult : MonoBehaviour
{
    [SerializeField] int bossValue;

    private StageCSV stageCSV;
     
    private MonsterModel monsterModel;
    private int curStageIndex = 0;
    private int waveCount = 0;

    private void Start()
    {
        stageCSV = StageCSV.Instance;
    }

    /// <summary>
    /// 몬스터 모델 인스턴스를 받아옴
    /// </summary>
    /// <param name="monsterModel">생성된 몬스터 인스턴스</param>
    public void GetMonsterInstance(MonsterModel monsterModel)
    {
        this.monsterModel = monsterModel;  
    }
     
    /// <summary>
    /// 현재 진행중인 스테이지의 Table Index 값을 받아옴
    /// </summary>
    /// <param name="stageIndex">Data Table Index</param>
    public void GetStageIndex(int stageIndex, int waveCount)
    {
        curStageIndex = stageIndex;
        this.waveCount = waveCount;
    }

    /// <summary>
    /// 각 스테이지 별 몬스터 추가 능력치 적용
    /// 공격력 계산식
    /// (Stage_AttackIncNum * ( 10 ^ Stage_AttackIncUnit)) * Enemy_atk
    /// 체력 계산식
    /// (Stage_HpIncNum * ( 10 ^ Stage_HpIncUnit)) * Enemy_hp
    /// </summary>
    public void MonsterIncreaseAbility()
    { 
        float monsterHP = 0f;   
        int monsterAtk = 0; 

        //공격력 증가 수치
        float attackNum = stageCSV.State[curStageIndex].Stage_AttackNum;
        float attackUnit = stageCSV.State[curStageIndex].Stage_attackUnit;

        //체력 증가 수치
        float hpNum = stageCSV.State[curStageIndex].Stage_hpNum;
        float hpUnit = stageCSV.State[curStageIndex].Stage_hpUnit;
        // 테이블에서 값을 가져오므로 기존 프리팹 수치는 계산식에서 제외
        monsterAtk = (int)(attackNum * Mathf.Pow(10, attackUnit));
        monsterModel.MonsterAttack = monsterAtk;
        monsterHP = (hpNum * Mathf.Pow(10, hpUnit));
        monsterModel.MonsterHP = monsterHP; 
    }

    public float CurStageMonsterHP()
    {
        return monsterModel.MonsterHP;
    }

    public int CurStageMonsterAtk()
    {
        return monsterModel.MonsterAttack;
    }

}
