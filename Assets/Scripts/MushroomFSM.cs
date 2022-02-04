using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFSM : MonoBehaviour
{
    // 버섯 몬스터 상태 상수
  enum MonsterState
    {
        Idle, 
        Trace, 
        Attack,
        Damaged, 
        Die
    }

    MonsterState m_State; // 버섯 몬스터 상태 변수

    Rigidbody2D rigid;   // Rigidbody2D 변수 : 오브젝트가 물리엔진 제어를 받도록

    public float findDistance =9f;  // 플레이어 발견 범위
    public float attackDistance = 1.2f; // 공격 가능 범위

    public Transform player;  // 플레이어 트랜스폼

    float currentTime = 0;  // 누적 시간
    float attackDelay = 3f;  // 공격 딜레이 시간

    public int attackPower = 7;  // 몬스터 공격력

    public int hp = 40;   // 버섯 몬스터 체력

    public int nextMove;   // 몬스터 행동지표 결정 변수 (이동방향 저장)   -1 :왼쪽, 0:정지, 1:오른쪽

    Animator anim;  // 애니메이터 변수
    SpriteRenderer spriteRenderer; // sprite renderer 변수 : 몬스터 그래픽 설정 제어

    
    //int exp = 20;   // 경험치

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", 3f);
        m_State = MonsterState.Idle;  // 최초의 버섯 몬스터 상태 : 대기 
        player = GameObject.Find("Player").transform;  // 플레이어의 트랜스폼 컴포넌트 받기
    }

   void Idle()
    {
        // nextMove값에 따라 랜덤으로 이동
        //rigid.velocity=new Vector3(nextMove*1.2f,rigid.velocity.y ,2);
        Vector3 dir = new Vector3(nextMove, rigid.velocity.y, 0);
        transform.position += dir * 1 * Time.deltaTime;

       anim.SetInteger("walkSpeed", nextMove);  // 애니메이션 맵핑

        // 플랫폼 낭떠러지 확인
        Vector3 frontVec = new Vector3(rigid.position.x + nextMove*0.2f, rigid.position.y, 0);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider ==null)
        {
            Turn();
        }

        // 플레이어와의 거리가 발견범위 이내면 Trace상태로 전환
        if (Vector2.Distance(transform.position, player.position) < findDistance)
        {
            m_State = MonsterState.Trace;
            print("상태 전환: Idle -> Trace");
        }
    }

   void Think()
    {
        // 다음 이동 설정
        nextMove = Random.Range(-1,2);  // -1:왼쪽, 0: 정지,  1:오른쪽

        // 그래픽 방향 재설정
        if (nextMove != 0)
            spriteRenderer.flipX = (nextMove == 1);

        // 애니메이션 맵핑
        anim.SetInteger("walkSpeed", nextMove);

        // 다음 이동 설정 (재귀)
        Invoke("Think", 5f);  // 재귀
    }

    void Turn()  // 몬스터 방향 전환 함수
    {
        nextMove = nextMove * (-1);  // 방향 전환
        spriteRenderer.flipX = (nextMove == 1);  // 그래픽 방향 재설정
        CancelInvoke();  // nextMove 직접 변경 해줬으니 Think() 멈췄다가 재호출
        Invoke("Think", 5f);
    }

    void Trace()  
    {
      // 플레이어 발견 범위 벗어나면
        if (Vector2.Distance(transform.position, player.position) > findDistance)
        { 
            // 대기상태 Idle 로 전환
            m_State = MonsterState.Idle;
            print("상태 전환 : Trace -> Idle");
        }

        // 플레이어와의 거리가 공격범위 밖이라면 플레이어 향해 이동
        else if(Vector2.Distance(transform.position, player.position)>attackDistance)
        {
            // 애니메이션 맵핑
            anim.SetInteger("walkSpeed", 1);

            if (player.transform.position.x < transform.position.x) // 몬스터의 왼쪽에 플레이어 위치
            {
                // 왼쪽 방향으로 이동
                Vector3 dir = new Vector3(-1, rigid.velocity.y, 0);
                transform.position += dir * 2 * Time.deltaTime;
                nextMove = -1;
                spriteRenderer.flipX = false;  // 그래픽 방향 재설정
            }

            else if (player.position.x > transform.position.x)  // 몬스터의 오른쪽에 플레이어가 위치
            {
                // 오른쪽 방향으로 이동
                Vector3 dir = new Vector3(1, rigid.velocity.y, 0);
                transform.position += dir * 2 * Time.deltaTime;
                nextMove = 1;
                spriteRenderer.flipX = true;  // 그래픽 방향 재설정
            }
        }

        // 플레이어와의 거리가 공격범위 이내라면 공격(Attack)으로 전환
        else
        {
            m_State = MonsterState.Attack;
            print("상태 전환 : Trace->Attack");

            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시킴 (공격 상태로 전환될 때 바로 공격)
            currentTime = attackDelay;

        }

    }

    void Attack()
    {
        // 플레이어가 공격범위 이내에 있다면 플레이어 공격
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // 일정 시간마다 플레이어 공격
            currentTime += Time.deltaTime;

            if(currentTime>attackDelay)
            {
                GameObject.Find("GameManager").SendMessage("LoseHP");
                print("공격");
                currentTime = 0;

                // 공격 애니메이션
                anim.SetTrigger("Attack");
            }
        }

        // 공격범위 밖이면 추적(Trace)상태로 전환
        else
        {
            m_State = MonsterState.Trace;
            print("상태 전환 : Attack -> Trace");
            currentTime = 0;
        }
    }

    void Damaged()
    {
        // 피격 상태 처리 위한 코루틴 실행
        StartCoroutine(DamageProcess());
    }

    // 피격 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간(0.5초)만큼 기다린다
        yield return new WaitForSeconds(0.5f);

        // 현재 상태를 추적 상태로 전환
        m_State = MonsterState.Trace;
        print("상태 전환 : Damaged -> Trace");
    }

    public void HitMonster (int hitPower)
    {
        // 이미 피격 상태이거나 사망 상태라면 함수 종료
        if (m_State == MonsterState.Damaged || m_State == MonsterState.Die )
            return;

        // 플레이어 공격력만큼 몬스터 체력 감소
        hp -= hitPower;

        // 체력이 0보다 크면 피격 상태로 전환
        if (hp>0)
        {
            m_State = MonsterState.Damaged;
            print("상태 전환 : AnyState -> Damaged");

            // 피격 애니메이션
            anim.SetTrigger("Damaged");
            Damaged();
        }
        // 체력이 0 이하면 죽음 상태로 전환
        else
        {
            m_State = MonsterState.Die;
            print("상태 전환 : Any State -> Die");
            Die();
        }
    }

    void Die()
    {
        // 진행중인 피격 코루틴 함수 중지
        StopAllCoroutines();

        //죽음 상태 처리 위한 코루틴 실행
        StartCoroutine(DieProcess());

        // 죽음 애니메이션 실행
        anim.SetTrigger("Die");

        // 경험치 증가
        GameObject.Find("GameManager").SendMessage("GainEXP");
    }

    IEnumerator DieProcess()
    {
        // 2초 동안 기다린 후 자기 자신 제거
        yield return new WaitForSeconds(2f);
        print("소멸");
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
