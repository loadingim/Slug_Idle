using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressIconController : MonoBehaviour
{
    [Header("기본 설정")]
    [Tooltip("프로그레스바의 ForeGround")]
    [SerializeField] private Image progressForeground;
    [Tooltip("현재 프로그레스 아이콘")]
    [SerializeField] private RectTransform progressIcon;

    // Update is called once per frame
    void Update()
    {
        if (progressForeground != null && progressIcon != null)
        {
            var pos = progressForeground.rectTransform.anchoredPosition;
            var width = progressForeground.rectTransform.sizeDelta.x;
            progressIcon.anchoredPosition = new Vector3(pos.x + (width * progressForeground.fillAmount), pos.y);
        }
    }
}
