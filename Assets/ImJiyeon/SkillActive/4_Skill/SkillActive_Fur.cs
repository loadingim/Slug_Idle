using UnityEngine;
using UnityEngine.UI;

public class SkillActive_Fur : Skill
{
    public void SkillFur()
    {
        Activate();
    }

    public override void Activate()
    {
        isActived = false;

        Debug.Log("네번째 스킬 사용됨");

        //for (int i = 0; i < gameManager.StageInstance.Monsters.Length; i++)
        //{
        //    gameManager.StageInstance.Monsters[i].MonsterHP -= SkillAttack;
        //}

        StartCoroutine(SetCurrentCooltime(CoolTime, LookCoolTime, gameObject.GetComponent<Button>()));
    }
}
