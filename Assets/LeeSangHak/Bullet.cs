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
        // Ÿ���� ����ְų� Ÿ���� ��Ȱ��ȭ�϶�
        if (target == null || target.activeSelf == false)
        {
            transform.Translate(trans.position * speed * Time.deltaTime);
        }


        // Ÿ���� �ְ� Ȱ��ȭ�϶�
        if (target != null && target.activeSelf == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, trans.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
            {
                // ���Ϳ��� ������ ������
                //target.GetComponent<Monster>().TakeDamage(damage);

                // źȯ ����
                //Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            collision.GetComponent<MonsterModel>().MonsterHP--;
            Debug.Log("���Ͱ� �޴� ������");
            playerController.ammo--;
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
