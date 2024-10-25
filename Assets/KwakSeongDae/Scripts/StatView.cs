using System;
using TMPro;
using UnityEngine;

public class StatView : MonoBehaviour
{
    [Serializable]
    struct StatTextView
    {
        public string prefixText;
        public PlayerData stat;
        public TextMeshProUGUI textMesh;
    }

    [SerializeField] private StatTextView[] textViews;

    private PlayerDataModel playerDataModel;

    void Start()
    {
        playerDataModel = PlayerDataModel.Instance;
        if (playerDataModel == null)
        {
            Debug.Log("PlayerDataModel이 있는 경우에 스탯 관련 시스템이 활성화됩니다.");
            return;
        }
    }

    void Update()
    {
        foreach (var textView in textViews)
        {
            UpdateView(textView);
        }
    }

    /// <summary>
    /// 등록된 스탯 수치 텍스트를 모델에 저장된 수치와 동기화시키는 함수
    /// </summary>
    /// <param name="textView"></param>
    void UpdateView(StatTextView textView)
    {
        if (playerDataModel == null) return;
        switch (textView.stat)
        {
            case PlayerData.Health:
                textView.textMesh.text = textView.prefixText + playerDataModel.Health.ToString();
                break;
            case PlayerData.Attack:
                textView.textMesh.text = textView.prefixText + playerDataModel.Attack.ToString("F2");
                break;
            case PlayerData.AttackSpeed:
                textView.textMesh.text = textView.prefixText + playerDataModel.AttackSpeed.ToString("F2");
                break;
            case PlayerData.Money:
                textView.textMesh.text = textView.prefixText + playerDataModel.Money.ToString();
                break;
            default:
                break;
        }
    }
}
