using UnityEngine;
using UnityEngine.UI;

public class SkillActive_One : Skill
{
    [SerializeField] Image LookCoolTime;
    [SerializeField] float CoolTime;

    // 1. 쿨타임
    // 2. 공격값


    public void SkillOne()
    {
        Activate();
    }

    public override void Activate()
    {
        isActived = false;

        // 스킬 코드 작성 예정
        Debug.Log("첫번째 스킬 사용됨");

        StartCoroutine(SetCurrentCooltime(CoolTime, LookCoolTime, gameObject.GetComponent<Button>()));
    }
}
