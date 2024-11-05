using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfoData : MonoBehaviour
{
    public static WeaponInfoData Instance;
    public Weapon Heavy;
    public Weapon Flame;
    public Weapon Roket;
    public Weapon Shotgun;

    public int Heavy_Level;
    public int Flame_Level;
    public int Roket_Level;
    public int Shotgun_Level;

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
        Heavy.damage = WeaponCSV.Instance.Weapon[Heavy_Level].Weapon_per;
        Flame.damage = WeaponCSV.Instance.Weapon[Flame_Level].Weapon_per;
        Roket.damage = WeaponCSV.Instance.Weapon[Roket_Level].Weapon_per;
        Shotgun.damage = WeaponCSV.Instance.Weapon[Shotgun_Level].Weapon_per;
    }

    private void Update()
    {
        Heavy.damage = WeaponCSV.Instance.Weapon[Heavy_Level].Weapon_per;
        Flame.damage = WeaponCSV.Instance.Weapon[Flame_Level].Weapon_per;
        Roket.damage = WeaponCSV.Instance.Weapon[Roket_Level].Weapon_per;
        Shotgun.damage = WeaponCSV.Instance.Weapon[Shotgun_Level].Weapon_per;
    }
}