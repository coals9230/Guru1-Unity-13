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
        PotionText.text = "Potion: " + Potion + "\n" + "HP: " + HP;

        if (HP == 0)
        {
            RestartGame();
        }
    }

    public void GainHP()
    {
        HP += 20;
        Potion--;
        PotionText.text = "Potion: " + Potion + "\n" + "HP: " + HP;
    }

    public void GainEXP()
    {
        EXP += 20;

        if (EXP >= 50)
        {
            Level++;
        }
        else if (EXP >= 150)
        {
            Level++;
        }
        else if (EXP >= 250)
        {
            Level++;
        }
        else if (EXP >= 450)
        {
            Level++;
        }

    }
    public void GotPotion()
    {
        Potion++;
        PotionText.text = "Potion: " + Potion + "\n" + "HP: " + HP;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
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
