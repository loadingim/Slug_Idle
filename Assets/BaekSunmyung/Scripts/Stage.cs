using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    [Tooltip("Monster Wave Cycle")]
    [SerializeField] private float cycleTimer;

    [Tooltip("Monster Create Delay")]
    [SerializeField] private float createTimer;

    [Header("Monster Safe Zone")]
    [SerializeField] private Transform safeZone;

    [SerializeField] private int viewMonsterCount;
    [SerializeField] private int killMonsterCount;
    [SerializeField] private int totalMonsterCount;

    //임시 보스
    [SerializeField] private TextMeshProUGUI bossText;

    public int ViewMonsterCount { get { return viewMonsterCount; } }

    public Action bgAction;

    private float killRate;

    //Stage 관련 임시 변수명
    private MiddleMap curMiddleMap = MiddleMap.First;
    private int curSmallStage = 1;
    private int curWave = 0;
    private int maxWave = 3; // 추후 Data Table 에서 받아올 필요 있음
    private int curWaveMonsterCount = 5;    //Data Table에서 받아와야 함

    //현재 중분류 난이도 체크 (임시 변수명)
    private Difficutly curDifficult = Difficutly.Easy;
    private bool[,] difficultCheck = new bool[(int)MiddleMap.SIZE, (int)Difficutly.SIZE];


    //몬스터 생성 코루틴 카운트 변수
    private int createLimitCount = 0;

    //몬스터 생성 코루틴
    private Coroutine createCo;


    //테스트 코드
    [SerializeField] MonsterModel[] monsters;
    private int testIndex = 0;


    private void Start()
    {
        //테스트 코드
        monsters = new MonsterModel[curWaveMonsterCount];


        //모든 중분류의 Easy 난이도는 True로 변경
        for (int i = 0; i < (int)MiddleMap.SIZE; i++)
        {
            difficultCheck[i, 0] = true;
        }


        //MapController > 배경, 하늘 이미지 전달
        //mapController.BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
        //mapController.SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);


        //추후 Table에서 받아와서 수정 필요
        //첫 번째 Wave 시작
        totalMonsterCount = curWaveMonsterCount * maxWave;

    }


    private void Update()
    {
        Debug.Log($"{curMiddleMap} / 난이도 :{curDifficult}");
        Debug.Log($"현재 스테이지:{curMiddleMap} - {curSmallStage} -{curWave}");
         
        //테스트 코드
        if (Input.GetKeyDown(KeyCode.Space))
        {
            monsters[testIndex].MonsterHP = 0;
        }

        if (monsters[testIndex] != null)
        {
            if (monsters[testIndex].MonsterHP < 1)
            {
                Destroy(monsters[testIndex]);
                viewMonsterCount--;
                killMonsterCount++;
                testIndex++;
                //스테이지 진행률
                killRate = ((float)killMonsterCount / totalMonsterCount) * 100f;
            }
        }
 

        //스테이지 진행률 UI 연동
        foreGround.fillAmount = killRate * 0.01f;
        StageClear();
        MonsterSafeZone();


        if (curWaveMonsterCount <= createLimitCount && createCo != null)
        {
            Debug.Log("생성 코루틴 중지");
            StopCoroutine(createCo);
            createCo = null;
        }

        if (viewMonsterCount < 1)
        {
            testIndex = 0; 
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

            //임시 조건 (int)curMiddleMap < (int)MiddleMap.SIZE;
            if ((int)curMiddleMap < mapData.Count - 1)
            {
                curMiddleMap++;
            }
            else
            {
                //모든 중분류 클리어 시 첫 중분류로 변경
                curMiddleMap = MiddleMap.First;
            }

            curSmallStage = 1;

            SetDifficult();

            //배경, 하늘 이미지 전달
            //mapController.BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
            //mapController.SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);
        }

        //추후 Table에서 받아와서 수정 필요
        totalMonsterCount = curWaveMonsterCount * maxWave;
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

    /// <summary>
    /// Stage Wave 생성 
    /// </summary>
    public void Wave()
    {
        //if (curWave <= maxWave)
        //마지막 Wave일 때 보스 몬스터가 생성됨
        if (curWave < maxWave)
        {
            bossText.gameObject.SetActive(false);
            //curWaveMonsterCount 에 Data Table 에서 받아온 값을 저장 
            createLimitCount = 0;
            CreateMonster();
        }
        //보스전 로직 작성
        else
        {
            //curWaveMonsterCount 에 Data Table 에서 받아온 값을 저장
            bossText.gameObject.SetActive(true); 
            createLimitCount = 0;
            CreateMonster();
        }


        //IF curWave < maxWave
        //viewMonsterCount = curWaveMonsterCount;
        //ELSE IF curWave >= maxWave 현재 웨이브가 마지막 웨이브인 상태
        //보스몬스터 생성
        //보스 몬스터가 생성됐으면 isMeenBoss


    }

    public void BossAndDeath()
    {

        //IF 보스 만난 상태에서 죽었는가
        //isMeenBoss && isDeath
        //보스 아이콘 노출
        //ELSE
         
    }


    /// <summary>
    /// 몬스터 생성 기능
    /// </summary>
    public void CreateMonster()
    {
        if(createCo == null)
        {
            createCo = StartCoroutine(CreateMonsterCo());
        } 
        
    }

    private IEnumerator CreateMonsterCo()
    {
         
        WaitForSeconds createWait = new WaitForSeconds(createTimer);
        WaitForSeconds cycleWait = new WaitForSeconds(cycleTimer);

        //시간값으로 대기할 지 준비 상태로 할지 생각 필요
        yield return cycleWait;
        curWave++;
        viewMonsterCount = curWaveMonsterCount;

        while (curWaveMonsterCount > createLimitCount)
        {
            Debug.Log("생성 시작");
            float xPos = UnityEngine.Random.Range(11f, 13f);
            float yPos = UnityEngine.Random.Range(2.5f, -3f);
            Vector3 offset = new Vector3(xPos, yPos, 0);

            GameObject monsterInstance = Instantiate(monsterPrefab, monsterSpawnPoint.position + offset, monsterSpawnPoint.rotation);
            Collider2D monsterCollider = monsterInstance.GetComponent<Collider2D>();
            monsterCollider.enabled = false;

            monsters[createLimitCount] = monsterInstance.GetComponent<MonsterModel>();
            AdjustmentStats(createLimitCount);
            createLimitCount++;
            yield return createWait;
        }


        yield break;
    }


    /// <summary>
    /// 몬스터 Collider On
    /// </summary>
    public void MonsterSafeZone()
    {

        for (int i = 0; i < monsters.Length; i++)
        {
            //몬스터가 Null이 아닌 상태에서 SafeZone을 지났을 경우 Collider 활성화
            if (monsters[i] != null)
            {
                if (monsters[i].transform.position.x < safeZone.transform.position.x)
                {
                    Collider2D collider = monsters[i].GetComponent<Collider2D>();
                    collider.enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// 난이도 별 스탯 조정
    /// </summary>
    /// <param name="index">몬스터 생성 시 받아올 인덱스</param>
    public void AdjustmentStats(int index)
    {
        switch (curDifficult)
        {
            case Difficutly.Easy:
                monsters[index].MonsterMoveSpeed = 3.5f;
                break;

            case Difficutly.Normal:
                monsters[index].MonsterMoveSpeed = 5.5f;
                break;

            case Difficutly.Hard:
                monsters[index].MonsterMoveSpeed = 7.5f;
                break;
        }
        
    }

    /// <summary>
    /// 현재 중분류 맵에서 난이도가 True 인 값을 찾아서 현재 난이도에 할당
    /// </summary>
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

    /// <summary>
    /// 백그라운드 이미지 위치 리셋 액션
    /// </summary>
    /// <param name="action"></param>
    public void BackGroundResetAction(Action action)
    {
        bgAction = action;
    }



}
