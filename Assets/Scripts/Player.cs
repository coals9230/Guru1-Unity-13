using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MovementSpeed = 8;
    public float JumpForce = 15;

    Rigidbody2D _rigidbody;
    Animator anim;
    SpriteRenderer sprite;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 이동
        var movement = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

        // [X]키 공격 
        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetBool("isWalking", false);
            anim.SetTrigger("attack");

            float distance = Vector2.Distance(GameObject.Find("Monster").transform.position,
                transform.position);

            if (distance <= 2.5f)
            {
                GameObject.Find("Monster").SendMessage("Damaged");
            }
                
        }

        // 캐릭터 방향 설정
        if (Input.GetButton("Horizontal"))
        {
            sprite.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }

        // 애니메이션 설정
        if (Input.GetButtonUp("Horizontal"))
            anim.SetBool("isWalking", false);
        else if (Input.GetButtonDown("Horizontal"))
            anim.SetBool("isWalking", true);
        
        // 점프
        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            anim.SetBool("isWalking",false);
        }

    }

    // 피격 애니메이션 실행
    void Damaged()
    {
        anim.SetTrigger("Damaged");
    }
}
