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
    CircleCollider2D circelCollider;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circelCollider = GetComponent<CircleCollider2D>();

        Invoke("Think", Constants.InvokeTime);
    }

    void FixedUpdate()
    {
        //nextMove에 따라서 움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform check
        //현재 포지션 x에 nextMove*0.3f 마진을 줘서 앞쪽의 Platform을 체크
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec,
                                                Vector3.down, 1,
                                                LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Debug.Log("경고! 앞 낭떠러지");
            Turn();
        }
    }

    //행동 로직
    void Think()
    {
        //-1,0,1 셋 중 랜덤
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("RunningSpeed", nextMove);
        if(nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        float nextThinkTime = Random.Range(2f, 5f);
        //nextThinkTime마다 재귀호출
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", Constants.InvokeTime);
    }

    public void OnDamaged()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        nextMove = 0;
        //Collider Disable
        circelCollider.enabled = false;
        //Die effect jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //Destroy
        Invoke("DeActive", 5);
    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
