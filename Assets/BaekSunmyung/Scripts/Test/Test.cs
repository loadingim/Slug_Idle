using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public static Test Instance { get; private set; }

    public bool a;
    public bool b;
    public bool c;
    public bool d;

    public int Heavy_Level = 0;
    public int Flame_Level = 0;
    public int Roket_Level = 0;
    public int Shotgun_Level = 0;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }


}
