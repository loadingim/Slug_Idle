using UnityEngine;

public class MonsterShotBullet : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] PlayerDataModel playerDataModel;
             private Rigidbody2D rb;

    [SerializeField] float monsterAttackSpeed;
    [SerializeField] int   monsterAttack;
    [SerializeField] float returnTime;
             private float remainTime;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerDataModel = Player.GetComponent<PlayerDataModel>();
        rb = GetComponent<Rigidbody2D>();
    }

    // 오브젝트 활성화 시, 쿨타임을 서로 연동
    void OnEnable() { remainTime = returnTime; }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어의 콜라이더와 몬스터의 총알이 맞닿았을 때, 해당 총알은 삭제된다.
        if (Player == collision.gameObject)
        {
            playerDataModel.Health -= monsterAttack;
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 총알은 생성 시 자동으로 플레이어를 향해 발사된다.
        transform.Translate(Player.transform.position * monsterAttackSpeed * Time.deltaTime);

        // 총알이 맞지 않고 일정 시간이 지났을 때, 자동으로 삭제된다.
        remainTime -= Time.deltaTime;
        if (remainTime < 0) { Destroy(gameObject); }
    }
}
