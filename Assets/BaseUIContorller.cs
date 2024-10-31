using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIContorller : MonoBehaviour
{
    private void Awake()
    {
        // UI가 씬이 전환되도 사라지지 않도록 조치
        DontDestroyOnLoad(gameObject);
    }
}
