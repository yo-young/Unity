using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Constants
{
    public const int InvokeTime = 2;
}

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", Constants.InvokeTime);
    }

    void FixedUpdate()
    {
        //nextMove�� ���� ������
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform check
        //���� ������ x�� nextMove*0.3f ������ �༭ ������ Platform�� üũ
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec,
                                                Vector3.down, 1,
                                                LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Debug.Log("���! �� ��������");
            Turn();
        }
    }

    //�ൿ ����
    void Think()
    {
        //-1,0,1 �� �� ����
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("RunningSpeed", nextMove);
        if(nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        float nextThinkTime = Random.Range(2f, 5f);
        //nextThinkTime���� ���ȣ��
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", Constants.InvokeTime);
    }
}
