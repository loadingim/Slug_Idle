using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string name;
    public float damage, attackSpeed;
    public int level;
    [SerializeField] GameObject bulletPrefabs;
    private void OnEnable()
    {
        if (name == "F")
        {
           /* damge = WeaponCSV.Instant.weapon[0].attak;
            attackSpeed = WeaponCSV.Instant.weapon[0].attakspeed;*/
        }

        if (name == "S")
        {
            //PlayerDataModel.Instance.wAttack = 300;
        }

        if (name == "L")
        {
            //PlayerDataModel.Instance.wAttack = 500;
        }

        if (name == "H")
        {
            //PlayerDataModel.Instance.wAttack = 150;
            int x = 1;

            float y = 2.5f;

            x += (int)y;
        }
    }
 /*   public void shot()
    {
        Debug.Log("╬Нец");

            GameObject bulletGameObj = Instantiate(bulletPrefabs, muzzlePoint.transform.position, transform.rotation);
            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            bullet.SetTarget(targetMonster);
            attackCooldown = Time.time + PlayerDataModel.Instance.AttackSpeed;
    }*/
}