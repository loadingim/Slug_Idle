using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour
{

    [SerializeField] private PlayerDataModel player;
    
    private void Start()
    {
        player = GetComponent<PlayerDataModel>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Health = 0;
            
        }
    }

}
