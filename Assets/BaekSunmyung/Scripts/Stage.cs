using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Stage : MonoBehaviour
{
    [Header("맵 스테이지 데이터")]
    [SerializeField] private List<MapData> mapData = new List<MapData>();
    [SerializeField] private MapController mapController;
    [SerializeField] private Image foreGround;

    [Header("Monster Spawn")]
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private GameObject monsterPrefab;

    [SerializeField] private int viewMonsterCount;
    [SerializeField] private int killMonsterCount;
    [SerializeField] private int totalMonsterCount;

    public int ViewMonsterCount { get { return viewMonsterCount; } }

    public Action bgAction;

    private float killRate;

    //임시 변수명
    private MiddleMap curMiddleMap = MiddleMap.One;
    private int curSmallStage = 1;
    private int curWave = 1;
    private int maxWave = 4; // 추후 Data Table 에서 받아올 필요 있음
    private int curWaveMonsterCount = 5;

    //현재 중분류 난이도 체크
    private Difficutly curDifficult = Difficutly.Easy;
    private bool[,] difficultCheck = new bool[(int)MiddleMap.SIZE, (int)Difficutly.SIZE];

    private void Start()
    {
        for (int i = 0; i < (int)MiddleMap.SIZE; i++)
        {
            //ㅅ
            difficultCheck[i, 0] = true;
        }


        //MapController > 배경, 하늘 이미지 전달
        //mapController.BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
        //mapController.SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);

        //추후 Table에서 받아와서 수정 필요
        //totalMonsterCount = curWaveMonsterCount * maxWave;
        totalMonsterCount = curWaveMonsterCount;
        Wave();
    }


    private void Update()
    {
        Debug.Log($"{curMiddleMap} / 난이도 :{curDifficult}");
        Debug.Log($"현재 스테이지:{curMiddleMap} - {curSmallStage} -{curWave}");

        //테스트 코드
        if (Input.GetKeyDown(KeyCode.Space))
        {

            viewMonsterCount--;
            killMonsterCount++;

            //스테이지 진행률
            killRate = ((float)killMonsterCount / totalMonsterCount) * 100f;

        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            viewMonsterCount++;
        }

        //스테이지 진행률 UI 연동
        foreGround.fillAmount = killRate * 0.01f;
        StageClear();

        if (viewMonsterCount < 1)
        {
            curWave++;
            Wave();
        }

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
            
            ChangeDifficult();
            
            if((int)curMiddleMap < mapData.Count-1)
            {
                curMiddleMap++;
            }
            else
            {
                curMiddleMap = MiddleMap.One;
            }

            curSmallStage = 1;

            SetDifficult();

            //배경, 하늘 이미지 전달
            //mapController.BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
            //mapController.SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);
        }

        //추후 Table에서 받아와서 수정 필요
        //totalMonsterCount = curWaveMonsterCount * maxWave;
        totalMonsterCount = curWaveMonsterCount;
        curWave = 0;

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
            NextStage();
        }
    }

    public void SetDifficult()
    {
        //현재 중분류의 클리어한 난이도 확인
        //true 가 된 나이도가 있으면 해당 난이도로 변경

        for (int i = 0; i < (int)Difficutly.SIZE; i++)
        {
            if (difficultCheck[(int)curMiddleMap, i])
            {
                curDifficult = (Difficutly)i;
            }
        }
    }

    /// <summary>
    /// 해당 중분류 다음 난이도 해금
    /// </summary>
    public void ChangeDifficult()
    {
        //현재 중분류의 난이도 확인
        int curDifIndex = (int)curDifficult + 1;
        difficultCheck[(int)curMiddleMap, curDifIndex] = true;

    }

    public void Wave()
    {

        if (curWave <= maxWave)
        {
            viewMonsterCount = curWaveMonsterCount;
            CreateMonster();
        }
    }


    public void CreateMonster()
    {
        //몬스터 클래스 받아와서 Instantiate 진행
        for (int i = 0; i < curWaveMonsterCount; i++)
        {

            //Offset 값은 기획 의도에 맞춰 변경
            float xPos = UnityEngine.Random.Range(9.5f, 11f);
            float yPos = UnityEngine.Random.Range(2.5f, -3f);
            Vector3 offset = new Vector3(xPos, yPos, 0);

            GameObject monsterInstance = Instantiate(monsterPrefab, monsterSpawnPoint.position + offset, monsterSpawnPoint.rotation);
        }

    }


    public void BackGroundResetAction(Action action)
    {
        bgAction = action;
    }

}
