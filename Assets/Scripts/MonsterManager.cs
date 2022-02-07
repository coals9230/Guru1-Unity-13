using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject aMonster;
    public GameObject bMonster;


    void Start()
    {
        level1();
    }

    void Update()
    {}

    public void level1()
    {
        aMonster.SetActive(true);
        bMonster.SetActive(false);
    }

    public void level2()
    {
        aMonster.SetActive(false);
        bMonster.SetActive(true);
    }

}
