using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectDestroy : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.75f);
    }
}
