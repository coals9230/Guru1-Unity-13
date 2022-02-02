using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosterManager : MonoBehaviour
{
    float currentTime;  // 현재시간
    public float createTime = 15f; // 일정 시간

    public GameObject monsterFactory;  // 몬스터 공장

    void Start()
    {
        
    }

    
    void Update()
    {
        // 시간이 흐르다
        currentTime += Time.deltaTime;

        // 현재 시간이 일정시간이 되면
        if (currentTime>createTime)
        {
            // 몬스터 공장에서 몬스터 생성해서 MonsterManager 위치에 배치
            GameObject monster = Instantiate(monsterFactory);
            monster.transform.position = transform.position;

            // 현재 시간을 0으로 초기화 
            currentTime = 0;
        }
    }
}
