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
        // �̵�
        var movement = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

        // ĳ���� ���� ����
        if (Input.GetButton("Horizontal"))
        {
            sprite.flipX = (Input.GetAxisRaw("Horizontal") == 1);

            // ���� ���� �ڽ� ����
            if (Input.GetAxisRaw("Horizontal") ==1)  // ������ ����Ű ������ ��
                pos.localPosition = new Vector2(1.25f, pos.localPosition.y);
            else
                pos.localPosition = new Vector2(-1.25f, pos.localPosition.y);
        }

        // [X] ����
        if (Input.GetKey(KeyCode.X))
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll (pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                // ������Ʈ �±�tag�� Monster�̸� ������Ʈ�� damaged�Լ� ȣ��
                if (collider.tag == "Monster")
                {
                    collider.SendMessage("Damaged");
                }
            }

            // ���� �ִϸ��̼�
            anim.SetBool("isWalking", false);
            anim.SetTrigger("attack");
        }

        // �ִϸ��̼� ����
        if (Input.GetButtonUp("Horizontal"))
            anim.SetBool("isWalking", false);
        else if (Input.GetButtonDown("Horizontal"))
            anim.SetBool("isWalking", true);

        // ����
        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            anim.SetBool("isWalking", false);
        }

    }

    // �ǰ� �ִϸ��̼� ����
    void Damaged()
    {
        anim.SetTrigger("Damaged");
    }

    // Overlap�Ǵ� �ڽ� ũ��� ��ġ ǥ��
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position,boxSize);
    }
}
