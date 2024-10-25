using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapController : MonoBehaviour
{

    [Header("배경 이미지 리스트")]
    [SerializeField] private List<GameObject> backgroundMaps = new List<GameObject>();
 

    [Header("배경 이미지 이동 속도")]
    [SerializeField] private float mapTranslateSpeed;     //Player MoveSpeed 고려

    [Header("맵 스테이지 데이터")]
    [SerializeField] private List<MapData> mapData = new List<MapData>();

    //테스트 코드 > 추후 변수명 수정 필요
    [SerializeField] private int viewMonsterCount = 0;
    [SerializeField] private int totalMonsterCount = 0;
    [SerializeField] private int killMonsterCount = 0;

    [SerializeField] private Fade fade;

    //변수명 수정 필요
    private float killRate = 0f;

    //배경 이미지 이동 끝 위치 xPos 
    private float endPosX = 0;

    //배경 이미지 개수
    private int backGroundCount = 0;

    //배경 이미지 초기화 위치
    private Vector3 startPos;

    //임시 변수명
    private MiddleMap curMiddleMap = MiddleMap.One;
    private int curSmallStage = 1;


    private bool isDeath;


    private void Awake()
    {
        backGroundCount = backgroundMaps.Count;

        endPosX = backgroundMaps[0].transform.localScale.x;

        //배경 이미지 개수에 따라 초기화 x위치 변경
        startPos = new Vector3(endPosX * (backGroundCount - 1), 0f);
    }

    private void Start()
    { 
        //BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite);
        //SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);
    }


    private void Update()
    {
        Debug.Log($"현재 스테이지:{curMiddleMap} - {curSmallStage}");

        //테스트 코드
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //viewMonsterCount--;
            killMonsterCount++;

            //스테이지 진행률
            killRate = ((float)killMonsterCount / totalMonsterCount) * 100;

        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            viewMonsterCount++;
        }


        TranslateBackGround();
        RePositionBackGround();
        StageClear();


        //isDeath 값은 추후 Player Life 값 받아올 필요가 있음
        if (isDeath)
        {
            ResetBackGround();
        }


    }


    /// <summary>
    /// 배경 이미지 시작 위치로 변경
    /// </summary>
    public void ResetBackGround()
    {

        for (int i = 0; i < backGroundCount; i++)
        {
            backgroundMaps[i].transform.position = new Vector3(endPosX * i, 0f, 0f);
         
        }

    }

    /// <summary>
    /// Stage 클리어 시 BackGround 설정 변경
    /// </summary>
    public void StageClear()
    {

        if (killRate >= 100f)
        {
            fade.FadeOut();
            ResetBackGround(); 
            NextStage();

            killMonsterCount = 0;
            killRate = 0f;
            viewMonsterCount = 1;
        }
        else
        {
            //맵 설정 완료됐으면 Fade In
            if (fade.IsFade)
            {
                fade.FadeIn();
            }
        }

    }

    public void NextStage()
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

            //Map Data의 Sprite Image로 변경
            //BackGroundSpriteChange(mapData[(int)curMiddleMap].BackGroundSprite); 
            //SkySpriteChange(mapData[(int)curMiddleMap].SkySprite);
        }
    }

    /// <summary>
    /// 배경 이미지 위치 이동
    /// </summary>
    public void TranslateBackGround()
    {
        //현재 맵 상에서 보이는 몬스터가 없을 경우에만 맵 이동 진행
        //추후 Player or Monster에서 감지된 Count를 받아 올 필요 있음
        if (viewMonsterCount == 0)
        {
            for (int i = 0; i < backGroundCount; i++)
            {
                backgroundMaps[i].transform.Translate(Vector3.left * mapTranslateSpeed * Time.deltaTime);
            }
        }

    }

    /// <summary>
    /// 배경 이미지 위치 우측
    /// </summary>
    public void RePositionBackGround()
    {
        for (int i = 0; i < backGroundCount; i++)
        {
            if (backgroundMaps[i].transform.localPosition.x <= -endPosX)
            {
                backgroundMaps[i].transform.localPosition = startPos;
            }
        }
    }

    /// <summary>
    /// Map Data의 맵 배경 Sprite 변경
    /// </summary>
    /// <param name="sprite">맵 단계 별 받아올 백그라운드 이미지</param>
    public void BackGroundSpriteChange(Sprite sprite)
    { 
        for (int i = 0; i < backGroundCount; i++)
        {
            SpriteRenderer render = backgroundMaps[i].GetComponent<SpriteRenderer>();
            render.sprite = sprite;
        }
    }

    /// <summary>
    /// Map data의 하늘 배경 Sprite 변경
    /// </summary>
    /// <param name="sprite">맵 단계 별 하늘 이미지</param>
    public void SkySpriteChange(Sprite sprite)
    {
        for(int i = 0; i < backGroundCount; i++)
        { 
            SpriteRenderer skyRen = backgroundMaps[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
            skyRen.sprite = sprite; 
        } 
    }



}
