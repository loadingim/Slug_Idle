using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct StageData
{
    public int Stage_ID, Stage_secondClass, Stage_thirdClass, Stage_wave, Stage_monsterNumCal,
                Stage_monsterNumCorrec, Stage_upStatusNum, Stage_monsterCategory, Stage_upStatusUnit;
    public float Stage_upStatus;
    public string Stage_firstClass, eName;
}

public class StageCSV : MonoBehaviour
{
    const string stagePath = "https://docs.google.com/spreadsheets/d/1IQlUt1E_mBeowAF0kYdPeLFND6kxM0qA/export?gid=904019982&format=csv";
    [SerializeField] List<StageData> State;

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
        for (int y = 3; y < lines.Length; y++)
        {
            StageData stageData = new StageData();

            string[] values = lines[y].Split(',', '\t');

            stageData.eName = values[0];
            stageData.Stage_ID = int.Parse(values[1]);
            stageData.Stage_firstClass = values[2];
            stageData.Stage_secondClass = int.Parse(values[3]);
            stageData.Stage_thirdClass = int.Parse(values[4]);
            stageData.Stage_wave = int.Parse(values[5]);
            /*stageData.Stage_monsterNumCal = int.Parse(values[6]);
            stageData.Stage_MonsterNumCorrec = int.Parse(values[7]);
            stageData.Stage_monsterCategory = int.Parse(values[8]);
            stageData.Stage_upStatusNum = int.Parse(values[9]);
            stageData.Stage_upStatusUnit = int.Parse(values[10]);
            stageData.Stage_upStatus = float.Parse(values[11]);*/

            State.Add(stageData);

        }
    }
}
