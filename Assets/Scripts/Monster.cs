using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int HP = 30;
    void Attacked()
    {
        HP -= 10;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

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
