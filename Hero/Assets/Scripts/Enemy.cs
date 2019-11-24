using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyState
{
    public bool isAlive;
    public bool chasing;
    public bool isAttacking;

}

public class Enemy : Fighter
{
    //Enemy_Body上的组件
    private Animator enemyAnm;
    private AnimatorStateInfo stateInfo;
    private BoxCollider2D enemyBoxCol;
    private SpriteRenderer enemySprite;

    //Enemy上的组件
    private Rigidbody2D enemyRgb;


    //搜索玩家参数
    //private Vector3 chaseLength;
    public float chaseLength;
    public float attackLength;
    private EnemyState enemyState;

    //public Transform player;

    public float atkCD;
    private float lastAtkTime;

    private void Start()
    {
        enemyAnm = GetComponent<Animator>();
        enemyBoxCol = GetComponent<BoxCollider2D>();
        enemyRgb = GetComponentInParent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        enemyState.isAlive = true;
        enemyState.chasing = false;
        enemyState.isAttacking = false;

        lastAtkTime = -atkCD;

    }

    protected void Update()
    {
        //取得当前动画信息
        stateInfo = enemyAnm.GetCurrentAnimatorStateInfo(0);

        //移动判定：远/中/近
        float delta = Mathf.Abs(Player.Instance.transform.position.x - transform.position.x);
        if (delta > chaseLength)
        {
            //远距离，不追逐
            enemyState.isAttacking = false;
            enemyState.chasing = false;
            enemyAnm.SetBool("chasing", false);
            enemyAnm.SetBool("attacking", false);
            enemyRgb.velocity = Vector3.zero;
            enemyRgb.Sleep();

        }
        else if (delta <= chaseLength && delta > attackLength)
        {
            //保证当前
            if(stateInfo.normalizedTime > 0.9f)
            {
                //中距离，开启追逐
                enemyState.isAttacking = false;
                enemyState.chasing = true;
                enemyAnm.SetBool("chasing", true);
                enemyAnm.SetBool("attacking", false);
            }

        }
        else if (delta <= attackLength)
        {
            //近距离，取消追逐，变为攻击状态
            enemyState.chasing = false;
            enemyState.isAttacking = true;
            enemyAnm.SetBool("chasing", false);
            enemyAnm.SetBool("attacking", true);

            AttackMove();
        }

        //在追逐区域内进行的追逐状态
        if (enemyState.chasing && !enemyState.isAttacking)
        {
            if ((Player.Instance.transform.position.x - transform.position.x) > 0)
            {
                transform.parent.position += new Vector3(0.5f, 0f, 0f) * Time.deltaTime;
                transform.parent.localScale = Vector3.one;
            }
            else
            {
                transform.parent.position += new Vector3(-0.5f, 0f, 0f) * Time.deltaTime;
                transform.parent.localScale = new Vector3(-1, 1, 1);
            }
        }


        //Player方向矫正
        if (transform.parent.transform.eulerAngles != Vector3.zero)
        {
            transform.parent.transform.eulerAngles = Vector3.zero;
        }

    }

    protected virtual void AttackMove()
    {
        if(Time.time-lastAtkTime >=atkCD)
        {
            //Debug.Log("sss");
            //if ((Player.Instance.transform.position.x - transform.position.x) > 0)
            //{
            //    enemyRgb.AddForce(Vector2.left * 4);
            //    lastAtkTime = Time.time;
            //}
            //else
            //{
            //    //transform.parent.position += new Vector3(-0.5f, 0f, 0f) * Time.deltaTime;
            //    //transform.parent.localScale = new Vector3(-1, 1, 1);
            //    enemyRgb.AddForce(Vector2.left * 4);
            //    lastAtkTime = Time.time;
            //}

            enemyRgb.AddForce(new Vector2(transform.parent.localScale.x *2f, 0));
            
        }

    }

    //protected virtual void SearchTarget()
    //{
    //    float delta = Mathf.Abs(Player.Instance.transform.position.x - transform.position.x);
    //    if (delta <= chaseLength)
    //    {
    //        enemyState.chasing = true;
    //        enemyAnm.SetBool("chasing", true);
    //    }
    //    else
    //    {
    //        enemyState.chasing = false;
    //        enemyAnm.SetBool("chasing", false);
    //    }          
    //}

    protected virtual void ChaseTarget()
    {

    }




    protected override void InitHealth()
    {
        base.InitHealth();
    }

    protected override void GetHurt(Damage damage)
    {
        base.GetHurt(damage);
    }

    protected override void Death()
    {
        base.Death();
    }
}
