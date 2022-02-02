using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int HP = 200;

    public int PotionHP = 0;
    public void LoseHP() 
    { 
        HP -= 10;
        PotionText.text = "Potion: " + PotionHP + "\n" + "HP: " + HP;
    }

    public void GainHP() 
    {
        HP += 20;
        PotionHP--;
        PotionText.text = "Potion: " + PotionHP + "\n" + "HP: " + HP;
    }
    public void GotPotion() 
    {
        PotionHP++;
        PotionText.text = "Potion: " + PotionHP + "\n" + "HP: " + HP;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public Text PotionText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (PotionHP == 0)
            {
                Debug.Log("Not enough Potion");
            }
            else
            {
                GainHP();
                Debug.Log("HP:" + HP);
            }
        }

        if (HP == 0)
        {
            RestartGame();
        }
    }
}
