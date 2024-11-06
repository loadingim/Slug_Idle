using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public GameObject targetMonster = null;
    public float attackCooldown = 0f;
    [SerializeField] GameObject bulletPrefab;
    public GameObject weaponPrefab;
    public int attackRange;
    [SerializeField] float times;
    [SerializeField] GameObject[] monsters;
    public GameObject muzzlePoint;
    [SerializeField] Animator upperAnim;
    [SerializeField] Animator lowerAnim;
    private float attackSpeed;
    private bool respawn;
    private BulletManager bulletManager;

    private void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();

        // 모델 데이터 : 공격 속도 및 사거리
        attackSpeed = 1f / StoreCSV.Instance.Store[PlayerDataModel.Instance.AttackSpeedLevel + 4999].StatusStore_satatusNum;
        
        attackRange = 20;
    }



    private void Update()
    {
        // Debug.Log(1f / StoreCSV.Instance.Store[PlayerDataModel.Instance.AttackSpeedLevel + 4999].StatusStore_satatusNum);
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

        if (PlayerDataModel.Instance.Health <= 0)
        {
            Death();
        }
        
        if (weaponPrefab != null)
        {
            upperAnim.SetBool("isWeapon", true);

            if (!weaponPrefab.gameObject.activeSelf)
            {
                weaponPrefab = null;
            }
        }

        if (weaponPrefab == null)
        {
            upperAnim.SetBool("isWeapon", false);
        }


        if (respawn == true)
        {
            StopCoroutine(Respawn());
            respawn = false;
        }
        //  }

    }

    private void FindTarget()
    {
        upperAnim.SetBool("isAtk", false);
        lowerAnim.SetBool("isMove", true);


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
    }

    public void Attack()
    {
        if (targetMonster != null && Vector2.Distance(transform.position, targetMonster.transform.position) < attackRange)
        {
            lowerAnim.SetBool("isMove", false);
            upperAnim.SetBool("isAtk", true);
            upperAnim.SetFloat("speed", attackSpeed + (PlayerDataModel.Instance.AttackSpeedLevel * 0.01f));

            ShootBullet();

            attackCooldown = Time.time + 1f / StoreCSV.Instance.Store[PlayerDataModel.Instance.AttackSpeedLevel + 4999].StatusStore_satatusNum; ;
        }
    }

    public void RemoveBullets(GameObject bullet)
    {
        if (bulletManager != null)
        {
            bulletManager.RemoveBullet(bullet);
        }
    }


    public void Death()
    {
        StartCoroutine(Respawn());
    }

    public void SwapWeapon(GameObject weapon)
    {
        if (weaponPrefab != null)
        {
            weaponPrefab.gameObject.SetActive(false);
        }

        weaponPrefab = weapon;
        weaponPrefab.gameObject.SetActive(true);

    }

    private void ShootBullet()
    {
        if (weaponPrefab == null)
        {
            GameObject bulletGameObj = Instantiate(bulletPrefab, muzzlePoint.transform.position, transform.rotation);

            // 생성된 탄환을 BulletManager에 추가
            if (bulletManager != null)
            {
                bulletManager.AddBullet(bulletGameObj);
            }

            Bullet bullet = bulletGameObj.GetComponent<Bullet>();
            bullet.SetTarget(targetMonster);
        }
        else
        {
            weaponPrefab.GetComponent<Weapon>().shot();
        }
    }

    IEnumerator Respawn()
    {
        upperAnim.SetBool("Death", true);
        lowerAnim.gameObject.SetActive(false);

        if (bulletManager != null)
        {
            bulletManager.ClearAllBullets();
        }

        foreach (GameObject model in monsters)
        {
            Destroy(model.gameObject);
        }
        
        
        yield return new WaitForSeconds(1f);

        PlayerDataModel.Instance.Health = PlayerDataModel.Instance.MaxHealth;
        upperAnim.SetBool("Death", false);
        lowerAnim.gameObject.SetActive(true);

        respawn = true;
    }

}