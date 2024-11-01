using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
