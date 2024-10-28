using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    [SerializeField] private Stage stage;

    [Header("배경 이미지 리스트")]
    [SerializeField] private List<GameObject> backgroundMaps = new List<GameObject>();
  
    [Header("배경 이미지 이동 속도")]
    [SerializeField] private float mapTranslateSpeed;     //Player MoveSpeed 고려
 
    [SerializeField] private Fade fade;

    //배경 이미지 이동 끝 위치 xPos 
    private float endPosX = 0;

    //배경 이미지 개수
    private int backGroundCount = 0;

    private Action bgAction;

    //배경 이미지 초기화 위치
    private Vector3 startPos;

    private int viewMonsterCount;
    private bool isDeath;

    private Coroutine resetRoutine;
    private bool isChange;

    private void Awake()
    {
        backGroundCount = backgroundMaps.Count;

        endPosX = backgroundMaps[0].transform.localScale.x;

        //배경 이미지 개수에 따라 초기화 x위치 변경
        startPos = new Vector3(endPosX * (backGroundCount - 1), 0f);
    }

    private void Start()
    {
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

            if(isChange && resetRoutine != null)
            {
                Debug.Log("코루틴 중지");
                StopCoroutine(resetRoutine);
                resetRoutine = null;
                isChange = false;
            }

        }
    }


    /// <summary>
    /// 배경 이미지 시작 위치로 변경
    /// </summary>
    public void ResetBackGround()
    {
        fade.FadeOut();

        resetRoutine = StartCoroutine(ResetCo());
    }
  

    /// <summary>
    /// 배경 이미지 위치 이동
    /// </summary>
    public void TranslateBackGround()
    {
        //현재 맵 상에서 보이는 몬스터가 없을 경우에만 맵 이동 진행
        //추후 Player or Monster에서 감지된 Count를 받아 올 필요 있음
        // or Player가 이동 상태일 경우 이동하는 방식으로 수정
        if (viewMonsterCount == 0)
        {
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
    
    private IEnumerator ResetCo()
    {
        //맵이 재배치되기 전 대기 시간
        //추후 player 가 준비 상태를 받아 올 수 있으면 대기 시간은 필요 없음
        yield return new WaitForSeconds(1.5f);
 
        if (!isChange)
        {
            for (int i = 0; i < backGroundCount; i++)
            {
                backgroundMaps[i].transform.position = new Vector3(endPosX * i, 0f, 0f);
            } 
        }

        isChange = true;

        yield break;

    }


}
