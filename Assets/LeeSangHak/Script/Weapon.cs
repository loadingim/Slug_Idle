using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string name;
    public float damage, attackSpeed;
    public int level;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] PlayerController player;
    [SerializeField] TeamContorller teamPlayer;
    [SerializeField] SlugController slugPlayer;

    private BulletManager bulletManager;

    private void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();
    }

    private void OnEnable()
    {
        if (player != null)
            player.SwapWeapon(gameObject);
    }

    public void shot()
    {
        if (teamPlayer != null)
        {
            GameObject bulletGameObj = Instantiate(bulletPrefab, teamPlayer.muzzlePoint.transform.position, transform.rotation);
            if (bulletManager != null)
            {
                bulletManager.AddBullet(bulletGameObj);
            }

            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            bullet.SetTarget(teamPlayer.targetMonster);
        }

        if (player != null)
        {
            GameObject bulletGameObj = Instantiate(bulletPrefab, player.muzzlePoint.transform.position, transform.rotation);
            if (bulletManager != null)
            {
                bulletManager.AddBullet(bulletGameObj);
            }

            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            bullet.SetTarget(player.targetMonster);
        }

        if (slugPlayer != null)
        {
            GameObject bulletGameObj = Instantiate(bulletPrefab, slugPlayer.muzzlePoint.transform.position, transform.rotation);
            if (bulletManager != null)
            {
                bulletManager.AddBullet(bulletGameObj);
            }

            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            bullet.SetTarget(slugPlayer.targetMonster);
        }



    }
}