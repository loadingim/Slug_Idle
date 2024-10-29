using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct StageData
{
    public enum Difficutly { Easy, Normal, Hard, Hell1, Hell2, Hell3 }

    public int Stage_ID, Stage_secondClass, Stage_thirdClass, Stage_wave, Stage_monsterNum,
                Stage_monsterCategory, Stage_statusIncUnit, Stage_goldUnit;
    public float Stage_statusIncNum, Stage_statusIncdistributionPer, Stage_goldNum;
    public string eName, Stage_firstClass;
}

public class StageCSV : MonoBehaviour
{
    const string stagePath = "https://docs.google.com/spreadsheets/d/16tlgiV3qBJWd1WSHFwBYkwnP0dFQkOJm/export?gid=1337676358&format=csv";
    public List<StageData> State;

    private void Awake()
    {
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
        for (int y = 4; y < 250; y++)
        {
            StageData stageData = new StageData();

            string[] values = lines[y].Split(',', '\t');

            stageData.eName = values[0];
            stageData.Stage_ID = int.Parse(values[1]);
            stageData.Stage_firstClass = values[2];
            stageData.Stage_secondClass = int.Parse(values[3]);
            stageData.Stage_thirdClass = int.Parse(values[4]);
            stageData.Stage_wave = int.Parse(values[5]);
            stageData.Stage_monsterNum = int.Parse(values[6]);
            stageData.Stage_monsterCategory = int.Parse(values[7]);
            stageData.Stage_statusIncNum = float.Parse(values[8]);
            stageData.Stage_statusIncUnit = int.Parse(values[9]);
            stageData.Stage_statusIncdistributionPer = float.Parse(values[10]);
            stageData.Stage_goldNum = float.Parse(values[11]);
            stageData.Stage_goldUnit = int.Parse(values[12]);

            State.Add(stageData);
        }
    }
}
