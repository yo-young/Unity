using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //public 변수로 설정하면 Inspector에서 실행 중에도 값 변경 가능
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    //단발적인 키 입력은 Update에서 하는 것이 좋다 ex) 점프
    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        //Stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            //normalized : 벡터 크기를 1로 만든 상태(단위벡터) GetAxisRaw랑 비슷하다?
            //normalized에 곱해주는 값이 크면 버튼을 떼도 미끄러진다.
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.001f, rigid.velocity.y);
        }

        //Direction Sprite
        //flipX는 bool값으로 default는 false
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //Run animation 적용
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
    }

    //지속적인 키 입력은 FixedUpdate에서 하는 것
    void FixedUpdate()
    {
        //FixedUpdate는 보통 1초에 약 50회 돈다.
        //1초 동안 방향키를 누르고 있으면 AddForce가 50번 입력된다.
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        //Vector2의 첫번째 파라미터는 x축 속도, 두번째 파라미터는 y축 속도?
        //Max Speed
        if (rigid.velocity.x > maxSpeed) // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1)) // Left Max Speed
            // 왼쪽은 마이너스 속도기 때문에 맥스 스피드에 -1 곱
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y < 0) // y축 속도가 음수인 경우 = 점프하고 내려오는 순간
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0)); //레이를 그린다.
            //현재 포지션에서 LayerMask Platform을 검색한다.
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position,
                                                    Vector3.down, 1,
                                                    LayerMask.GetMask("Platform"));
            //rayHit에 충돌이 있는 경우
            if (rayHit.collider != null)
            {
                //충돌한 오브젝트와의 거리가 0.5 이하인 경우
                if (rayHit.distance < 0.5f)
                {
                    //Debug.Log(rayHit.collider.name);
                    //isJumping을 false로 setting
                    anim.SetBool("isJumping", false);
                }
            }
        }
    }
}
