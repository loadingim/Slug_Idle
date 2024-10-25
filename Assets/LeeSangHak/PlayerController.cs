using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    private PlayerDataModel dataModel;
    [SerializeField] GameObject targetMonster = null;
    public float attackCooldown = 0f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int ammo;

    private void Start()
    {
        dataModel.AttackSpeed = 10;
    }

    private void Update()
    {
        Debug.Log("진행중");
        if (targetMonster == null || targetMonster.activeSelf != true)
        {
            ammo = 0;
            Debug.Log("찾는중");
            FindTarget();
        }

        if (targetMonster != null && Time.time >= attackCooldown)
        {
            Debug.Log("공격중");
            Attack();
        }
    }

    private void FindTarget()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float distance = Vector2.Distance(transform.position, monster.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetMonster = monster;
            }
        }
    }


    public void TakeHit(int damage)
    {
        dataModel.Health -= damage;
        if (dataModel.Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Attack()
    {

        Debug.Log("공격진입");
        if (targetMonster != null && ammo < 5)
        {
            ammo++;
            Debug.Log("공격시작");
            GameObject bulletGameObj = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            bullet.SetTarget(targetMonster);
            attackCooldown = Time.time + dataModel.AttackSpeed;
        }
    }
}