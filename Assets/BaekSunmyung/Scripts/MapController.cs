using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    [Header("배경 이미지 리스트")]
    [SerializeField] private List<GameObject> backgroundMaps = new List<GameObject>();

    [Header("배경 이미지 이동 속도")]
    [SerializeField] private float mapTranslateSpeed;     //Player MoveSpeed 고려

    //테스트 코드
    [SerializeField] private int viewMonsterCount = 0;
    [SerializeField] private int totalMonsterCount = 0;
    [SerializeField] private int killMonsterCount = 0;

    [SerializeField] private Fade fade;

    private float killRate = 0f;

    //배경 이미지 이동 끝 위치 xPos 
    private float endPosX = 0;

    //배경 이미지 개수
    private int backGroundCount = 0;

    //배경 이미지 초기화 위치
    private Vector3 startPos;

    private void Awake()
    {
        backGroundCount = backgroundMaps.Count;

        endPosX = backgroundMaps[0].transform.localScale.x;

        //배경 이미지 개수에 따라 초기화 x위치 변경
        startPos = new Vector3(endPosX * (backGroundCount - 1), 0f);
    }

    private void Update()
    {
        Debug.Log($"맵 진행률 :{killRate}");

        //테스트 코드
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //viewMonsterCount--;
            killMonsterCount++;
            killRate = ((float)killMonsterCount / totalMonsterCount) * 100;

        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            viewMonsterCount++;
        }




        TranslateBackGround();
        RePositionBackGround();
        ResetBackGround();
    }

    public void ResetBackGround()
    {   
        if(killRate >= 100f)
        {
            //fade.FadeOut();

            for(int i = 0; i < backGroundCount; i++)
            {
                backgroundMaps[i].transform.position = new Vector3(endPosX * i,0f, 0f);
            }

            //fade.FadeIn();

            //테스트 코드
            viewMonsterCount = 1;
        }


    }


    /// <summary>
    /// 배경 이미지 위치 이동
    /// </summary>
    public void TranslateBackGround()
    {
        //현재 맵 상에서 보이는 몬스터가 없을 경우에만 맵 이동 진행
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
    /// 맵 배경 Sprite 변경
    /// </summary>
    /// <param name="sprite">맵 단계 별 받아올 sprite 변수</param>
    public void ChangeSprite(Sprite sprite)
    {
        for (int i = 0; i < backGroundCount; i++)
        {
            SpriteRenderer render = backgroundMaps[i].GetComponent<SpriteRenderer>();
            render.sprite = sprite;
        }
    }


}
