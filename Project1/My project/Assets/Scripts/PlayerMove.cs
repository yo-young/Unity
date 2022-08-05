using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //public ������ �����ϸ� Inspector���� ���� �߿��� �� ���� ����
    public float maxSpeed;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    //�ܹ����� Ű �Է��� Update���� �ϴ� ���� ����
    void Update()
    {
        //Stop speed
        if(Input.GetButtonUp("Horizontal"))
        {
            //normalized : ���� ũ�⸦ 1�� ���� ����(��������) GetAxisRaw�� ����ϴ�?
            //normalized�� �����ִ� ���� ũ�� ��ư�� ���� �̲�������.
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.001f, rigid.velocity.y);
        }

        //Direction Sprite
        //flipX�� bool������ default�� false
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //Run animation ����
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
    }

    //�������� Ű �Է��� FixedUpdate���� �ϴ� ��
    void FixedUpdate()
    {
        //FixedUpdate�� ���� 1�ʿ� �� 50ȸ ����.
        //1�� ���� ����Ű�� ������ ������ AddForce�� 50�� �Էµȴ�.
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        //Vector2�� ù��° �Ķ���ʹ� x�� �ӵ�, �ι�° �Ķ���ʹ� y�� �ӵ�?
        //Max Speed
        if (rigid.velocity.x > maxSpeed) // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1)) // Left Max Speed
            // ������ ���̳ʽ� �ӵ��� ������ �ƽ� ���ǵ忡 -1 ��
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
    }
}
