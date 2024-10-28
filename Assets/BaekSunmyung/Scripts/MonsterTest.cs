using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{collision.gameObject.name}");
 
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }

}
