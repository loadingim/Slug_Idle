using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfoData : MonoBehaviour
{
    public static WeaponInfoData Instance;


    [Header("관리 오브젝트")]
    [SerializeField] Weapon Heavy;
    [SerializeField] Weapon Flame;
    [SerializeField] Weapon Roket;
    [SerializeField] Weapon Shotgun;

    [Space (15f)]
    [SerializeField] SlugController Metal;
    [SerializeField] SlugController Drill;
    [SerializeField] SlugController Heli;
    [SerializeField] SlugController Jet;

    [Header("오브젝트 사용 가능 여부")]
    public bool useHeavy, useFlame, useRoket, useShotgun;

    [Header("레벨")]
    public int Heavy_Level = 0;
    public int Flame_Level = 0;
    public int Roket_Level = 0;
    public int Shotgun_Level = 0;

    [Space(15f)]
    public int Metal_Level = 0;
    public int Drill_Level = 0;
    public int Heli_Level = 0;
    public int Jet_Level = 0;

    private float Heavy_Num = 0;
    private float Flame_Num = 0;
    private float Roket_Num = 0;
    private float Shotgun_Num = 0;

    private float Metal_Num = 0;
    private float Drill_Num = 0;
    private float Heli_Num = 0;
    private float Jet_Num = 0;

    [Header("최종 데미지 합산")]
    public float weaponDamage;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log(Flame_Level);
        Heavy.damage = WeaponCSV.Instance.Weapon[Heavy_Level].Weapon_per;
        Flame.damage = WeaponCSV.Instance.Weapon[Flame_Level].Weapon_per;
        Roket.damage = WeaponCSV.Instance.Weapon[Roket_Level].Weapon_per;
        Shotgun.damage = WeaponCSV.Instance.Weapon[Shotgun_Level].Weapon_per;

        /*Metal.weaponPrefab.da = WeaponCSV.Instance.Weapon[Metal_Level].Weapon_per;*/
        /*Drill.damage = WeaponCSV.Instance.Weapon[Drill_Level].Weapon_per;*/
        /*Heli.damage = WeaponCSV.Instance.Weapon[Heli_Level].Weapon_per;*/
        /*Jet.damage = WeaponCSV.Instance.Weapon[Jet_Level].Weapon_per;*/
    }

    private void Update()
    {
        WeaponDamage();
    }

    private void WeaponDamage()
    {
        if (useHeavy == true)
        {
            Heavy_Num = WeaponCSV.Instance.Weapon[Heavy_Level].Weapon_per;
        }

        if (useFlame == true)
        {
            Flame_Num = WeaponCSV.Instance.Weapon[Flame_Level].Weapon_per;
        }

        if (useRoket == true)
        {
            Roket_Num = WeaponCSV.Instance.Weapon[Roket_Level].Weapon_per;
        }

        if (useShotgun == true)
        {
            Shotgun_Num = WeaponCSV.Instance.Weapon[Shotgun_Level].Weapon_per;
        }

        weaponDamage = Heavy_Num + Flame_Num + Roket_Num + Shotgun_Num;
    }

    private void SlugDamage()
    {

    }
}