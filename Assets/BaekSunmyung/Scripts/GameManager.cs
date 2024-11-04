using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
   [SerializeField] private bool isOpenInventory;
    public bool IsOpenInventory
    {
        get { return isOpenInventory; }
        set { isOpenInventory = value; }
    }
    

    [SerializeField] private TimeSpan offlineTime;   
    [SerializeField] private TimeSpan onlineTime;

    [SerializeField] private double offline;
    [SerializeField] private float spanTime;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        onlineTime = DateTime.UtcNow - DateTime.UnixEpoch;
         

        var now = DateTime.Now.ToLocalTime();
        var spanTime = (now - DateTime.UnixEpoch); 
        
        
    }
 
    private void Update()
    {
        if (isOpenInventory)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
         
        if (Input.GetMouseButtonDown(0))
        {
#if UNITY_EDITOR
            PlayerPrefs.SetInt("StageIndex",StageManager.Instance.StageIndex);

            offlineTime = (DateTime.UtcNow - DateTime.UnixEpoch); 
            offline = (double)offlineTime.TotalSeconds;

            Debug.Log($"유니티 종료:{offline}");
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
         

    }


}
