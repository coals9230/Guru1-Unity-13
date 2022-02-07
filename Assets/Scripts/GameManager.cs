using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int HP;
    public int EXP = 0;
    public int Level = 1;

    public int Potion;

    public Text PotionText;  // 체력, 포션 개수, 경험치 표시 텍스트


    public void LoseHP(int damage)
    {
        HP -= damage;

        // 플레이어 피격 애니메이션 실행 함수
        GameObject.Find("Player").SendMessage("Damaged"); 

        if (HP <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void GainHP()
    {
        HP += 20;
        Potion--;
    }

    public void GainEXP()
    {
        EXP += 20;

        if (EXP == 60)
        {
            GameObject.Find("MonsterManager").SendMessage("level1");
        }

        if (EXP == 60)
        {
            Level++;
            GameObject.Find("MonsterManager").SendMessage("level2");
        }

        if (EXP == 120)
        {
            Level++;
            SceneManager.LoadScene("Stage2");
            GameObject.Find("MonsterManager").SendMessage("level1");
        }

        if (EXP == 180)
        {
            Level++;
            GameObject.Find("MonsterManager").SendMessage("level2");
        }

        if (EXP == 240)
        {
            Level++;
            SceneManager.LoadScene("Stage3");
            GameObject.Find("MonsterManager").SendMessage("level1");
        }

        if(EXP ==300)
        {
            GameObject.Find("MonsterManager").SendMessage("level2");
        }

        if (EXP == 360)
            SceneManager.LoadScene("Success");
    }

    public void GotPotion()
    {
        Potion++;
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        HP = 200;
        Potion = 0;
    }
   

    void Update()
    {
        PotionText.text = "Potion: " + Potion + "\n" + "HP: " + HP + "\n" + "EXP: " + EXP;

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Potion == 0)
            {
                Debug.Log("Not enough Potion");
            }
            else
            {
                GainHP();
                Debug.Log("HP:" + HP);
            }
        }
    }

}
