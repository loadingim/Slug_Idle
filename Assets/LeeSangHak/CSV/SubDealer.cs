using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public struct SubDealerData
{
    public enum AssistantDealer_category { Companion, Slug }

    public int AssistantDealer_iD, AssistantDealer_level, AssistantDealer_priceUnit;
    public float AssistantDealer_attackPer, AssistantDealer_attackSpdPer, AssistantDealer_priceNum;
    public string eName, AssistantDealer_name;

    public AssistantDealer_category assistantDealer_Category;
}

public class SubDealer : MonoBehaviour
{
    const string subDealerPath = "https://docs.google.com/spreadsheets/d/1yrhRkrB5UQH2JDYT2_vz9RW6yRzVaYv2/export?gid=2127461744&format=csv";
    public List<SubDealerData> SubDealers;
    public static SubDealer Instance;
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
        UnityWebRequest request = UnityWebRequest.Get(subDealerPath); // 링크를 통해서 웹사이트에 다운로드 요청
        yield return request.SendWebRequest();                  // 링크를 진행하고 완료될 때까지 대기

        // 완료된 상황
        string receiveText = request.downloadHandler.text;      // 다운로드 완료한 파일을 텍스트로 읽기

        Debug.Log(receiveText);

        string[] lines = receiveText.Split('\n');
        for (int y = 5; y < lines.Length; y++)
        {
            SubDealerData subDealerData = new SubDealerData();

            string[] values = lines[y].Split(',', '\t');

            subDealerData.eName = values[0];
            subDealerData.AssistantDealer_iD = int.Parse(values[1]);
            subDealerData.AssistantDealer_name = values[2];
            Enum.TryParse(values[3], out subDealerData.assistantDealer_Category);            
            subDealerData.AssistantDealer_level = int.Parse(values[4]);
            subDealerData.AssistantDealer_attackPer = float.Parse(values[5]);
            subDealerData.AssistantDealer_attackSpdPer = float.Parse(values[6]);
            subDealerData.AssistantDealer_priceNum = float.Parse(values[7]);
            subDealerData.AssistantDealer_priceUnit = int.Parse(values[8]);

            SubDealers.Add(subDealerData);
        }

        downloadCheck = true;
    }
}