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

        if (name == "Metal")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Metal_Level].AssistantDealer_attackPer);

        if (name == "Drill")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Drill_Level].AssistantDealer_attackPer);

        if (name == "Heil")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Heli_Level].AssistantDealer_attackPer);

        if (name == "Jet")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Jet_Level].AssistantDealer_attackPer);

        bullet.SetTarget(partnerPlayer.targetMonster);
    }
}
