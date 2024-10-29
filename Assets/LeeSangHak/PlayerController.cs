using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public GameObject targetMonster = null;
    public float attackCooldown = 0f;
    [SerializeField] GameObject bulletPrefab;
    public int ammo, attackRange;
    [SerializeField] float times;
    [SerializeField] GameObject[] monsters;
    

    private void Start()
    {
        // 모델 데이터 : 공격 속도
        attackRange = 10;
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
            FindTarget();
        }

        if (targetMonster != null && Time.time >= attackCooldown)
        {
            Attack();
        }
    }

    private void FindTarget()
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");



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
            Debug.Log("피격됨");
        }
    }

    public void Attack()
    {
        Debug.Log("어택");
        if (targetMonster != null && Vector2.Distance(transform.position, targetMonster.transform.position) < attackRange)
        {
            Debug.Log("발사");
            GameObject bulletGameObj = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            bullet.SetTarget(targetMonster);
            attackCooldown = Time.time + PlayerDataModel.Instance.AttackSpeed;
        }
    }
}