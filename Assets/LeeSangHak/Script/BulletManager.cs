using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public List<GameObject> bullets = new List<GameObject>();

    public void AddBullet(GameObject bullet)
    {
        bullets.Add(bullet);
    }

    public void ClearAllBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            if (bullet != null)
            {
                Destroy(bullet);
            }
        }
        bullets.Clear();
    }

    public void RemoveBullet(GameObject bullet)
    {
        bullets.Remove(bullet);
    }
}
