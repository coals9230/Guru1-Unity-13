using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour
{
    /*public static GameManager gm;
       //public GameObject gameLabel;
       //Image gameImage;

       public void Awake()
       {
           if(gm == null)
           {
               gm = this;
           }
       }*/
    public GameObject gameOption;

    public void OpenOptionWindow()
    {
        gameOption.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseOptionWindow()
    {
        gameOption.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
        gameOption.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
