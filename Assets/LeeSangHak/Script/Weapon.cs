using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string name;
    public float damage, attackSpeed;

    private void OnEnable()
    {
        if (name == "F")
        {
            //PlayerDataModel.Instance.wAttack = 200;
        }

        if (name == "S")
        {
            //PlayerDataModel.Instance.wAttack = 300;
        }

        if (name == "L")
        {
            //PlayerDataModel.Instance.wAttack = 500;
        }

        if (name == "H")
        {
            //PlayerDataModel.Instance.wAttack = 150;
        }
    }
}
