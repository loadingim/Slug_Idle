using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;




public class Store : MonoBehaviour
{
    [Header("GR Ray")]
    private GraphicRaycaster ray;
    [SerializeField] private Canvas canvas;
    [SerializeField] private PointerEventData ped = new PointerEventData(EventSystem.current);
    private List<RaycastResult> results = new List<RaycastResult>();

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
    [SerializeField] private WeaponInfoData infoData;
    //private Test weaponInfoData;
    private PlayerDataModel playerDataModel;
    private GameManager gameManager;

    [Header("Store Button List")]
    [SerializeField] private List<Button> buttonList = new List<Button>();

    private string itemName = "";
    private int itemPrice = 0;

    private Color tureColor = new Color(1f, 1f, 1f, 1f);
    private ShopData curShopData;
    private int shopIndex = 0;
    private bool isSelect;



    private void Awake()
    { 
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].onClick.AddListener(Shop);
        }
        BuyBtn.onClick.AddListener(ItemBuy);
        priceBtn.onClick.AddListener(UpGrade);
    }

    private void OnDisable()
    {
        isSelect = false;
    }

    private void Start()
    { 
        gameManager = GameManager.Instance;
        infoData = WeaponInfoData.Instance;
        //weaponInfoData = Test.Instance;

        ray = canvas.GetComponent<GraphicRaycaster>();
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

        //WeaponInfoData
        //weaponInfoData = GetComponent<Test>();

        ItemBuyCheck();
        priceBtn.interactable = false;
        playerDataModel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataModel>(); 
    }

    private void Update()
    {
        ped.position = Input.mousePosition;
        results.Clear();
        ray.Raycast(ped, results);

    }

    private void ItemBuyCheck()
    {
        infoData.useHeavy = shopData[0].IsBuy;
        infoData.useFlame = shopData[1].IsBuy;
        infoData.useRoket = shopData[2].IsBuy;
        infoData.useShotgun = shopData[3].IsBuy;
        infoData.useMetal = shopData[4].IsBuy;
        infoData.useDrill = shopData[5].IsBuy;
        infoData.useHeli = shopData[6].IsBuy;
        infoData.useJet = shopData[7].IsBuy;
        infoData.useFio = shopData[8].IsBuy;
        infoData.useEri = shopData[9].IsBuy;
        infoData.useMarco = shopData[10].IsBuy;
        infoData.useTarma = shopData[11].IsBuy; 

    }

    private void Shop()
    {
        if (results.Count > 0)
        {
            string name = results[0].gameObject.name;
            string[] strIndex = Regex.Replace(name, @"[^0-9]", " ").Split(' ');
            shopIndex = Convert.ToInt32((strIndex[strIndex.Length - 1]));
        }

        curShopData = shopData[shopIndex];

        if (shopData[shopIndex].IsBuy)
        {
            ShowInfoEnhance();
        }
        else
        {
            ItemBuyPopup();
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
                break;

            case StoreType.Partner:
                Debug.Log("파트너 강화 성공");
                curShopData.CurEnhance++;
                curShopData.IncAttack++;
                curShopData.EnhancePrice += 20;
                break;

            case StoreType.Slug:
                Debug.Log("슬러그 강화 성공");
                curShopData.CurEnhance++;
                curShopData.IncAttack++;
                curShopData.EnhancePrice += 20;
                break;

            case StoreType.Skill:
                Debug.Log("스킬 강화 성공");
                curShopData.CurEnhance++;
                curShopData.IncAttack++;
                curShopData.EnhancePrice += 20;
                break;
        }

        playerDataModel.Money -= curShopData.EnhancePrice;

        switch (shopIndex)
        {
            case 0:
                infoData.Heavy_Level = curShopData.CurEnhance;
                break;
            case 1:
                infoData.Flame_Level = curShopData.CurEnhance;
                break;
            case 2:
                infoData.Roket_Level = curShopData.CurEnhance;
                break;
            case 3:
                infoData.Shotgun_Level = curShopData.CurEnhance;
                break;

            case 4:
                infoData.Metal_Level = curShopData.CurEnhance;
                break;
            case 5:
                infoData.Drill_Level = curShopData.CurEnhance;
                break;

            case 6:
                infoData.Heli_Level = curShopData.CurEnhance;
                break;

            case 7:
                infoData.Jet_Level = curShopData.CurEnhance;
                break;

            case 8:
                infoData.Marco_Level = curShopData.CurEnhance;
                break;

            case 9:
                infoData.Eri_Level = curShopData.CurEnhance;
                break;
            case 10:
                infoData.Tarma_Level = curShopData.CurEnhance;
                break;
            case 11:
                infoData.Fio_Level = curShopData.CurEnhance;
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
        //curAttackTextInfo.text = curShopData.IncAttack.ToString();
        curAttackTextInfo.text = infoData.weaponDamage.ToString();
        TextMeshProUGUI priceText = priceBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        priceText.text = curShopData.EnhancePrice.ToString();

        if (10 < curShopData.EnhancePrice)
        {
            priceBtn.interactable = false;
        }
        else
        {
            priceBtn.interactable = true;
        } 
    }

    /// <summary>
    /// 선택한 아이템 강화 정보 
    /// </summary>
    private void ShowInfoEnhance()
    {
        isSelect = !isSelect ? true : false;
        if (isSelect)
        {
            enhanceNum.text = "+" + curShopData.CurEnhance.ToString() + " " + curShopData.ItenName;
            itemIconImageInfo.color = Color.white;
            itemIconImageInfo.sprite = curShopData.IconImage;

            //PlayerModel.Attack Change
            //curAttackTextInfo.text = curShopData.IncAttack.ToString();
            curAttackTextInfo.text = infoData.weaponDamage.ToString();
            TextMeshProUGUI priceText = priceBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            priceText.text = curShopData.EnhancePrice.ToString();

            if (10 > curShopData.EnhancePrice)
            {
                priceBtn.interactable = true;
            }
        }
        else
        {
            enhanceNum.text = "";
            itemIconImageInfo.color = new Color(0, 0, 0, 0);
            curAttackTextInfo.text = "";
            TextMeshProUGUI priceText = priceBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            priceText.text = "Price";

        }

        //선택하지 않은 상태 or 보유 머니보다 아이템이 비싸면 상호작용 불가
        if (!isSelect || 10 < curShopData.EnhancePrice)
        {
            priceBtn.interactable = false;
        }

    }


    /// <summary>
    /// 아이템 구매 팝업 정보
    /// </summary>
    private void ItemBuyPopup()
    {

        buyPopup.SetActive(true);
        curAttackTextBuy.text = curShopData.IncAttack.ToString();
        descText.text = curShopData.Desc;
        itemIconImageBuy.sprite = curShopData.IconImage;
        itemPrice = curShopData.Pirce;
        buyText.text = itemPrice.ToString() + " Buy";

        //PlayerDataModel.Money < itemPrice
        //Item Price > ShopData.ItemPrice
        if (playerDataModel.Money < itemPrice)
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
        curShopData.IsBuy = true;
        buttonList[shopIndex].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        playerDataModel.Money -= itemPrice;
        buyPopup.SetActive(false);

    }


}
