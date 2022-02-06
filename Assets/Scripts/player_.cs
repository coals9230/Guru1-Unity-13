using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_ : MonoBehaviour
{
    public float MovementSpeed = 8;
    public float JumpForce = 15;

    Rigidbody2D _rigidbody;
    Animator anim;
    SpriteRenderer sprite;

    public Transform pos;
    public Vector2 boxSize= new Vector2(1,1.5f);

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

        // 캐릭터 방향 설정
        if (Input.GetButton("Horizontal"))
        {
            sprite.flipX = (Input.GetAxisRaw("Horizontal") == 1);

            // 공격 범위 박스 방향
            if (Input.GetAxisRaw("Horizontal") ==1)  // 오른쪽 방향키 눌렀을 때
                pos.localPosition = new Vector2(1.25f, pos.localPosition.y);
            else
                pos.localPosition = new Vector2(-1.25f, pos.localPosition.y);
        }

        // [X] 공격
        if (Input.GetKey(KeyCode.X))
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll (pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                // 오브젝트 태그tag가 Monster이면 컴포넌트의 damaged함수 호출
                if (collider.tag == "Monster")
                {
                    collider.SendMessage("Damaged");
                }
            }

            // 공격 애니메이션
            anim.SetBool("isWalking", false);
            anim.SetTrigger("attack");
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
            anim.SetBool("isWalking", false);
        }

    }

    // 피격 애니메이션 실행
    void Damaged()
    {
        anim.SetTrigger("Damaged");
    }

    // Overlap되는 박스 크기와 위치 표시
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position,boxSize);
    }
}
