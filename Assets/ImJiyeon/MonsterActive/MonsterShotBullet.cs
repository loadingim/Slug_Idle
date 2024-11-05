using UnityEngine;

public class MonsterShotBullet : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] PlayerDataModel playerDataModel;

    [SerializeField] float returnTime;
             private float remainTime;


    private int damage;
    public int Damage { get { return damage; } set { damage = value; } }

    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }



    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerDataModel = Player.GetComponent<PlayerDataModel>();
        rigid = GetComponent<Rigidbody2D>();
    }


    void OnEnable() { remainTime = returnTime; }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player == collision.gameObject)
        {
            //Debug.Log("몬스터 총알 플레이어에게 맞음");
            playerDataModel.Health -= damage;
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 총알 발사
        Vector3 dir = (Player.transform.position - transform.position).normalized;
        rigid.velocity = new Vector2(dir.x * 2f * speed, dir.y * 2f * speed);

        // 총알이 맞지 않고 일정 시간이 지났을 때, 자동으로 삭제된다.
        remainTime -= Time.deltaTime;
        if (remainTime < 0) { Destroy(gameObject); }
    }
}
