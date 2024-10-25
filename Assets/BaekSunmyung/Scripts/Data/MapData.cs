using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(menuName = "CreateMap/Map")]
public class MapData : ScriptableObject
{
    [Header("맵 데이터")]
    [Tooltip("현재 중분류 맵")]
    [SerializeField] private MiddleMap middleStage;
    public MiddleMap MiddleStage { get { return middleStage; } }

    [Tooltip("소분류 최대 단계")]
    [SerializeField] private int maxSmallStage;
    public int MaxSmallStage { get { return maxSmallStage; } }

    [Tooltip("중분류 맵에 사용할 배경 이미지")]
    [SerializeField] private Sprite backGroundSprite;
    public Sprite BackGroundSprite { get { return backGroundSprite; } }


}