using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void Start()
    {

    }

    public void Start_Game()
    {
        SceneManager.LoadScene("Stage1");
    }
}

