using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugWeapon : MonoBehaviour
{
    public string name;
    public float damage;
    public int level;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject MetalSmoke;
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

        if (name == "Metal")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Metal_Level].AssistantDealer_attackPer);

        if (name == "Drill")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Drill_Level].AssistantDealer_attackPer);

        if (name == "Heil")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Heli_Level].AssistantDealer_attackPer);

        if (name == "Jet")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Jet_Level].AssistantDealer_attackPer);
        
        bullet.SetTarget(slugPlayer.targetMonster);
    }
}