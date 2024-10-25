using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject targetMonster = null;
    public float attackCooldown = 0f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int ammo;
    [SerializeField] float times;

    private void Start()
    {
        // 모델 데이터 : 공격 속도
        PlayerDataModel.Instance.AttackSpeed = 3f;
        Debug.Log(PlayerDataModel.Instance.AttackSpeed);
    }

    private void Update()
    {
        if (targetMonster == null || targetMonster.activeSelf != true)
        {
            targetMonster = null;
        }

        times = Time.time;

        // 타겟으로 지정된 몬스터가 비어있고 몬스터가 활성화가 아닐 시
        if (targetMonster == null || targetMonster.activeSelf != true)
        {
            ammo = 0;
            FindTarget();
        }

        if (targetMonster != null && Time.time >= attackCooldown)
        {
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
        PlayerDataModel.Instance.Health -= damage;
        if (PlayerDataModel.Instance.Health <= 0)
        {
            // Destroy(gameObject); < 죽었을 시 
        }
    }

    public void Attack()
    {
        if (targetMonster != null && ammo < 1)
        {
            ammo++;
            GameObject bulletGameObj = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            bullet.SetTarget(targetMonster);
            attackCooldown = Time.time + PlayerDataModel.Instance.AttackSpeed;
        }
    }
}