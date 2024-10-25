using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float speed;
    [SerializeField] GameObject target;

    private void Update()
    {
        if (target == null || target.activeSelf == false)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 distance = (target.transform.position - transform.position).normalized;
        transform.position += (Vector3)distance * speed * Time.deltaTime;

        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            // 몬스터에게 데미지 입히기
            //target.GetComponent<Monster>().TakeDamage(damage);

            // 탄환 제거
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }


}
