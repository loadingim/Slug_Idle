using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] float speed;
    [SerializeField] GameObject target;
    [SerializeField] Transform trans;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        trans = target.transform;
        // 타겟은 비어있거나 타겟이 비활성화일때
        if (target == null || target.activeSelf == false)
        {
            transform.Translate(trans.position * speed * Time.deltaTime);
        }


        // 타겟이 있고 활성화일때
        if (target != null && target.activeSelf == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, trans.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
            {
                // 몬스터에게 데미지 입히기
                //target.GetComponent<Monster>().TakeDamage(damage);

                // 탄환 제거
                //Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            collision.GetComponent<MonsterModel>().MonsterHP--;
            Debug.Log("몬스터가 받는 데미지");
            playerController.ammo--;
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
