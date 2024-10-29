using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName ="CreateBoss/Boss")]
public class BossData : ScriptableObject
{
    [Header("Boss Sprite Data")]
    [SerializeField] private List<Sprite> bossIdleSprite = new List<Sprite>();
    public List<Sprite> BossIdleSprite { get { return bossIdleSprite; } }

    [SerializeField] private List<Sprite> bossMoveSprite = new List<Sprite>();
    public List<Sprite> BossMoveSprite { get { return bossMoveSprite; } }

    [SerializeField] private List<Sprite> bossAttackSprite = new List<Sprite>();
    public List<Sprite> BossAttackSprite { get { return bossAttackSprite; } }

    [SerializeField] private List<Sprite> bossDeathSprite = new List<Sprite>();
    public List<Sprite> BossDeathSprite { get { return bossDeathSprite; } }

    [SerializeField] private List<Sprite> bossSlugSprite = new List<Sprite>();
    public List<Sprite> BossSlugSprite { get {return bossSlugSprite; } }

}
