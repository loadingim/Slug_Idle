using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    [SerializeField] private Stage stage;
    [Header("맵 스테이지 데이터")]
    [SerializeField] private List<MapData> mapData = new List<MapData>();

    [Header("배경 이미지 리스트")]
    [SerializeField] private List<GameObject> backgroundMaps = new List<GameObject>();

    [Header("배경 이미지 이동 속도")]
    [SerializeField] private float mapTranslateSpeed;     //Player MoveSpeed 고려

    [SerializeField] private Fade fade;

    [Header("Background Map Reset")]
    [SerializeField] private float backGroundResetSpeed;

    private int thirdIndex = 0;
    public int ThirdIndex { get { return thirdIndex; } set { thirdIndex = value; } }

    //배경 이미지 이동 끝 위치 xPos 
    private float endPosX = 0;

    //배경 이미지 개수
    private int backGroundCount = 0;

    private Action bgAction;

    //배경 이미지 초기화 위치
    private Vector3 startPos;

    private int viewMonsterCount;
    private bool isDeath;

    private CoroutineManager crManager;
    private Coroutine resetRoutine;
    private bool isChange;
    private string coroutineName = "ResetCoroutine";

    private void Awake()
    {
        backGroundCount = backgroundMaps.Count;

        endPosX = backgroundMaps[0].transform.localScale.x * 17.82f;

        //배경 이미지 개수에 따라 초기화 x위치 변경
        startPos = new Vector3(endPosX * (backGroundCount - 1), -2.29f);
    }

    private void Start()
    {
        crManager = CoroutineManager.Instance;
        bgAction = ResetBackGround;
        stage.BackGroundResetAction(bgAction);
    }


    private void Update()
    {
        TranslateBackGround();
        RePositionBackGround();

        if (fade.IsFade)
        {
            fade.FadeIn();

            if (isChange)
            {
                crManager.ManagerCoroutineStop(this);
            } 
        }
    }


    /// <summary>
    /// 배경 이미지 시작 위치로 변경
    /// </summary>
    public void ResetBackGround()
    {
        fade.FadeOut();
        Coroutine resetRoutine = StartCoroutine(MapResetCoroutine());
        crManager.ManagerCoroutineStart(resetRoutine, this);
    }


    /// <summary>
    /// 배경 이미지 위치 이동
    /// </summary>
    public void TranslateBackGround()
    {
        //현재 맵 상에서 보이는 몬스터가 없을 경우에만 맵 이동 진행
        if (stage.FieldWaveMonsterCount == 0)
        {
            //대분류 기준 4번째 보스 스테이지 스크롤링 x
            if (stage.SecondClass == 4 && stage.IsBoss)
            {
                RePositionBackGround();
                return;
            }
                 
            for (int i = 0; i < backGroundCount; i++)
            { 
                backgroundMaps[i].transform.Translate(Vector3.left * mapTranslateSpeed * Time.deltaTime);
            }
        } 
    }

    /// <summary>
    /// 배경 이미지 위치 우측 이동
    /// </summary>
    public void RePositionBackGround()
    {
        for (int i = 0; i < backGroundCount; i++)
        {
            if (backgroundMaps[i].transform.localPosition.x <= -endPosX)
            {
                backgroundMaps[i].transform.localPosition = new Vector3(33.56f, -2.29f);
            }
        }
    }

    /// <summary>
    /// Map Data의 맵 배경 Sprite 변경 
    /// </summary>
    /// <param name="index">대분루 별 맵 인덱스</param>
    public void BackGroundSpriteChange(int index)
    {
        for (int i = 0; i < backGroundCount; i++)
        {
            SpriteRenderer render = backgroundMaps[i].GetComponent<SpriteRenderer>();
 
            if (index == 4)
            { 
                if (thirdIndex <= 3)
                {
                    render.sprite = mapData[index - 1].BackGroundSprite[0];
                }
                else 
                { 
                    //
                    if(i == 1)
                        render.flipX = true;
                    render.sprite = mapData[index - 1].BackGroundSprite[1];
                } 
            }
            else
            {
                render.sprite = mapData[index - 1].BackGroundSprite[0];

            }
            
        }
    }

    /// <summary>
    /// Map data의 하늘 배경 Sprite 변경
    /// </summary>
    /// <param name="index">맵 단계별 받아올 인덱스</param>
    public void SkySpriteChange(int index)
    {
        for (int i = 0; i < backGroundCount; i++)
        {
            SpriteRenderer skyRen = backgroundMaps[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (mapData[index - 1].SkySprite != null)
            {
                skyRen.sprite = mapData[index - 1].SkySprite[0];
            }
            else
            { 
                skyRen.sprite = null;
            } 
        }
    }
     

    private IEnumerator MapResetCoroutine()
    {
        yield return CoroutineManager.Instance.GetWaitForSeconds(backGroundResetSpeed);

        if (!isChange)
        {
            //if (GameManager.Instance.IsOpenInventory)
            //{
            //    yield return new WaitUntil(() => !GameManager.Instance.IsOpenInventory);
            //}

            for (int i = 0; i < backGroundCount; i++)
            {
                backgroundMaps[i].transform.position = new Vector3(endPosX * i, -2.29f, 0f);
            }
        }

        isChange = true;

        yield break;

    }


}
