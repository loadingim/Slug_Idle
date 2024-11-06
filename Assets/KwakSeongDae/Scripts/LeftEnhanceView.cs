using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeftEnhanceView : MonoBehaviour
{
    [Header("아이템 설정")]
    public TextMeshProUGUI titleText;
    public Sprite icon;
    public TextMeshProUGUI statUpText;
    public TextMeshProUGUI buyText;


    public void UpdateItem(string title, int itemLevel, Sprite icon, string statUpText, string buyText)
    {
        titleText.text = $"+{itemLevel} {title}";
        this.icon = icon;
        this.statUpText.text = statUpText;
        this.buyText.text = buyText;
    }
}
