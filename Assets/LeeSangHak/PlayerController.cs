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
    public int ammo;
    [SerializeField] float times;

    private void Start()
    {
        // �� ������ : ���� �ӵ�
        PlayerDataModel.Instance.AttackSpeed = 10f;
        Debug.Log(PlayerDataModel.Instance.AttackSpeed);
    }

    private void Update()
    {
        if (targetMonster == null || targetMonster.activeSelf != true)
        {
            targetMonster = null;
        }

        times = Time.time;

        // Ÿ������ ������ ���Ͱ� ����ְ� ���Ͱ� Ȱ��ȭ�� �ƴ� ��
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
            // Destroy(gameObject); < �׾��� �� 
            Debug.Log("�ǰݵ�");
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