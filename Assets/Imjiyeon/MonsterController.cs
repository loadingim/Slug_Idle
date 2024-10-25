using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    /*
        기능의 순서
        1. 몬스터 생성 (해당 기능은 다른 스크립트에서 진행 예정)
        2. 생성된 몬스터가 앞으로 움직임 (걸어다니는 몬스터가 더 많으므로 걸어다니는 몬스터를 디폴트로 설정)
        3. 앞으로 움직일 때, 공격 범위 내로 플레이어의 콜라이더가 들어올 때 공격을 개시
        4. 체력이 0이 될 경우, 개체는 삭제되며 동시에 재화를 드랍. (추후 플레이어 재화와 연동하여 등록된 값을 ++ 하는 식으로 구현을 예상하고 있음)

        화면 밖에 있는 몬스터는 트리거가 on이 되어 맞지 않도록 함
     */

    public enum MonsterState { Move, Attack, Dead, Size }
}
