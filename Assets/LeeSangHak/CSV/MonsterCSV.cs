using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct MonsterData
{
    public int Enemy_iD, Enemy_hp, Enemy_atk;
    public float Enemy_atkSpeed;
    public string eName, Enemy_nameEng, Enemy_nameKor, Enemy_atkAnimation, Enemy_moveAnimation, Enemy_deadAnimation, Enemy_atkSound, Enemy_deadSound;
}

public class MonsterCSV : MonoBehaviour
{
    const string monsterPath = "https://docs.google.com/spreadsheets/d/16tlgiV3qBJWd1WSHFwBYkwnP0dFQkOJm/export?gid=1087246424&format=csv";
    [SerializeField] List<MonsterData> monster;

    private void Awake()
    {
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
        for (int y = 4; y < lines.Length; y++)
        {
            MonsterData monsterData = new MonsterData();

            string[] values = lines[y].Split(',', '\t');

            monsterData.eName = values[0];
            monsterData.Enemy_iD = int.Parse(values[1]);
            monsterData.Enemy_nameEng = values[2];
            monsterData.Enemy_nameKor = values[3];
            monsterData.Enemy_hp = int.Parse(values[4]);
            monsterData.Enemy_atk = int.Parse(values[5]);
            monsterData.Enemy_atkSpeed = float.Parse(values[6]);
            monsterData.Enemy_atkAnimation = values[7];
            monsterData.Enemy_moveAnimation = values[8];
            monsterData.Enemy_deadAnimation = values[9];
            monsterData.Enemy_atkSound = values[10];
            monsterData.Enemy_deadSound = values[11];

            monster.Add(monsterData);

        }
    }
}