using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressIconController : MonoBehaviour
{
    [Header("기본 설정")]
    [Tooltip("프로그레스바의 ForeGround")]
    [SerializeField] private Image progressForeground;
    [Tooltip("ForeGround의 자식에 있는 프로그레스 아이콘\n 부모의 Anchor와 같도록 사전 설정 필요")]
    [SerializeField] private RectTransform progressIcon;
    [Tooltip("아이콘 위치 오프셋")]
    [SerializeField] private Vector2 offset;

    // Update is called once per frame
    void Update()
    {
        if (progressForeground != null && progressIcon != null)
        {
            var width = progressForeground.rectTransform.sizeDelta.x;
            progressIcon.anchoredPosition = new Vector3(width * progressForeground.fillAmount + offset.x, offset.y);
        }
    }
}
