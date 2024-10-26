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

    public void BackGroundResetAction(Action action)
    {
        bgAction = action;
    }

}
