using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerWeapon : MonoBehaviour
{
    public string name;
    public float damage, attackSpeed;
    public int level;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] PartnerContorller partnerPlayer;

    private BulletManager bulletManager;

    private void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();
    }

    public void shot()
    {
            GameObject bulletGameObj = Instantiate(bulletPrefab, partnerPlayer.muzzlePoint.transform.position, transform.rotation);
            if (bulletManager != null)
            {
                bulletManager.AddBullet(bulletGameObj);
            }

        PartnerBullet bullet = bulletGameObj.GetComponent<PartnerBullet>();
        bullet.SetDamage(damage);
        bullet.SetTarget(partnerPlayer.targetMonster);
    }
}
