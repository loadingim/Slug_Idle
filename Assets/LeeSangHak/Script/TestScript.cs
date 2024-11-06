using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    private void Update()
    {
        if (BulletCSV.Instance.downloadCheck == true && MonsterCSV.Instance.downloadCheck == true && StageCSV.Instance.downloadCheck == true && StoreCSV.Instance.downloadCheck == true && WeaponCSV.Instance.downloadCheck)
        {
            SceneManager.LoadScene("Test");
        }
    }
}
