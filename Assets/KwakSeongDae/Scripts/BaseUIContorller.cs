using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseUIContorller : MonoBehaviour
{
    [Header("UI 요소 설정")]
    [Header("ㄴ유저 정보 UI")]
    [SerializeField] Sprite userIcon;
    public Sprite UserIcon
    {
        get { return userIcon; }
        set { userIcon = value; OnUserIconChanged?.Invoke(userIcon); }
    }
    public UnityAction<Sprite> OnUserIconChanged;

    [SerializeField] private int battlePower;
    public int BattlePower
    {
        get { return battlePower; }
        set
        {
            battlePower = value;
            OnBattlePowerChanged?.Invoke(battlePower);
        }
    }
    public UnityAction<int> OnBattlePowerChanged;

    [Header("ㄴ스테이지 진행상황 UI")]
    [Tooltip("현재 스테이지 보스 아이콘")]
    [SerializeField] private Sprite bossIcon;
    public Sprite BossIcon
    {
        get { return bossIcon; }
        set
        {
            bossIcon = value;
            OnBossIconChanged?.Invoke(bossIcon);
        }
    }
    public UnityAction<Sprite> OnBossIconChanged;

    [Header("아이템 세팅 (씬 초기에만 작동, 넣은 순서대로 슬롯 인벤토리 아이템과 연동)")]
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private GameObject[] partners;
    [SerializeField] private GameObject[] slugs;

    [Header("\n\n(수정 X,프리팹에 설정 안된 경우 문의) UI 요소")]
    [Header("ㄴ플레이어 유저 UI")]
    [SerializeField] private Image userIconImage;
    [SerializeField] private TextMeshProUGUI battlePowerText;
    [Header("ㄴ스테이지 진행 UI")]
    [SerializeField] private Image bossIconImage;
    [SerializeField] private TextMeshProUGUI stageText;
    [Header("ㄴ인벤토리 UI")]
    [SerializeField] private Transform weaponInventory;
    [SerializeField] private Transform partnerInventory;
    [SerializeField] private Transform slugInventory;

    private void Awake()
    {
        // UI가 씬이 전환되도 사라지지 않도록 조치
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        OnUserIconChanged += ChangeUserIcon;
        OnBattlePowerChanged += ChangeBattlePowerText;
        OnBossIconChanged += ChangeBossIcon;

    }

    private void OnDisable()
    {
        OnUserIconChanged -= ChangeUserIcon;
        OnBattlePowerChanged -= ChangeBattlePowerText;
        OnBossIconChanged -= ChangeBossIcon;

    }

    private void Start()
    {
        // 각 인벤토리는 설정된 인벤토리 아이템들로 초기화
        if (weaponInventory != null && partnerInventory != null && slugInventory != null)
        {
            // 각 슬롯에 인벤토리 아이템 장착
            InventoryItem[] inventoryWeapons = weaponInventory.GetComponentsInChildren<InventoryItem>();
            for (int i = 0; i < inventoryWeapons.Length; i++)
            {
                if (weapons.Length <= i) break;
                inventoryWeapons[i].ItemObject = weapons[i];
                inventoryWeapons[i].SlotCheck();
            }
            InventoryItem[] inventoryPartners = partnerInventory.GetComponentsInChildren<InventoryItem>();
            for (int i = 0; i < inventoryPartners.Length; i++)
            {
                if (partners.Length <= i) break;
                inventoryPartners[i].ItemObject = partners[i];
                inventoryPartners[i].SlotCheck();
            }
            InventoryItem[] inventorySlugs = slugInventory.GetComponentsInChildren<InventoryItem>();
            for (int i = 0; i < inventorySlugs.Length; i++)
            {
                if (slugs.Length <= i) break;
                inventorySlugs[i].ItemObject = slugs[i];
                inventorySlugs[i].SlotCheck();
            }
        }

        ChangeUserIcon(userIcon);
        ChangeBattlePowerText(battlePower);
        ChangeBossIcon(bossIcon);
    }

    // 인스펙터에서 변경된 내용도 이벤트 호출되도록 설정
    private void OnValidate()
    {
        OnUserIconChanged?.Invoke(userIcon);
        OnBattlePowerChanged?.Invoke(battlePower);
        OnBossIconChanged?.Invoke(bossIcon);
    }

    // 유저 아이콘이 바뀌는 이벤트 처리 함수
    void ChangeUserIcon(Sprite newSprite)
    {
        // 유저 이미지의 아이콘 변경
        if (userIconImage != null)
        {
            userIconImage.sprite = newSprite;
        }
    }

    // 전투력 텍스트 바뀌는 이벤트 처리 함수
    void ChangeBattlePowerText (int newBattlePower)
    {
        // 전투력 텍스트 변경
        battlePowerText?.SetText(newBattlePower.ToString());
    }

    // 보스 아이콘이 바뀌는 이벤트 처리 함수
    void ChangeBossIcon(Sprite newSprite)
    {
        // 보스 이미지의 아이콘 변경
        if (bossIconImage != null)
        {
            bossIconImage.sprite = newSprite;
        }
    }
}
