using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitAgain : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
