using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct MonsterData
{
    public enum Enemy_type 
    {
        모덴군, 일반몬스터1, 일반몬스터2, 일반몬스터3, 일반몬스터4, 일반몬스터5, 일반몬스터6,
        일반몬스터7, 일반몬스터8, 일반몬스터9, 일반몬스터10, 보스몬스터1, 보스몬스터2, 보스몬스터3, 보스몬스터4,
        보스몬스터5, 보스몬스터6, 보스몬스터7, 보스몬스터8, 보스몬스터9, 보스몬스터10
    }

    public int Enemy_iD, Enemy_atk, Enemy_hp;
    public float Enemy_atkSpd;
    public string eName, Enemy_name;

    public Enemy_type enemy_Type;

}

public class MonsterCSV : MonoBehaviour
{
    const string monsterPath = "https://docs.google.com/spreadsheets/d/1yrhRkrB5UQH2JDYT2_vz9RW6yRzVaYv2/export?gid=206694631&format=csv";
    public List<MonsterData> Monster;
    public static MonsterCSV Instance;
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
        UnityWebRequest request = UnityWebRequest.Get(monsterPath); // 링크를 통해서 웹사이트에 다운로드 요청
        yield return request.SendWebRequest();                  // 링크를 진행하고 완료될 때까지 대기

        // 완료된 상황
        string receiveText = request.downloadHandler.text;      // 다운로드 완료한 파일을 텍스트로 읽기

        Debug.Log(receiveText);

        string[] lines = receiveText.Split('\n');
        for (int y = 5; y < lines.Length; y++)
        {
            MonsterData monsterData = new MonsterData();

            string[] values = lines[y].Split(',', '\t');

            monsterData.eName = values[0];
            monsterData.Enemy_iD = int.Parse(values[1]);
            monsterData.Enemy_name = values[2];
            Enum.TryParse(values[3], out monsterData.enemy_Type);
            monsterData.Enemy_hp = int.Parse(values[4]);
            monsterData.Enemy_atk = int.Parse(values[5]);
            monsterData.Enemy_atkSpd = float.Parse(values[6]);

            Monster.Add(monsterData);
        }

        downloadCheck = true;
    }
}