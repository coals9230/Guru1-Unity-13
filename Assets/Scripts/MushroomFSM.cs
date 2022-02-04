using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFSM : MonoBehaviour
{
    // ���� ���� ���� ���
  enum MonsterState
    {
        Idle, 
        Trace, 
        Attack,
        Damaged, 
        Die
    }

    MonsterState m_State; // ���� ���� ���� ����

    Rigidbody2D rigid;   // Rigidbody2D ���� : ������Ʈ�� �������� ��� �޵���

    public float findDistance =9f;  // �÷��̾� �߰� ����
    public float attackDistance = 1.2f; // ���� ���� ����

    public Transform player;  // �÷��̾� Ʈ������

    float currentTime = 0;  // ���� �ð�
    float attackDelay = 3f;  // ���� ������ �ð�

    public int attackPower = 7;  // ���� ���ݷ�

    public int hp = 40;   // ���� ���� ü��

    public int nextMove;   // ���� �ൿ��ǥ ���� ���� (�̵����� ����)   -1 :����, 0:����, 1:������

    Animator anim;  // �ִϸ����� ����
    SpriteRenderer spriteRenderer; // sprite renderer ���� : ���� �׷��� ���� ����

    
    //int exp = 20;   // ����ġ

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", 3f);
        m_State = MonsterState.Idle;  // ������ ���� ���� ���� : ��� 
        player = GameObject.Find("Player").transform;  // �÷��̾��� Ʈ������ ������Ʈ �ޱ�
    }

   void Idle()
    {
        // nextMove���� ���� �������� �̵�
        //rigid.velocity=new Vector3(nextMove*1.2f,rigid.velocity.y ,2);
        Vector3 dir = new Vector3(nextMove, rigid.velocity.y, 0);
        transform.position += dir * 1 * Time.deltaTime;

       anim.SetInteger("walkSpeed", nextMove);  // �ִϸ��̼� ����

        // �÷��� �������� Ȯ��
        Vector3 frontVec = new Vector3(rigid.position.x + nextMove*0.2f, rigid.position.y, 0);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider ==null)
        {
            Turn();
        }

        // �÷��̾���� �Ÿ��� �߰߹��� �̳��� Trace���·� ��ȯ
        if (Vector2.Distance(transform.position, player.position) < findDistance)
        {
            m_State = MonsterState.Trace;
            print("���� ��ȯ: Idle -> Trace");
        }
    }

   void Think()
    {
        // ���� �̵� ����
        nextMove = Random.Range(-1,2);  // -1:����, 0: ����,  1:������

        // �׷��� ���� �缳��
        if (nextMove != 0)
            spriteRenderer.flipX = (nextMove == 1);

        // �ִϸ��̼� ����
        anim.SetInteger("walkSpeed", nextMove);

        // ���� �̵� ���� (���)
        Invoke("Think", 5f);  // ���
    }

    void Turn()  // ���� ���� ��ȯ �Լ�
    {
        nextMove = nextMove * (-1);  // ���� ��ȯ
        spriteRenderer.flipX = (nextMove == 1);  // �׷��� ���� �缳��
        CancelInvoke();  // nextMove ���� ���� �������� Think() ����ٰ� ��ȣ��
        Invoke("Think", 5f);
    }

    void Trace()  
    {
      // �÷��̾� �߰� ���� �����
        if (Vector2.Distance(transform.position, player.position) > findDistance)
        { 
            // ������ Idle �� ��ȯ
            m_State = MonsterState.Idle;
            print("���� ��ȯ : Trace -> Idle");
        }

        // �÷��̾���� �Ÿ��� ���ݹ��� ���̶�� �÷��̾� ���� �̵�
        else if(Vector2.Distance(transform.position, player.position)>attackDistance)
        {
            // �ִϸ��̼� ����
            anim.SetInteger("walkSpeed", 1);

            if (player.transform.position.x < transform.position.x) // ������ ���ʿ� �÷��̾� ��ġ
            {
                // ���� �������� �̵�
                Vector3 dir = new Vector3(-1, rigid.velocity.y, 0);
                transform.position += dir * 2 * Time.deltaTime;
                nextMove = -1;
                spriteRenderer.flipX = false;  // �׷��� ���� �缳��
            }

            else if (player.position.x > transform.position.x)  // ������ �����ʿ� �÷��̾ ��ġ
            {
                // ������ �������� �̵�
                Vector3 dir = new Vector3(1, rigid.velocity.y, 0);
                transform.position += dir * 2 * Time.deltaTime;
                nextMove = 1;
                spriteRenderer.flipX = true;  // �׷��� ���� �缳��
            }
        }

        // �÷��̾���� �Ÿ��� ���ݹ��� �̳���� ����(Attack)���� ��ȯ
        else
        {
            m_State = MonsterState.Attack;
            print("���� ��ȯ : Trace->Attack");

            // ���� �ð��� ���� ������ �ð���ŭ �̸� �����Ŵ (���� ���·� ��ȯ�� �� �ٷ� ����)
            currentTime = attackDelay;

        }

    }

    void Attack()
    {
        // �÷��̾ ���ݹ��� �̳��� �ִٸ� �÷��̾� ����
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // ���� �ð����� �÷��̾� ����
            currentTime += Time.deltaTime;

            if(currentTime>attackDelay)
            {
                GameObject.Find("GameManager").SendMessage("LoseHP");
                print("����");
                currentTime = 0;

                // ���� �ִϸ��̼�
                anim.SetTrigger("Attack");
            }
        }

        // ���ݹ��� ���̸� ����(Trace)���·� ��ȯ
        else
        {
            m_State = MonsterState.Trace;
            print("���� ��ȯ : Attack -> Trace");
            currentTime = 0;
        }
    }

    void Damaged()
    {
        // �ǰ� ���� ó�� ���� �ڷ�ƾ ����
        StartCoroutine(DamageProcess());
    }

    // �ǰ� ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð�(0.5��)��ŭ ��ٸ���
        yield return new WaitForSeconds(0.5f);

        // ���� ���¸� ���� ���·� ��ȯ
        m_State = MonsterState.Trace;
        print("���� ��ȯ : Damaged -> Trace");
    }

    public void HitMonster (int hitPower)
    {
        // �̹� �ǰ� �����̰ų� ��� ���¶�� �Լ� ����
        if (m_State == MonsterState.Damaged || m_State == MonsterState.Die )
            return;

        // �÷��̾� ���ݷ¸�ŭ ���� ü�� ����
        hp -= hitPower;

        // ü���� 0���� ũ�� �ǰ� ���·� ��ȯ
        if (hp>0)
        {
            m_State = MonsterState.Damaged;
            print("���� ��ȯ : AnyState -> Damaged");

            // �ǰ� �ִϸ��̼�
            anim.SetTrigger("Damaged");
            Damaged();
        }
        // ü���� 0 ���ϸ� ���� ���·� ��ȯ
        else
        {
            m_State = MonsterState.Die;
            print("���� ��ȯ : Any State -> Die");
            Die();
        }
    }

    void Die()
    {
        // �������� �ǰ� �ڷ�ƾ �Լ� ����
        StopAllCoroutines();

        //���� ���� ó�� ���� �ڷ�ƾ ����
        StartCoroutine(DieProcess());

        // ���� �ִϸ��̼� ����
        anim.SetTrigger("Die");

        // ����ġ ����
        GameObject.Find("GameManager").SendMessage("GainEXP");
    }

    IEnumerator DieProcess()
    {
        // 2�� ���� ��ٸ� �� �ڱ� �ڽ� ����
        yield return new WaitForSeconds(2f);
        print("�Ҹ�");
        Destroy(gameObject);
    }

    void Update()
    {
      switch(m_State)
        {
            case MonsterState.Idle:
                Idle();
                break;

            case MonsterState.Trace:
                Trace();
                break;

            case MonsterState.Attack:
               Attack();
                break;

            case MonsterState.Damaged:
                //Damaged();
                break;

            case MonsterState.Die:
                //Die();
                break;
        }
    }
}
