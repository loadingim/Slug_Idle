using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    [Header("체크박스 기본 설정")]
    [Tooltip("체크박스 클릭 역할을 할 버튼")]
    [SerializeField] private Button checkBoxButton;
    [Tooltip("체크박스 이미지")]
    [SerializeField] private Image checkBoxImage;
    [Tooltip("체크박스 해제 상태 스프라이트")]
    [SerializeField] private Sprite uncheckSprite;
    [Tooltip("체크박스 체크 상태 스프라이트")]
    [SerializeField] private Sprite checkSprite;
    [Header("체크박스 현재 상태")]
    [Tooltip("체크박스 체크 상태")]
    public bool isCheck;

    // Start is called before the first frame update
    void Start()
    {
        if (checkBoxButton != null)
        {
            checkBoxButton.onClick.AddListener(ToggleCheckbox);
            UpdateImage();
        }
    }

    /// <summary>
    /// 체크박스 클릭시, 체크 상태 토글하는 함수
    /// </summary>
    public void ToggleCheckbox()
    {
        if (checkBoxButton == null) return;
        // 체크 상태 바꾸기 (토글)
        isCheck ^= true;
        UpdateImage();
    }

    /// <summary>
    /// 체크 상태에 따라 이미지를 업데이트하는 함수
    /// </summary>
    void UpdateImage()
    {
        if (isCheck)
        {
            // 체크 이미지로 전환
            checkBoxImage.sprite = checkSprite;
        }
        else
        {
            // 체크 해제 이미지로 전환
            checkBoxImage.sprite = uncheckSprite;
        }
    }
}
