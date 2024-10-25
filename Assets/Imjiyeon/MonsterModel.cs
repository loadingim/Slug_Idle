using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    // 몬스터의 체력  (몬스터는 체력을 회복하지 않으므로, 최대 체력 값은 필요하지 않을 것으로 판단함)
    [SerializeField] float monsterHP;
    public float MonsterHP { get { return monsterHP; } set { monsterHP = value; } }


    // 몬스터의 공격력
    [SerializeField] float monsterAttack;
    public float MonsterAttack { get { return monsterAttack; } set { monsterAttack = value; } }


    // 몬스터의 공격속도
    [SerializeField] float monsterAttackSpeed;
    public float MonsterAttackSpeed { get { return monsterAttackSpeed; } set { monsterAttackSpeed = value; } }


    // 몬스터가 떨어트리는 재화 갯수
    [SerializeField] int dropGold;
    public int DropGold { get { return dropGold; } set { dropGold = value; } }


    // 몬스터 이동 속도
    [SerializeField] float monsterMoveSpeed;
    public float MonsterMoveSpeed { get { return monsterMoveSpeed; }  set { monsterMoveSpeed = value; } }



    // 몬스터는 총알 공격을 기본 베이스로 하므로, 추후 플레이어가 일정 거리 안에 들어올 경우 슈팅을 가하는 기능을 만들것
}
