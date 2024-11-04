using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(hitEffect, collision.transform.position, transform.rotation);
    }
}
