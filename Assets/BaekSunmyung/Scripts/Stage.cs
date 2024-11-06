using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Stage : MonoBehaviour
{
    [Header("스테이지 난이도 능력치")]
    [SerializeField] private StageDifficult stageDifficult;

    [Header("맵 스테이지 데이터")]
    [SerializeField] private List<MapData> mapData = new List<MapData>();
    [SerializeField] private MapController mapController;
    [SerializeField] private Image foreGround;

    [Header("Monster Spawn")]
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private GameObject normalMonsterPrefab;
    [SerializeField] private GameObject[] bosslMonsterPrefab;

    [Header("Monster Wave Setting")]
    [Tooltip("Monster Wave Cycle")]
    [SerializeField] private float cycleTimer;

    [Tooltip("Monster Create Delay")]
    [SerializeField] private float createTimer;

    [Header("Monster Safe Zone")]
    [SerializeField] private Transform safeZone;

    [Header("Stage Data Variable")]
    [Tooltip("대분류 스테이지 개수")]
    [SerializeField] private int stageSecondClass;
    [Tooltip("소분류 Wave 개수")]
    [SerializeField] private int waveCount;

    [SerializeField] private TextMeshProUGUI stageInfoText;

    [Header("Test Mode")]
    [SerializeField] private GameObject testSet;
    [SerializeField] private Button testModeButton;
    [SerializeField] private TMP_InputField textIndex;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI hpText;
    private GraphicRaycaster ray;
    private PointerEventData ped = new PointerEventData(EventSystem.current);
    private List<RaycastResult> results = new List<RaycastResult>();

    


    //임시 보스
    [SerializeField] private GameObject bossObject;
    [SerializeField] private Button bossChallengeBtn;

    public int FieldWaveMonsterCount { get { return fieldWaveMonsterCount; } }

    //배경 이미지 리셋
    public Action bgAction;

    //배경 이미지 인덱스
    private int bgSecondClsIndex = 0;

    //Data Table CSV 변수
    private StageCSV stageCSV;

    private CoroutineManager coroutineManager;

    //Data Table Index 변수
    private int parserIndex;
    public int ParserIndex { get { return parserIndex; } }
    //몬스터 소탕률
    private float killRate;

    //Stage 관련 변수
    private int curSecondClass;     //스테이지 난이도 변수
    public int SecondClass { get { return curSecondClass; } }

    private int curThirdClass;          //현재 스테이지의 위치
    [SerializeField] private int curWaveMonsterCount;    //현재 Wave에서 생성될 몬스터 수
    [SerializeField] private int curWaveKillCount;       //이전 스테이지 이동 시 진행률에서 빼줄 값
    [SerializeField] private int fieldWaveMonsterCount;  //현재 필드위에 남은 몬스터 수
    [SerializeField] private int killMonsterCount;       //현재까지 잡은 몬스터 수
    [SerializeField] private int ThirdClassMonsterCount; //현재 스테이지의 소환될 총 몬스터 수

    //Wave 생성 여부
    private bool isWave;

    //보스 스테이지 진입 여부
    private bool isBoss;
    public bool IsBoss { get { return isBoss; } }

    //일반 스테이지 클리어 여부
    private bool isStageClear;

    //Player Skill CoolTime 초기화
    [SerializeField] private bool coolTimeReset;
    public bool CoolTimeReset { get { return coolTimeReset; } }

    //보스 스테이지 클리어 여부
    private bool isBossClear;

    //해당 스테이지 반복 여부
    private bool isLoop;

    //현재 중분류 난이도 체크 (임시 변수명)
    private Difficutly curDifficult = Difficutly.Easy;

    //몬스터 생성 코루틴 카운트 변수
    private int createLimitCount = 0;

    //코루틴 이름
    private string createCoroutineName = "CreateMonster";

    //생성된 몬스터 저장 배열
    public MonsterModel[] monsters;

    //Player 변수
    private PlayerDataModel player;
    [SerializeField] private bool isPlayerLife = true;

    public bool IsWave { get { return isWave; } }
     
    private void Awake()
    {
        bossChallengeBtn.onClick.AddListener(BossChallenge);
        testModeButton.onClick.AddListener(TestMode);
        ray = canvas.GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        coroutineManager = CoroutineManager.Instance;
        stageCSV = StageCSV.Instance;
        monsters = new MonsterModel[5];
        parserIndex = StageManager.Instance.StageIndex;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataModel>();
        bossObject.gameObject.SetActive(false);
    }

    

    private void Update()
    {
        //Data를 받아오지 못한 상태면 Return
        if (stageCSV.State.Count == 0)
        {
            return;
        }

        //생성된 Wave 몬스터가 없을 경우
        if (!isWave && fieldWaveMonsterCount < 1)
        {
            coolTimeReset = true;
            //Wave 호출
            Wave();
        }

        if (player.Health < 1)
        {
            isPlayerLife = false;
        }
        TestActive(); 
        MonsterSafeZone();
        PlayerDeath();
        StopMonsterCoroutine();
        StageClear();
        MonsterRemover();
        SetStageText();

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
            foreach (MonsterModel model in monsters)
            {
                if (model != null)
                {
                    Destroy(model.gameObject);
                }
            }

            //몬스터 저장 배열 클리어
            Array.Clear(monsters, 0, monsters.Length);
            isPlayerLife = true;
        }
        else
        {
            //플레이어가 살아있을 때 몬스터 제거 작업
            foreach (MonsterModel model in monsters)
            { 
                if (model != null && model.MonsterHP < 1)
                {
                    for (int i = 0; i < monsters.Length; i++)
                    {
                        if (model == monsters[i])
                        {
                            Debug.Log("몬스터 제거 완료");
                            curWaveKillCount++;
                            monsters[i] = null;
                            killMonsterCount++;
                            fieldWaveMonsterCount--;
                        }
                    }
                }
            }
            //스테이지 클리어 트리거
            isStageClear = true;

            //배경, 하늘 이미지 전달 
            curSecondClass = stageCSV.State[parserIndex].Stage_secondClass;
            mapController.BackGroundSpriteChange(curSecondClass);
            mapController.SkySpriteChange(curSecondClass);

        }
    }



    /// <summary>
    /// 다음 스테이지 이동
    /// </summary>
    private void NextStage()
    {
        SetDifficult();
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
            isBoss = false;
            isBossClear = true;
            NextStage();
        }
    }

    /// <summary>
    /// Data Table ThirdClass Column > 소환되는 총 몬스터
    /// </summary>
    public void CalculateMonsterSpawn()
    {
        //각 스테이지 1Wave 진입 시 Boss 클리어 전 상태로 변경 
        isBossClear = false;

        //스테이지 소환 몬스터 수 초기화
        ThirdClassMonsterCount = 0;

        for (int i = parserIndex; i < parserIndex + waveCount; i++)
        {
            ThirdClassMonsterCount += stageCSV.State[i].Stage_monsterNum;
        }
    }

    /// <summary>
    /// Stage Wave 생성 
    /// </summary>
    public void Wave()
    {
        //스테이지 클리어 한 상태에서만 Index 증가
        if (isStageClear)
        {
            parserIndex++;
             
            curWaveKillCount = 0;
        }

        //스테이지 새로 진입 
        if (parserIndex % waveCount == 0)
        {
            PlayerPrefs.SetFloat("StageIndex", parserIndex);
            CalculateMonsterSpawn();
        }

        mapController.ThirdIndex = parserIndex % waveCount;

        //보스 스테이지 진입 단계
        if (parserIndex % waveCount >= 4)
        {
            isBoss = true; 
            bossObject.gameObject.SetActive(false);
        }
        BossStage();

        //스테이지 진행률
        killRate = ((float)killMonsterCount / ThirdClassMonsterCount) * 100f;

        //Index 다중 증가 방지
        isWave = true;
        isStageClear = false;
        coolTimeReset = false;

        //소분류 스테이지 
        curThirdClass = stageCSV.State[parserIndex].Stage_thirdClass;

        //현재 웨이브 몬스터 수
        curWaveMonsterCount = stageCSV.State[parserIndex].Stage_monsterNum;

        //몬스터 생성 제한 수
        createLimitCount = 0;
        stageDifficult.GetStageIndex(parserIndex, waveCount);
        CreateMonster();

    }

    public void BossStage()
    {
        //보스 단계에 진입한 상태에서 플레이어 사망 상태
        if (parserIndex % waveCount == 3 && isBoss && !isBossClear)
        {
            bossObject.gameObject.SetActive(true);
        }
    }

    public void PlayerDeath()
    {
        int prevIndex = 0;
        int prevWaveCount = 0;
        if (!isPlayerLife)
        {
            coolTimeReset = true;

            //이전 Wave 인덱스
            prevIndex = parserIndex > 0 ? parserIndex - 1 : 0;

            //이전 Wave 몬스터 수
            prevWaveCount = stageCSV.State[prevIndex].Stage_monsterNum;

            //현재 죽인 몬스터 수 - (이전 스테이지에서 생상된 몬스터 수 + 현재 스테이지에서 죽인 몬스터수)
            killMonsterCount = (killMonsterCount - (prevWaveCount + curWaveKillCount));

            //스테이지 진행률 조정
            killRate = ((float)killMonsterCount / ThirdClassMonsterCount) * 100f;
            fieldWaveMonsterCount = 0;
            isStageClear = false;

            //보스 스테이지 진입 상태
            if (isBoss)
            {
                //보스 필드 진입 후 죽으면 -1 감소 
                if (!isLoop)
                {
                    parserIndex--;
                    isLoop = true;
                }
            }
            else
            {
                parserIndex += parserIndex > 0 ? -1 : 0;
            }

            //배경 리셋
            bgAction?.Invoke();

            curWaveKillCount = 0;
        }

    }

    /// <summary>
    /// 보스 스테이지 재도전 기능
    /// </summary>
    public void BossChallenge()
    {
        parserIndex++;
        isLoop = false;
        bossObject.gameObject.SetActive(false);
        bgAction?.Invoke();
    }


    private void StopMonsterCoroutine()
    {
        if (curWaveMonsterCount <= createLimitCount)
        {
            coroutineManager.ManagerCoroutineStop(this); 
            isWave = false;
        }
    }

    private Coroutine createRoutine;
    /// <summary>
    /// 몬스터 생성 기능
    /// </summary>
    public void CreateMonster()
    {
        createRoutine = StartCoroutine(CreateMonsterCo());
        coroutineManager.ManagerCoroutineStart(createRoutine, this); 
    }

    int monsterNumber = 1;
    private IEnumerator CreateMonsterCo()
    {
        monsters = new MonsterModel[curWaveMonsterCount]; 
        yield return CoroutineManager.Instance.GetWaitForSeconds(cycleTimer);

        createLimitCount = 0;
        fieldWaveMonsterCount = curWaveMonsterCount;
         
        while (curWaveMonsterCount > createLimitCount)
        {
            float xPos = UnityEngine.Random.Range(11f, 13f);
            float yPos = UnityEngine.Random.Range(2.5f, -3f);
            Vector3 offset = new Vector3(xPos, yPos, 0);

            if (PlayerDataModel.Instance.Health >= 0)
            {

                if(parserIndex % waveCount <= 3)
                {
                    GameObject monsterInstance = Instantiate(normalMonsterPrefab, monsterSpawnPoint.position + offset, monsterSpawnPoint.rotation);
                    Collider2D monsterCollider = monsterInstance.GetComponent<Collider2D>();
                    monsterCollider.enabled = false;

                    monsterInstance.gameObject.name = monsterNumber.ToString() + "몬스터";
                    monsterNumber++;
                    monsters[createLimitCount] = monsterInstance.GetComponent<MonsterModel>(); 
                }
                else if(parserIndex % waveCount == 4)
                {
                    GameObject bossMonsterInstance = Instantiate(bosslMonsterPrefab[curThirdClass - 1], monsterSpawnPoint.position + offset, monsterSpawnPoint.rotation);
                    Collider2D bossMonsterCollider = bossMonsterInstance.GetComponent<Collider2D>();
                    bossMonsterCollider.enabled = false;

                    bossMonsterInstance.gameObject.name = monsterNumber.ToString() + "보스 몬스터";
                    monsterNumber++;
                    monsters[createLimitCount] = bossMonsterInstance.GetComponent<MonsterModel>();
                }


                stageDifficult.GetMonsterInstance(monsters[createLimitCount]);
                stageDifficult.MonsterIncreaseAbility();

                createLimitCount++;
            }
            yield return CoroutineManager.Instance.GetWaitForSeconds(createTimer);

        }
        
        //TestSet 정보창에서 현재 스테이지 몬스터 스탯을 보여줌
        atkText.text = "ATK : " + stageDifficult.CurStageMonsterAtk().ToString();
        hpText.text = "HP : " + stageDifficult.CurStageMonsterHP().ToString();
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
    /// 현재 중분류 맵에서 난이도가 True 인 값을 찾아서 현재 난이도에 할당
    /// </summary>
    public void SetDifficult()
    {
        switch (stageCSV.State[parserIndex].stage_FirstClass)
        {
            case StageData.Stage_firstClass.쉬움:
                curDifficult = Difficutly.Easy;
                break;

            case StageData.Stage_firstClass.보통:
                curDifficult = Difficutly.Normal;
                break;

            case StageData.Stage_firstClass.어려움:
                curDifficult = Difficutly.Hard;
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


    public void SetStageText()
    {
        int curWave = stageCSV.State[parserIndex].Stage_wave; 

        stageInfoText.text = curDifficult.ToString() + " Stage " + curThirdClass.ToString() + " - " + curWave.ToString();
    }


    public void TestActive()
    {
        ped.position = Input.mousePosition;
        results.Clear();
        ray.Raycast(ped, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.name == "TestSet")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    testSet.gameObject.SetActive(true); 
                }
            }
        }
    }

    /// <summary>
    /// Stage Test Mode
    /// </summary> 
    public void TestMode()
    {
        bool isCheck = false;
         
        //CoroutineManager.Instance.ManagerCoroutineStop(createCoroutineName);
        CoroutineManager.Instance.ManagerCoroutineStop(this);

        while (true)
        {
            foreach (MonsterModel model in monsters)
            {
                if (model != null)
                {
                    isCheck = false;
                    Destroy(model.gameObject);
                }
                else
                {
                    isCheck = true;
                }
            }

            if (isCheck)
                break;
        }

        Array.Clear(monsters, 0, monsters.Length);

        isWave = false;  
        parserIndex = int.Parse(textIndex.text);
        killMonsterCount = 0;

        int startIndex = parserIndex - (parserIndex % waveCount);

        for (int i = startIndex; i < parserIndex; i++)
        {
            killMonsterCount += stageCSV.State[i].Stage_monsterNum;
        }

        int endIndex = waveCount - (parserIndex % waveCount);
        ThirdClassMonsterCount = 0;
        for (int i = startIndex; i < parserIndex + endIndex; i++)
        {
            ThirdClassMonsterCount += stageCSV.State[i].Stage_monsterNum;
        }

        killRate = ((float)killMonsterCount / ThirdClassMonsterCount) * 100f;
        fieldWaveMonsterCount = 0;

        curSecondClass = stageCSV.State[parserIndex].Stage_secondClass;
        mapController.BackGroundSpriteChange(curSecondClass);
        mapController.SkySpriteChange(curSecondClass); 
        parserIndex--;
 
    }



}
