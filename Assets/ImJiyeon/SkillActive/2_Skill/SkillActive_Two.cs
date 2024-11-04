using UnityEngine;
using UnityEngine.UI;

public class SkillActive_Two : Skill
{
    [SerializeField] Image LookCoolTime;
    [SerializeField] float CoolTime;

    public void SkillTwo()
    {
        Activate();
    }

    public override void Activate()
    {
        isActived = false;

        // 스킬 코드 작성 예정
        Debug.Log("두번째 스킬 사용됨");

        CoroutineManager.Instance.ManagerCoroutineStart(SetCurrentCooltime(CoolTime, LookCoolTime, gameObject.GetComponent<Button>()), SetCoolTimeCoroutineName);
    }
}
