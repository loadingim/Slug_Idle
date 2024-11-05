using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PartnerContorller : MonoBehaviour
{
    public GameObject targetMonster = null;
    public float attackCooldown = 0f;
    [SerializeField] GameObject weaponPrefab;
    public int attackRange;
    [SerializeField] float times;
    [SerializeField] GameObject[] monsters;
    public GameObject muzzlePoint;
    [SerializeField] Animator upperAnim;
    [SerializeField] Animator lowerAnim;
    private float attackSpeed;


    private void Start()
    {
        // 모델 데이터 : 공격 속도 및 사거리
        attackSpeed = 1f / StoreCSV.Instance.Store[PlayerDataModel.Instance.AttackSpeedLevel + 4999].StatusStore_satatusNum / SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Eri_Level].AssistantDealer_attackSpdPer;
        attackRange = 20;
    }



    private void Update()
    {
        if (gameObject.activeSelf)
        {
            times = Time.time;

            if (targetMonster == null || !targetMonster.activeSelf)
            {
                FindTarget();
            }

            if (targetMonster != null && Time.time >= attackCooldown)
            {
                Attack();
            }
        }
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

    public void Attack()
    {
        if (targetMonster != null && Vector2.Distance(transform.position, targetMonster.transform.position) < attackRange)
        {
            lowerAnim.SetBool("isMove", false);
            upperAnim.SetBool("isAtk", true);
            upperAnim.SetFloat("speed", attackSpeed + (PlayerDataModel.Instance.AttackSpeedLevel * 0.01f));

            ShootBullet();

            attackCooldown = Time.time + 1f / StoreCSV.Instance.Store[PlayerDataModel.Instance.AttackSpeedLevel + 4999].StatusStore_satatusNum / SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Eri_Level].AssistantDealer_attackSpdPer;
            Debug.Log($"동료 공속{StoreCSV.Instance.Store[PlayerDataModel.Instance.AttackSpeedLevel + 4999].StatusStore_satatusNum / SubDealer.Instance.SubDealers[WeaponInfoData.Instance.Eri_Level].AssistantDealer_attackSpdPer}");
        }
    }

    private void ShootBullet()
    {

        weaponPrefab.GetComponent<PartnerWeapon>().shot();

    }
}