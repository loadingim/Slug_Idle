using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stage : MonoBehaviour
{
    [Header("맵 스테이지 데이터")]
    [SerializeField] private List<MapData> mapData = new List<MapData>();

    [SerializeField] private MapController mapController;

    [SerializeField] private int viewMonsterCount;
    [SerializeField] private int killMonsterCount;
    [SerializeField] private int totalMonsterCount;


    public Action bgAction;

    private float killRate;

    //임시 변수명
    private MiddleMap curMiddleMap = MiddleMap.One;
    private int curSmallStage = 1;

    private int curWave = 1;
    private int maxWave = 4; // 추후 Data Table 에서 받아올 필요 있음

    //현재 중분류 난이도 체크
    private Difficutly curDifficult = Difficutly.Easy;
    private bool[,] difficultCheck = new bool[(int)MiddleMap.SIZE, (int)Difficutly.SIZE];

    private void Start()
    {
        //MapController > 배경, 하늘 이미지 전달
        //mapController.BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
        //mapController.SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);
    }


    private void Update()
    {
        Debug.Log($"현재 스테이지:{curMiddleMap} - {curSmallStage}");

        //테스트 코드
        if (Input.GetKeyDown(KeyCode.Space))
        {

            viewMonsterCount--;
            killMonsterCount++;

            //스테이지 진행률
            killRate = ((float)killMonsterCount / totalMonsterCount) * 100;

        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            viewMonsterCount++;
        }

        StageClear();
    }

    /// <summary>
    /// 다음 스테이지 이동
    /// </summary>
    private void NextStage()
    {
        //맵 스테이지 변경 > ex) 1-1 > 1-2
        if (curSmallStage < mapData[(int)curMiddleMap].MaxSmallStage)
        {
            curSmallStage++;
        }
        else
        {
            curMiddleMap++;
            curSmallStage = 1;

            //배경, 하늘 이미지 전달
            //mapController.BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
            //mapController.SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);
        }

        bgAction?.Invoke();
    }

    /// <summary>
    /// 스테이지 클리어
    /// </summary>
    private void StageClear()
    {
        if (killRate >= 100f)
        {
            killMonsterCount = 0;
            killRate = 0f;
            viewMonsterCount = 1;

            NextStage();
        }
    }
    
    public void SetDifficult()
    {
        //현재 중분류의 클리어한 난이도 확인
        //true 가 된 나이도가 있으면 해당 난이도로 변경
        
        //for(int i = 0; i < (int)Difficutly.SIZE; i++)
        //{
        //    if (difficultCheck[(int)curMiddleMap, i])
        //    {
        //        curDifficult = (Difficutly)i;
        //    }
        //}
         
    }

    public void ChangeDifficult()
    {
        //현재 중분류의 난이도 확인
        //IF 중분류의 smallStage가 최대 stage보다 커졌는가?
        //True : difficultCheck[(int)curMiddleMap, (int)curDifficult++] = true;
        //False : difficultCheck[(int)curMiddleMap, (int)curDifficult++] = false;
    }

    public void StartWave()
    {
        //IF 현재 웨이브가 maxWave 전인가
        //현재 웨이브 확인
        //현재 웨이브에 맞는 몬스터 생성 
        //CreateMonster()

        //IF killRate >= 100f
        //Stage Clear()
        //curWave ++;

        //IF 증가한 wave가 maxWave 보다 큰가?
        //curSmallStage 증가
        //IF curSmallStage가 중분류의 maxStage에 도달했는가?
        //middleStage 증가
    }

    public void CreateMonster()
    {
        //몬스터 클래스 받아와서 Instantiate 진행
    }


    public void BackGroundResetAction(Action action)
    {
        bgAction = action;
    }

}
