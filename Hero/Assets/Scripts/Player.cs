using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private int count = 0;
    private AnimatorStateInfo stateInfo;

    private bool isAttacking;         //是否在攻击状态 
    public Vector3 attackMoveDelta; //攻击位移


    private bool canAttackAgain;    //防止第三段连击被多次按下，进而导致多段位移
    private bool canInputMove;
    private bool canJump;
    private bool canStand;

    private Rigidbody2D rigidbody2d;
    //private bool isJumping;         //是否在跳跃中
    private bool isOnGround;        //是否在地面上
    //private bool isStanding;

    private int moveX;

    private BoxCollider2D boxCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        attackMoveDelta = Vector3.zero;
        rigidbody2d = gameObject.GetComponentInParent<Rigidbody2D>();

        boxCollider = GetComponent<BoxCollider2D>();

        //isStanding = false;

        canJump = true;
        canStand = true;
        canInputMove = true;
        isOnGround = true;
        moveX = 0;
    }

    private void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        InputAttack();
        
        if (count == 0)
            isAttacking = false;

        if (!isAttacking)
        {
            InputRun();
        }

        if(transform.parent.transform.eulerAngles != Vector3.zero)
        {
            transform.parent.transform.eulerAngles = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {

        if (isAttacking)
        {
            transform.parent.position += attackMoveDelta;
            attackMoveDelta.x = Mathf.Lerp(attackMoveDelta.x, 0f, 0.1f);

            if (Mathf.Abs(attackMoveDelta.x) < 0.001)
                attackMoveDelta.x = 0f;
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Background"))
        {
            isOnGround = true;
            animator.SetBool("onGround", true);
            //isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Background"))
        {
            isOnGround = false;
            animator.SetBool("onGround", false);
            //isJumping = true;
        }
    }

    //移动输入
    private void InputRun()
    {
        //if(SampleDirection ==)
        int InputX = (int)Input.GetAxisRaw("Horizontal");


        if (canInputMove)
        {
            moveX = InputX;
        }
        else
            moveX = 0;
        

        //滑步中不可读取移动
        if (canInputMove && !isAttacking)
        {
            transform.parent.position += new Vector3(moveX, 0, 0) * Time.deltaTime;

            animator.SetInteger("Horizontal", moveX);

            if (moveX != 0)
            {
                if (moveX > 0)
                {
                    transform.parent.localScale = Vector3.one;
                    if (Mathf.Abs(GetAttackMoveDelta()) < 0.04)
                    {
                        AddAttackMoveDelta(0.06f);
                    }
                }
                else if (moveX < 0)
                {
                    transform.parent.localScale = new Vector3(-1, 1, 1);
                    if (Mathf.Abs(GetAttackMoveDelta()) < 0.04)
                    {
                        AddAttackMoveDelta(0.06f);
                    }
                }
            }
            else
            {
                SetAttackAttackMoveDelta(0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //滑步
            if (Input.GetKey(KeyCode.S) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                if (isOnGround && canStand)
                {
                    animator.SetTrigger("stand");
                    rigidbody2d.AddForce(new Vector2(moveX * 30f, 0f));
                    rigidbody2d.velocity += new Vector2(moveX * 1.7f, 0f);
                    canInputMove = false;
                    //isStanding = true;
                    canStand = false;
                    StartCoroutine(StopStanding());
                }

            }
            else
            {
                //跳跃
                if (canJump && isOnGround)
                {
                    //障眼法：jump动画时间和滞空时间大致相同，避免复杂状态
                    //animator.SetTrigger("jump");
                    animator.SetBool("onGround", false);
                    //rigidbody2d.AddForce(Vector2.up);
                    rigidbody2d.AddForce(new Vector2(moveX * 20f, 10f));
                    rigidbody2d.velocity += Vector2.up * 2f;
                }
            }

        }
    }

    //读取攻击
    private void InputAttack()
    {
        //地面攻击判断
        if ((stateInfo.IsName("attack_1") || stateInfo.IsName("attack_2") || stateInfo.IsName("attack_3")) && stateInfo.normalizedTime > 1f)
        {
            //将count重置为0，即Idle状态
            count = 0;
            animator.SetInteger("attack", count);

            isAttacking = false;
            canJump = true;
        }

        //Debug.Log(stateInfo.);
        //空中攻击判断
        if( (stateInfo.IsName("attack_air_1") || stateInfo.IsName("attack_air_2") || stateInfo.IsName("attack_air_3")) && stateInfo.normalizedTime > 1f)
        {
            Debug.Log("sss");
            count = 0;
            animator.SetInteger("attack", count);
            isAttacking = false;
            canJump = true;
            //canAttackAgain = true;

        }
        else if((stateInfo.IsName("jump")||stateInfo.IsName("jump_fall")||stateInfo.IsName("attack_air_1")||stateInfo.IsName("attack_air_2")||stateInfo.IsName("attack_air_3")) && isOnGround)
        {
            count = 0;
            animator.SetInteger("attack", count);
            isAttacking = false;
            canJump = true;
            canAttackAgain = true;
        }

        //if (stateInfo.IsName("attack_air_1") && stateInfo.normalizedTime>1f)
        //    Debug.Log("lllllllllll");

        //地面攻击
        if (isOnGround && Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
            canJump = false;
            AttackOnGround();
            //SetAttackAttackMoveDelta(0f);
            //AddAttackMoveDelta(0.3f);
        }

        //空中攻击
        if(!isOnGround && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.V)))
        {
            isAttacking = true;
            canJump = false;
            AttackOnAir();
            //Debug.Log("kkk");
        }
    }

    //空中攻击
    private void AttackOnAir()
    {
        //攻击阶段一：
        if (count == 0 && canAttackAgain)
        {
            count = 4;
            animator.SetInteger("attack", count);
            canAttackAgain = false;

            rigidbody2d.velocity = Vector3.up;
            rigidbody2d.AddForce(Vector3.up * 5f);

        }
        else if (stateInfo.IsName("attack_air_1") && count == 4 && stateInfo.normalizedTime>0.4f)
        {
            //攻击阶段二：
            count = 5;
            animator.SetInteger("attack", count);
            rigidbody2d.velocity = Vector3.up *1.5f;
            rigidbody2d.AddForce(Vector3.up * 10f);
        }
        else if (stateInfo.IsName("attack_air_2") && count == 5 && stateInfo.normalizedTime > 0.4f)
        {
            //攻击阶段三： attack_2 ---> attack_3
            count = 6;
            animator.SetInteger("attack", count);
            rigidbody2d.velocity = Vector3.down *2f;
            rigidbody2d.AddForce(Vector3.down * 10f);
        }
        else if (count == 6)
        {
                canAttackAgain = true;
        }
    }


    //地面攻击
    private void AttackOnGround()
    {
        //攻击阶段一： idle ---> attack_1
        if (count == 0)
        {
            count = 1;
            animator.SetInteger("attack", count);
            canAttackAgain = true;
        }
        else if (stateInfo.IsName("attack_1") && count == 1)
        {
            //攻击阶段二： attack_1 ---> attack_2
            count = 2;
            animator.SetInteger("attack", count);
            AddAttackMoveDelta(0.2f);
        }
        else if (stateInfo.IsName("attack_2") && count == 2 && stateInfo.normalizedTime > 0.2f)
        {
            //攻击阶段三： attack_2 ---> attack_3
            count = 3;
            animator.SetInteger("attack", count);
            AddAttackMoveDelta(1.5f);
        }
        else if (count == 3)
        {
            if (canAttackAgain)
            {
                AddAttackMoveDelta(1.2f);
                canAttackAgain = false;
            }

        }
        else
        {
            SetAttackAttackMoveDelta(0f);
            //canAttackAgain = false;
        }
    }

    void GoToNextAttackAction()
    {
        animator.SetInteger("attack", count);
    }

    private float GetAttackMoveDelta()
    {
        return attackMoveDelta.x;
    }

    public void SetAttackAttackMoveDelta(float x)
    {
        attackMoveDelta.x = transform.parent.localScale.x * x / 100f;
    }

    public void AddAttackMoveDelta(float x)
    {
        attackMoveDelta.x += transform.parent.localScale.x * x / 100f;
    }


    IEnumerator StopStanding()
    {
        yield return new WaitForSeconds(0.6f);
        canInputMove = true;
        //isStanding = false;
        rigidbody2d.velocity = Vector2.zero;
        StartCoroutine(StandCD());
    }

    IEnumerator StandCD()
    {
        yield return new WaitForSeconds(0.3f);
        canStand = true;
    }
}
