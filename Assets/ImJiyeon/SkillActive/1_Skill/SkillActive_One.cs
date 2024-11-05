using UnityEngine;
using UnityEngine.UI;

public class SkillActive_One : Skill
{
    public void SkillOne()
    {
        Activate();
    }

    public override void Activate()
    {
        isActived = false;

        Debug.Log("첫번째 스킬 사용됨");

        for (int i = 0; i < gameManager.StageInstance.Monsters.Length; i++)
        {
            if (gameManager.StageInstance.Monsters[i] != null)
            {
                gameManager.StageInstance.Monsters[i].MonsterHP -= SkillAttack;
            }
        }

        CoroutineManager.Instance.ManagerCoroutineStart(StartCoroutine(SetCurrentCooltime(CoolTime, LookCoolTime, gameObject.GetComponent<Button>())), this);
    }
}
