using System;
using TMPro;
using UnityEngine;

public class StatView : MonoBehaviour
{
    [Serializable]
    struct StatTextView
    {
        public string prefixText;
        public Stat stat;
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

    void UpdateView(StatTextView textView)
    {
        if (playerDataModel == null) return;
        switch (textView.stat)
        {
            case Stat.Health:
                textView.textMesh.text = textView.prefixText + playerDataModel.Health.ToString();
                break;
            case Stat.Attack:
                textView.textMesh.text = textView.prefixText + playerDataModel.Attack.ToString();
                break;
            case Stat.AttackSpeed:
                textView.textMesh.text = textView.prefixText + playerDataModel.AttackSpeed.ToString();
                break;
            default:
                break;
        }
    }
}
