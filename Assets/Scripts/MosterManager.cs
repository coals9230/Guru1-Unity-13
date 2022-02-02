using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosterManager : MonoBehaviour
{
    float currentTime;  // ����ð�
    public float createTime = 15f; // ���� �ð�

    public GameObject monsterFactory;  // ���� ����

    void Start()
    {
        
    }

    
    void Update()
    {
        // �ð��� �帣��
        currentTime += Time.deltaTime;

        // ���� �ð��� �����ð��� �Ǹ�
        if (currentTime>createTime)
        {
            // ���� ���忡�� ���� �����ؼ� MonsterManager ��ġ�� ��ġ
            GameObject monster = Instantiate(monsterFactory);
            monster.transform.position = transform.position;

            // ���� �ð��� 0���� �ʱ�ȭ 
            currentTime = 0;
        }
    }
}
