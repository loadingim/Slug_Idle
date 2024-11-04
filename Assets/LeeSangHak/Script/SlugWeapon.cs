using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugWeapon : MonoBehaviour
{
    public string name;
    public float damage;
    public int level;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] SlugController slugPlayer;

    private BulletManager bulletManager;

    private void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();
    }

    public void shot()
    {
        GameObject bulletGameObj = Instantiate(bulletPrefab, slugPlayer.muzzlePoint.transform.position, transform.rotation);
        if (bulletManager != null)
        {
            bulletManager.AddBullet(bulletGameObj);
        }

        SlugBullet bullet = bulletGameObj.GetComponent<SlugBullet>();
        bullet.SetDamage(damage);
        bullet.SetTarget(slugPlayer.targetMonster);
    }
}