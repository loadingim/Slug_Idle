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
    const string monsterPath = "https://docs.google.com/spreadsheets/d/1IQlUt1E_mBeowAF0kYdPeLFND6kxM0qA/export?gid=1883674857&format=csv";
    [SerializeField] List<MonsterData> monster;

    private void Awake()
    {
        StartCoroutine(DownloadRoutine());
    }

    IEnumerator DownloadRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(monsterPath); // ��ũ�� ���ؼ� ������Ʈ�� �ٿ�ε� ��û
        yield return request.SendWebRequest();                  // ��ũ�� �����ϰ� �Ϸ�� ������ ���

        // �Ϸ�� ��Ȳ
        string receiveText = request.downloadHandler.text;      // �ٿ�ε� �Ϸ��� ������ �ؽ�Ʈ�� �б�

        Debug.Log(receiveText);

        string[] lines = receiveText.Split('\n');
        for (int y = 3; y < lines.Length; y++)
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