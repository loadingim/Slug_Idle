using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] float speed;
    [SerializeField] GameObject target;
    [SerializeField] Transform trans;

    private void Update()
    {
        trans = target.transform;
        // 타겟은 비어있거나 타겟이 비활성화일때
        if (target == null || target.activeSelf == false)
        {
            transform.Translate(trans.position * speed * Time.deltaTime);
            return;
        }

        
        // 타겟이 있고 활성화일때
        if (target != null && target.activeSelf == true)
        {
            transform.Translate(target.transform.position * speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
            {
                // 몬스터에게 데미지 입히기
                //target.GetComponent<Monster>().TakeDamage(damage);

                // 탄환 제거
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
