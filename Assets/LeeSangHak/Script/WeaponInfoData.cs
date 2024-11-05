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

    public bool a,b,c,d;

    public int Heavy_Level = 0;
    public int Flame_Level = 0;
    public int Roket_Level = 0;
    public int Shotgun_Level = 0;

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
    }

    private void Update()
    {

        if (a == true)
        {

        }


        Heavy.damage = WeaponCSV.Instance.Weapon[Heavy_Level].Weapon_per;
        Flame.damage = WeaponCSV.Instance.Weapon[Flame_Level].Weapon_per;
        Roket.damage = WeaponCSV.Instance.Weapon[Roket_Level].Weapon_per;
        Shotgun.damage = WeaponCSV.Instance.Weapon[Shotgun_Level].Weapon_per;

        if (Input.anyKeyDown)
        {
            Heavy_Level++;
            Flame_Level++;
            Roket_Level++;
            Shotgun_Level++;
        }
    }
}