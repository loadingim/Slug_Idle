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
    private void OnEnable()
    {
        player.SwapWeapon(gameObject);
    }
    public void shot()
    {
            GameObject bulletGameObj = Instantiate(bulletPrefab, player.muzzlePoint.transform.position, transform.rotation);
            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            player.bullets.Add(bulletGameObj);
            bullet.SetTarget(player.targetMonster);            
    }
}