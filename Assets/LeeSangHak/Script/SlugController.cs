using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SlugController : MonoBehaviour
{
    public GameObject targetMonster = null;
    public float attackCooldown = 0f;
    [SerializeField] GameObject weaponPrefab;
    public int attackRange;
    [SerializeField] float times;
    [SerializeField] GameObject[] monsters;
    public GameObject muzzlePoint;
    [SerializeField] Animator slugAnim;
    private float attackSpeed;


    private void Start()
    {
        // 모델 데이터 : 공격 속도 및 사거리
        attackSpeed = 1f;
        attackRange = 20;
    }



    private void Update()
    {
        // if (Time.timeScale == 0)
        // {
        times = Time.time;

        // 타겟으로 지정된 몬스터가 비어있고 몬스터가 활성화가 아닐 시
        if (targetMonster == null || !targetMonster.activeSelf)
        {
            FindTarget();
        }

        if (targetMonster != null && Time.time >= attackCooldown)
        {
            Attack();
        }
        //  }

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

    public void Attack()
    {
        if (targetMonster != null && Vector2.Distance(transform.position, targetMonster.transform.position) < attackRange)
        {


            ShootBullet();

            attackCooldown = Time.time + PlayerDataModel.Instance.AttackSpeed;
        }
    }

    private void ShootBullet()
    {

        weaponPrefab.GetComponent<Weapon>().shot();

    }
}