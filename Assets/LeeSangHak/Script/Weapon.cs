using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string name;
    public float damage, attackSpeed;
    public int level, index;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] PlayerController player;

    private BulletManager bulletManager;

    private void Start()
    {
        if (gameObject.name == "Heavy")
        {
            level = 1;
            index = 0;
            damage = PlayerDataModel.Instance.Attack;
        }

        bulletManager = FindObjectOfType<BulletManager>();
    }

    private void OnEnable()
    {
        if (player != null)
            player.SwapWeapon(gameObject);
    }

    public void shot()
    {
            GameObject bulletGameObj = Instantiate(bulletPrefab, player.muzzlePoint.transform.position, transform.rotation);
            if (bulletManager != null)
            {
                bulletManager.AddBullet(bulletGameObj);
            }

            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            bullet.SetTarget(player.targetMonster);
    }
}