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

    private void Awake()
    {

    }

    private void Update()
    {
        trans = target.transform;
        // 타겟은 비어있거나 타겟이 비활성화일때
        if (target == null)
        {
            transform.Translate(trans.position * speed * Time.deltaTime);
        }


        // 타겟이 있고 활성화일때
        if (target != null && target.activeSelf == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, trans.position, speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("접촉");
        if (collision.gameObject.tag == "Monster" && collision.gameObject == target)
        {
            Debug.Log("타겟접촉");
            collision.GetComponent<MonsterModel>().MonsterHP -= PlayerDataModel.Instance.Attack;
            Debug.Log("몬스터가 받는 데미지");
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
