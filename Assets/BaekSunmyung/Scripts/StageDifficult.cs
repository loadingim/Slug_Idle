using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class StageDifficult : MonoBehaviour
{
    private StageCSV stageCSV;
     
    private MonsterModel monsterModel;
    private int curStageIndex = 0;


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
    public void GetStageIndex(int stageIndex)
    {
        curStageIndex = stageIndex;
    }

    public void IncreaseAbility()
    {

    }



}
