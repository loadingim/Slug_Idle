using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static StageData;

[System.Serializable]
public struct StageData
{
    public enum Stage_firstClass { 쉬움, 보통, 어려움 }

    public int Stage_iD, Stage_secondClass, Stage_thirdClass, Stage_wave, Stage_monsterNum,
                Stage_monsterCategory, Stage_attackUnit, Stage_hpUnit, Stage_goldUnit;
    public float Stage_AttackNum, Stage_hpNum, Stage_goldNum;
    public string eName;
    public Stage_firstClass stage_FirstClass;

}

public class StageCSV : MonoBehaviour
{
    const string stagePath = "https://docs.google.com/spreadsheets/d/1yrhRkrB5UQH2JDYT2_vz9RW6yRzVaYv2/export?gid=28064368&format=csv";
    public List<StageData> State;
    public static StageCSV Instance;
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
        UnityWebRequest request = UnityWebRequest.Get(stagePath); // 링크를 통해서 웹사이트에 다운로드 요청
        yield return request.SendWebRequest();                  // 링크를 진행하고 완료될 때까지 대기

        // 완료된 상황
        string receiveText = request.downloadHandler.text;      // 다운로드 완료한 파일을 텍스트로 읽기

        Debug.Log(receiveText);

        string[] lines = receiveText.Split('\n');
        for (int y = 5; y < lines.Length; y++)
        {
            StageData stageData = new StageData();

            string[] values = lines[y].Split(',', '\t');

            stageData.eName = values[0];
            stageData.Stage_iD = int.Parse(values[1]);
            Enum.TryParse(values[2], out stageData.stage_FirstClass);
            stageData.Stage_secondClass = int.Parse(values[3]);
            stageData.Stage_thirdClass = int.Parse(values[4]);
            stageData.Stage_wave = int.Parse(values[5]);
            stageData.Stage_monsterNum = int.Parse(values[6]);
            stageData.Stage_monsterCategory = int.Parse(values[7]);
            stageData.Stage_AttackNum = float.Parse(values[8]);
            stageData.Stage_attackUnit = int.Parse(values[9]);
            stageData.Stage_hpNum = float.Parse(values[10]);
            stageData.Stage_hpUnit = int.Parse(values[11]);
            stageData.Stage_goldNum = float.Parse(values[12]);
            stageData.Stage_goldUnit = int.Parse(values[13]);

            State.Add(stageData);
        }

        downloadCheck = true;
    }
}
