using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
    [SerializeField] private GameObject bossObject;
    [SerializeField] private Button bossChallengeBtn;

    public int FieldWaveMonsterCount { get { return fieldWaveMonsterCount; } }

    //배경 이미지 리셋
    public Action bgAction;

    //Data Table CSV 변수
    private StageCSV csvParser;

    //Data Table Index 변수
    private int parserIndex = 0;

    //몬스터 소탕률
    private float killRate;

    //Stage 관련 임시 변수명
    private MiddleMap curSecondClass = MiddleMap.First;
    private int curThirdClass;
    private int curWaveMonsterCount;    //Data Table에서 받아와야 함

    //Wave 생성 여부
    private bool isWave;

    //보스 스테이지 진입 여부
    private bool isBoss;

    private bool isStageClear;

    //보스 스테이지 클리어 여부
    private bool isBossClear;

    private bool isLoop;

    //현재 중분류 난이도 체크 (임시 변수명)
    private Difficutly curDifficult = Difficutly.Easy;

    //몬스터 생성 코루틴 카운트 변수
    private int createLimitCount = 0;

    //몬스터 생성 코루틴
    private Coroutine createCo;

    //생성된 몬스터 저장 배열
    [SerializeField] MonsterModel[] monsters;

    //Player 변수
    PlayerDataModel player;
    [SerializeField] private bool isPlayerLife = true;
    [SerializeField] Bullet playerBullet;


    public bool IsWave { get { return isWave; } }



    private void Awake()
    {
        bossChallengeBtn.onClick.AddListener(BossChallenge);
    }

    private void Start()
    {
        csvParser = StageCSV.Instance;

        //MapController > 배경, 하늘 이미지 전달
        //mapController.BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
        //mapController.SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);

        monsters = new MonsterModel[5];

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataModel>();
        bossObject.gameObject.SetActive(false);
    }


    private void Update()
    {



        //Data를 받아오지 못한 상태면 Return
        if (csvParser.State.Count == 0)
        {
            return;
        }



        //생성된 Wave 몬스터가 없을 경우
        if (!isWave && fieldWaveMonsterCount < 1)
        {
            //Wave 호출
            Wave(); 
        }

        Debug.Log($"현재 스테이지 :{curThirdClass} - {csvParser.State[parserIndex].Stage_wave}");
        Debug.Log($"현재 인덱스 :{parserIndex}");
        if (player.Health < 1)
        {
            isPlayerLife = false;

        }


        StopedCoroutine();
        MonsterRemover();
        StageClear();
        MonsterSafeZone();
        PlayerDeath();
        BossStage();

        
 
        //스테이지 진행률 UI 연동 
        foreGround.fillAmount = killRate * 0.01f;


    }


    /// <summary>
    /// 죽은 몬스터 확인
    /// </summary>
    private void MonsterRemover()
    {
        if (!isPlayerLife)
        {
            //플레이어 사망 시 소환된 모든 몬스터 삭제
            //foreach (MonsterModel model in monsters)
            //{
            //    if (model != null)
            //    {
            //        Destroy(model.gameObject);
            //    }
            //}

            //몬스터 저장 배열 클리어
            Array.Clear(monsters, 0, monsters.Length);
        }
        else
        {
            foreach (MonsterModel model in monsters)
            {
                if (model != null && model.MonsterHP < 1)
                {
                    for (int i = 0; i < monsters.Length; i++)
                    {
                        if (model == monsters[i])
                        {
                            monsters[i] = null;
                            killMonsterCount++;
                            fieldWaveMonsterCount--;
                        }
                    }
                }
            }

        }
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
            isStageClear = true;
            NextStage();
        }
    }

    /// <summary>
    /// Data Table ThirdClass Column > 소환되는 총 몬스터
    /// </summary>
    public void CalculateMonsterSpawn()
    {
        bossObject.gameObject.SetActive(false);
        isBoss = false;

        //스테이지 소환 몬스터 수 초기화
        ThirdClassMonsterCount = 0;

        for (int i = parserIndex; i < parserIndex + waveCount; i++)
        {
            ThirdClassMonsterCount += csvParser.State[i].Stage_monsterNum;
        }
    }

    /// <summary>
    /// Stage Wave 생성 
    /// </summary>
    public void Wave()
    {
        killRate = ((float)killMonsterCount / ThirdClassMonsterCount) * 100f;

        //Index 다중 증가 방지
        isWave = true;
        isStageClear = false;

        //소분류 스테이지 
        curThirdClass = csvParser.State[parserIndex].Stage_thirdClass;

        //스테이지 새로 진입 
        if (parserIndex % waveCount == 0)
        {
            CalculateMonsterSpawn();
        }

        Debug.Log("웨이브 생성");
        //현재 웨이브 몬스터 수
        curWaveMonsterCount = csvParser.State[parserIndex].Stage_monsterNum;

        createLimitCount = 0;
        CreateMonster();


        //isBoss && !isBosoClear && 버튼이 나와있는 상태
        //return;

        //보스전 테스트 인덱스
        //parserIndex = 4;


        if (parserIndex % waveCount < 4)
        {
            //여기가 문제인가 조건을 isPlayerLife && isStageClear?
            if (isPlayerLife)
            {
                parserIndex++;
            }

            //parserIndex++;
        }

    }

    public void BossStage()
    {
        if (parserIndex % waveCount >= 4)
        {
            Debug.Log("보스 스테이지 진입");

            //보스 스테이지 진행
            isBoss = true;

            if (isBoss)
            {   
                //보스를 잡은 상태
                if(isStageClear)
                {
                    Debug.Log("보스 클리어");
                    parserIndex++;
                }
                else
                {
                    Debug.Log("보스 클리어 실패");
                    bossObject.gameObject.SetActive(true); 
                }
            }
        }
    }
     
    public void PlayerDeath()
    {
         
        if (!isPlayerLife)
        {
            Debug.Log("플레이어 사망");
            //Bullet을 어떻게 얻어와야 하는가?

            if (TryGetComponent<Bullet>(out playerBullet))
            {
                Destroy(playerBullet.gameObject);
            }
             
            //몬스터 생성 중지
            if (createCo != null)
            {
                StopCoroutine(createCo);
                createCo = null;

                //Wave False로 변경
                isWave = false;
            }
 
            //보스 스테이지 진입 상태
            if (isBoss)
            {
                //보스 필드 진입 후 죽으면 -1 감소 
                if (!isLoop)
                {
                    parserIndex += -1;
                    isLoop = true;
                }
            }
            else
            {
                //현재 2스테이지 이상 진입 상태
                if (parserIndex >= 5)
                { 
                    //이전 Wave 단계로 이동 
                    //ex) Index 13 (3Stage 4Wave 상태) > Index 5 (2Stage 1Wave) 이동
                    parserIndex += -((parserIndex % waveCount) + waveCount); 
                }
                //현재 1스테이지 상태
                else
                {
                    parserIndex += -parserIndex;
                }
            }

            //배경 리셋
            bgAction?.Invoke();
            //테스트 코드
            player.Health = 3000;

            isPlayerLife = true;
        } 
    }

 
    /// <summary>
    /// 보스 스테이지 재도전 기능
    /// </summary>
    public void BossChallenge()
    {
        Debug.Log("보스 재도전!!");
        parserIndex += 1;
        isLoop = false;
        bossObject.gameObject.SetActive(false);
        bgAction?.Invoke();
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

    public void StopedCoroutine()
    {
        //Wave 생상 완료됐으면
        if (curWaveMonsterCount <= createLimitCount && createCo != null)
        {
            //코루틴 중지
            StopCoroutine(createCo);
            createCo = null;
            //Wave False로 변경
            isWave = false;
        }
    }

    int a = 0;
    private IEnumerator CreateMonsterCo()
    {

        WaitForSeconds createWait = new WaitForSeconds(createTimer);
        WaitForSeconds cycleWait = new WaitForSeconds(cycleTimer);
        monsters = new MonsterModel[curWaveMonsterCount];

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

            monsterInstance.gameObject.name = a.ToString() + "몬스터";
            a++;
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
        switch (csvParser.State[parserIndex].Stage_firstClass)
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
