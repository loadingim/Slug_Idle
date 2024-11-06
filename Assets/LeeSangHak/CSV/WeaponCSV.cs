using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public struct WeaponData
{
    public enum Weapon_type { HeavyMachinegun, Shotgun, FlameShot, RocketRuncher }

    public int Weapon_iD, Weapon_level, Weapon_priceGoldUnit, Weapon_priceDiaNum, Weapon_diaNum;
    public float Weapon_per, Weapon_priceGoldNum, Weapon_diaPer;
    public string eName;

    public Weapon_type weapon_Type;
}

public class WeaponCSV : MonoBehaviour
{
    const string weaponPath = "https://docs.google.com/spreadsheets/d/1yrhRkrB5UQH2JDYT2_vz9RW6yRzVaYv2/export?gid=2042254668&format=csv";
    public List<WeaponData> Weapon;
    public static WeaponCSV Instance;
    public bool downloadCheck;

    private void Start()
    {
        downloadCheck = false;
    }

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

        StartCoroutine(DownloadRoutine());
    }

    IEnumerator DownloadRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(weaponPath); // 링크를 통해서 웹사이트에 다운로드 요청
        yield return request.SendWebRequest();                  // 링크를 진행하고 완료될 때까지 대기

        // 완료된 상황
        string receiveText = request.downloadHandler.text;      // 다운로드 완료한 파일을 텍스트로 읽기

        Debug.Log(receiveText);

        string[] lines = receiveText.Split('\n');
        for (int y = 5; y < lines.Length; y++)
        {
            WeaponData weaponData = new WeaponData();

            string[] values = lines[y].Split(',', '\t');

            weaponData.eName = values[0];
            weaponData.Weapon_iD = int.Parse(values[1]);
            Enum.TryParse(values[2], out weaponData.weapon_Type);
            weaponData.Weapon_level = int.Parse(values[3]);
            weaponData.Weapon_diaPer = float.Parse(values[4]);
            weaponData.Weapon_diaNum = int.Parse(values[5]);
            weaponData.Weapon_per = float.Parse(values[6]);
            weaponData.Weapon_priceGoldNum = float.Parse(values[7]);
            weaponData.Weapon_priceGoldUnit = int.Parse(values[8]);
            weaponData.Weapon_priceDiaNum = int.Parse(values[9]);
            
            Weapon.Add(weaponData);
        }

        downloadCheck = true;
    }
}