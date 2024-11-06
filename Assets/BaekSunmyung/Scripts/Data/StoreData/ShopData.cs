using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName ="CreateStore/Shop")]
public class ShopData : ScriptableObject
{
    [SerializeField] private StoreType storeType;
    public StoreType CurStoreType { get { return storeType; } }

    [SerializeField] private Sprite iconImage;
    public Sprite IconImage { get { return iconImage; } }

    [SerializeField] private string itemName;
    public string ItenName { get { return itemName; } }

    [SerializeField] private int incAttack;
    public int IncAttack { get { return incAttack; } set { incAttack = value; } }

    [SerializeField] private string desc;
    public string Desc { get { return desc; } }

    [SerializeField] private int buyPrice;
    public int Pirce { get { return buyPrice; } }

    [SerializeField] private int curEnhance;
    public int CurEnhance { get { return curEnhance; } set { curEnhance = value; } }

    [SerializeField] private int enhancePrice;
    public int EnhancePrice { get { return enhancePrice; } set { enhancePrice = value; } }

    [SerializeField] private bool isBuy;
    public bool IsBuy { get { return isBuy; } set { isBuy = value; } }
}
