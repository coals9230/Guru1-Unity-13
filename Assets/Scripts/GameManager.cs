using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int HP;
    public int EXP = 0;
    public int Level = 1;
    public int Potion;
    public void LoseHP()
    {
        HP -= 10;

        if (HP <= 0)
        {
            SceneManager.LoadScene("GameOver");
            //RestartGame();

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

        if (EXP >= 50)
        {
            Level++;
        }
        if (EXP >= 150)
        {
            Level++;
            SceneManager.LoadScene("Stage2");
        }
        if (EXP >= 250)
        {
            Level++;
        }
        if (EXP >= 450)
        {
            Level++;
            SceneManager.LoadScene("Stage3");
        }
        if (EXP >= 600)
            SceneManager.LoadScene("GameOver");
    }
    public void GotPotion()
    {
        Potion++;
    }

    
    public Text PotionText;
    void Start()
    {
        HP = 200;
        Potion = 0;
        
    }
   
    // Update is called once per frame
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
        //if(player)
    }
}
