using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(menuName = "CreateMap/Map")]
public class MapData : ScriptableObject
{
    [Header("맵 데이터")]
    [Tooltip("중분류 맵에 사용할 배경 이미지")]
    [SerializeField] private List<Sprite> backGroundSprite = new List<Sprite>();
    public List<Sprite> BackGroundSprite { get { return backGroundSprite; } }

    [Tooltip("중분류 맵에 사용할 하늘 이미지")]
    [SerializeField] private List<Sprite> skySprite;
    public List<Sprite> SkySprite { get { return skySprite; } }
    


}