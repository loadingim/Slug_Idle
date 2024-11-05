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

        if (name == "Eri")
        {
            Debug.Log(PlayerDataModel.Instance.Attack / SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Eri_Level].AssistantDealer_attackSpdPer);
        bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Eri_Level].AssistantDealer_attackPer);
        }
        if (name == "Fio")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Fio_Level].AssistantDealer_attackPer);

        if (name == "Marco")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Marco_Level].AssistantDealer_attackPer);

        if (name == "Tarma")
            bullet.SetDamage(PlayerDataModel.Instance.Attack * SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Tarma_Level].AssistantDealer_attackPer);

        bullet.SetTarget(partnerPlayer.targetMonster);
    }
}
