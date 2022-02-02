using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 direction = collision.gameObject.transform.position - transform.position;
        direction = direction.normalized * 300;
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction);
        GameObject.Find("GameManager").SendMessage("LoseHP");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
