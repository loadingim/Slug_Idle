using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;




public class Store : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [Header("Weapon")]
    [SerializeField] private Button heavyBtn;
    [SerializeField] private Button flameBtn;
    [SerializeField] private Button roketBtn;
    [SerializeField] private Button shotgunBtn;


    [Header("강화 정보")]
    [SerializeField] private TextMeshProUGUI enhanceNum;
    [SerializeField] private Image itemIconImageInfo;
    [SerializeField] private TextMeshProUGUI curAttackTextInfo;
    [SerializeField] private Button priceBtn; 

    [Header("Buy Popup")]
    [SerializeField] private GameObject buyPopup;
    [SerializeField] private Button BuyBtn;
    [SerializeField] private TextMeshProUGUI curAttackTextBuy;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Image itemIconImageBuy;
    [SerializeField] private List<ShopData> shopData = new List<ShopData>(); 
    private TextMeshProUGUI buyText;

    //WeaponInfo 자리
    private Test weaponInfoData;
    private PlayerDataModel playerDataModel;
    
    [Header("Store Button List")]
    [SerializeField] private List<Button> buttonList = new List<Button>();
    private bool[] weaponBools;

    private string itemName = "";
    private int itemPrice = 0;

    private Color tureColor = new Color(1f, 1f, 1f, 1f);
    private ShopData curShopData;
    private int shopIndex = 0;

    private void Awake()
    {

        //weaponInfodATA = weaponinfoData.Instace
        weaponInfoData = Test.Instance;

        heavyBtn.onClick.AddListener(Heavy);
        flameBtn.onClick.AddListener(Flame);
        roketBtn.onClick.AddListener(Roket);
        shotgunBtn.onClick.AddListener(ShotGun);
        BuyBtn.onClick.AddListener(ItemBuy);
        priceBtn.onClick.AddListener(UpGrade);
    }

    private void Start()
    { 
  
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].image.sprite = shopData[i].IconImage;

            //현재 구매한 아이템은 활성화 표시
            if (shopData[i].IsBuy)
            { 
                buttonList[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            } 
        }
         
        buyText = BuyBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        weaponInfoData = GetComponent<Test>();
        playerDataModel = FindObjectOfType<PlayerDataModel>();
    }

    private void Update()
    {
        curShopData = shopData[shopIndex];
    }

    private void Heavy()
    {
        shopIndex = 0;

        if (shopData[shopIndex].IsBuy)
        {
            ShowInfoEnhance(shopData[shopIndex]);
        }
        else
        {
            ItemBuyPopup(shopData[shopIndex]);
        }
    }
     
    private void Flame()
    {
        shopIndex = 1;

        if (shopData[shopIndex].IsBuy)
        {
            ShowInfoEnhance(shopData[shopIndex]);
        }
        else
        {
            ItemBuyPopup(shopData[shopIndex]);
        }
    }

    private void Roket()
    {
        shopIndex = 2;

        if (shopData[shopIndex].IsBuy)
        {
            ShowInfoEnhance(shopData[shopIndex]);
        }
        else
        {
            ItemBuyPopup(shopData[shopIndex]);
        }
    }

    private void ShotGun()
    {
        shopIndex = 3;


        if (shopData[shopIndex].IsBuy)
        {
            ShowInfoEnhance(shopData[shopIndex]);
        }
        else
        {
            ItemBuyPopup(shopData[shopIndex]);
        }
    }



    /// <summary>
    /// 타입 별 아이템 업그레이드
    /// </summary>
    private void UpGrade()
    {

        //기획 문의
        switch (curShopData.CurStoreType)
        {
            case StoreType.Weapon:
                Debug.Log("무기 강화 성공");
                curShopData.CurEnhance++;
                curShopData.IncAttack++;
                curShopData.EnhancePrice += 20;  
                //PlayerDataModel.Attack = curShopData.Inattack;

                break;

            case StoreType.Partner:

                break;

            case StoreType.Slug:

                break;

            case StoreType.Skill:

                break;
        }

        InfoUpdate();
    }

    /// <summary>
    /// 아이템 강화 후 정보 업데이트
    /// </summary>
    private void InfoUpdate()
    {
        enhanceNum.text = "+" + curShopData.CurEnhance.ToString() + " " + curShopData.ItenName;
        curAttackTextInfo.text = curShopData.IncAttack.ToString();
        TextMeshProUGUI priceText = priceBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        priceText.text = curShopData.EnhancePrice.ToString();
    }

    /// <summary>
    /// 선택한 아이템 강화 정보 
    /// </summary>
    /// <param name="shopData"></param>
    private void ShowInfoEnhance(ShopData shopData)
    {
        curShopData = shopData; 
        enhanceNum.text = "+" + shopData.CurEnhance.ToString() + " " + shopData.ItenName;
        itemIconImageInfo.sprite = shopData.IconImage;

        //PlayerModel.Attack Change
        curAttackTextInfo.text = shopData.IncAttack.ToString();
        TextMeshProUGUI priceText = priceBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        priceText.text = shopData.EnhancePrice.ToString();
    }


    /// <summary>
    /// 아이템 구매 팝업 정보
    /// </summary>
    /// <param name="shopData"></param>
    private void ItemBuyPopup(ShopData shopData)
    {
         
        buyPopup.SetActive(true);
        itemName = shopData.ItenName;
        curAttackTextBuy.text = shopData.IncAttack.ToString();
        descText.text = shopData.Desc;
        itemIconImageBuy.sprite = shopData.IconImage;
        itemPrice = shopData.Pirce;
        buyText.text = itemPrice.ToString() + " Buy";

        //PlayerDataModel.Money < itemPrice
        //Item Price > ShopData.ItemPrice
        if (3000 < itemPrice)
        {
            BuyBtn.interactable = false;
        }
        else
        {
            BuyBtn.interactable = true;
        }

    }

    /// <summary>
    /// 아이템 미소지 상태에서 아이템 구매 처리
    /// </summary>
    private void ItemBuy()
    {
        //playerDataModel.Money -= itemPrice;
 
        //구매한 아이템 확인 후 아이콘 활성화 표시
        //아이템 구매 상태 전환
        switch (itemName)
        {
            case "Heavy":
                curShopData.IsBuy = true;
                buttonList[0].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                break;

            case "Flame":
                curShopData.IsBuy = true;
                buttonList[1].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                break;

            case "Roket":
                curShopData.IsBuy = true;
                buttonList[2].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                break;

            case "ShotGun":
                curShopData.IsBuy = true;
                buttonList[3].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                break; 
        }

        buyPopup.SetActive(false);
         
    }


}
