using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

    [SerializeField] private int fieldWaveMonsterCount;
    [SerializeField] private int killMonsterCount;
    [SerializeField] private int ThirdClassMonsterCount;

    [SerializeField] private int waveCount;

    //임시 보스
    [SerializeField] private TextMeshProUGUI bossText;

    public int FieldWaveMonsterCount { get { return fieldWaveMonsterCount; } }

    private StageCSVTest csvPaser;
    private int paserIndex = -1;

    public Action bgAction;

    private float killRate;

    //Stage 관련 임시 변수명
    private MiddleMap curSecondClass = MiddleMap.First;
    private int curThirdClass;
    private int curWave;
    private int maxWave; // 추후 Data Table 에서 받아올 필요 있음
    private int curWaveMonsterCount;    //Data Table에서 받아와야 함

    private bool isBoss;

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

    private bool isWave;

    private void Start()
    {
        csvPaser = GetComponent<StageCSVTest>();
  
        //모든 중분류의 Easy 난이도는 True로 변경
        for (int i = 0; i < (int)MiddleMap.SIZE; i++)
        {
            difficultCheck[i, 0] = true;
        }


        //MapController > 배경, 하늘 이미지 전달
        //mapController.BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
        //mapController.SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);

        monsters = new MonsterModel[5];
 
    }


    private void Update()
    {
        if (!csvPaser.isComplet)
        {
            return;
        }



        //테스트 코드
        //머지 후에 몬스터가 Destroy 됐을 경우 판단하는 방식으로 작성
        if (Input.GetKeyDown(KeyCode.Space))
        {
            monsters[testIndex].MonsterHP = 0;
        }

        

        if (monsters[testIndex] != null)
        {
            Debug.Log($"몬스터 파괴 여부 :{monsters[testIndex].IsDestroyed()}");
            if (monsters[testIndex].IsDestroyed())
            {
                Debug.Log("파괴된 이후!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                testIndex++;
                fieldWaveMonsterCount--;
            }

            //    if (monsters[testIndex].IsDestroyed())
            //    {
            //        Destroy(monsters[testIndex]);
            //        fieldWaveMonsterCount--;
            //        killMonsterCount++;
            //        testIndex++;
            //        //스테이지 진행률

            //    }
        }

        if (curWaveMonsterCount <= createLimitCount && createCo != null)
        {
            StopCoroutine(createCo);
            createCo = null;
        }

        
        if (!isWave &&fieldWaveMonsterCount < 1)
        {
            //wave 몬스터를 다 잡았으면 paserIndex 증가 
            Wave();
        }


        //스테이지 진행률 UI 연동
        killRate = ((float)killMonsterCount / ThirdClassMonsterCount) * 100f;
        foreGround.fillAmount = killRate * 0.01f;
        StageClear();
        MonsterSafeZone();

    }

     
    /// <summary>
    /// 다음 스테이지 이동
    /// </summary>
    private void NextStage()
    { 
        SetDifficult();

        //배경, 하늘 이미지 전달
        //mapController.BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
        //mapController.SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);
         
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
        
        isWave = true;

        paserIndex++;

        curThirdClass = csvPaser.State[paserIndex].Stage_thirdClass; 
        Debug.Log($"{csvPaser.State[paserIndex].Stage_secondClass}-{curThirdClass}-{csvPaser.State[paserIndex].Stage_wave}");

        //ThirdClass에서 소환되는 총 몬스터
        if (paserIndex % waveCount == 0)
        {
            for (int i = paserIndex; i < paserIndex + waveCount; i++)
            {
                ThirdClassMonsterCount += csvPaser.State[i].Stage_monsterNum;
            }
        }

        //현재 웨이브 몬스터 수
        curWaveMonsterCount = csvPaser.State[paserIndex].Stage_monsterNum;
        CreateMonster();




        //if (curWave <= maxWave)
        //마지막 Wave일 때 보스 몬스터가 생성됨
        //if (curWave < maxWave)
        //{
        //    bossText.gameObject.SetActive(false);
        //    //curWaveMonsterCount 에 Data Table 에서 받아온 값을 저장 
        //    createLimitCount = 0;
        //    CreateMonster();
        //}
        ////보스전 로직 작성
        //else
        //{
        //    //curWaveMonsterCount 에 Data Table 에서 받아온 값을 저장
        //    bossText.gameObject.SetActive(true); 
        //    createLimitCount = 0;
        //    CreateMonster();
        //}


        //IF curWave < maxWave
        //viewMonsterCount = curWaveMonsterCount;
        //ELSE IF curWave >= maxWave 현재 웨이브가 마지막 웨이브인 상태
        //보스몬스터 생성
        //보스 몬스터가 생성됐으면 isMeenBoss


    }

    public void CreateBoss()
    {
        //IF 보스 만난 상태에서 죽었는가
        //isMeenBoss && isDeath
        //보스 아이콘 노출
        //ELSE

        curWaveMonsterCount = 1;

    }



    /// <summary>
    /// 몬스터 생성 기능
    /// </summary>
    public void CreateMonster()
    { 
        if (createCo == null)
        {
            createCo = StartCoroutine(CreateMonsterCo());
        }

    }

    private IEnumerator CreateMonsterCo()
    {

        WaitForSeconds createWait = new WaitForSeconds(createTimer);
        WaitForSeconds cycleWait = new WaitForSeconds(cycleTimer);
        monsters = new MonsterModel[curWaveMonsterCount];
        testIndex = 0;

        //시간값으로 대기할 지 준비 상태로 할지 생각 필요
        yield return cycleWait;
        createLimitCount = 0;
        fieldWaveMonsterCount = curWaveMonsterCount;

        while (curWaveMonsterCount > createLimitCount)
        {
            float xPos = UnityEngine.Random.Range(11f, 13f);
            float yPos = UnityEngine.Random.Range(2.5f, -3f);
            Vector3 offset = new Vector3(xPos, yPos, 0);

            GameObject monsterInstance = Instantiate(monsterPrefab, monsterSpawnPoint.position + offset, monsterSpawnPoint.rotation);
            Collider2D monsterCollider = monsterInstance.GetComponent<Collider2D>();
            monsterCollider.enabled = false;

            monsters[createLimitCount] = monsterInstance.GetComponent<MonsterModel>();
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
        switch (csvPaser.State[paserIndex].Stage_firstClass)
        {
            case "쉬움":
                curDifficult = Difficutly.Easy;
                break;

            case "보통":
                curDifficult = Difficutly.Normal;
                break;

            case "어려움":
                curDifficult = Difficutly.Hard;
                break;

            case "지옥1":
                curDifficult = Difficutly.Hell1;
                break;
            case "지옥2":
                curDifficult = Difficutly.Hell2;
                break;
            case "지옥3":
                curDifficult = Difficutly.Hell3;
                break;
        }
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
