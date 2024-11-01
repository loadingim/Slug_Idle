using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private bool isOpenInventory;
    [SerializeField] private float thisTimeScale;

    public bool IsOpenInventory
    {
        get { return isOpenInventory; }
        set { isOpenInventory = value; }
    }


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
    }


    /*
     *  if (GameManager.Instance.IsOpenInventory)
        {
            yield return new WaitUntil(() => !GameManager.Instance.IsOpenInventory);
        }
     */

}
