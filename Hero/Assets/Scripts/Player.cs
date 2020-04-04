using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerState
{
    public bool canInput;
    public bool canTurn;
    public bool canAttackAgain;
    public bool canJump;
    public bool canSlide;
    public bool canBeHurt;

    public bool isAlive;
    public bool isAttacking;
    public bool isUnderAttacking;
    public bool isOnGround;
}

//public enum State
//{
//    idle,
//    //onGround,   
//    onAir,
//    atkOnGround,
//    atkOnAir,
//    sliding,
//}

public class Player : Fighter
{

    public static Player Instance;


    //Player动作状态机
    private int count = 0;
    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private SpriteRenderer PlayerSprite;


    //Player父物体
    private Rigidbody2D rigidbody2d;
    private Vector2 direction;          //Player面朝的方向

    private int moveX;
    private float SpdMul;
    private float BlinkTime;

    private PlayerState state;


    //PlayerBody
    private BoxCollider2D boxCollider;
    private Cinemachine.CinemachineCollisionImpulseSource MyInpulse;

    public Transform pos;


    private void Awake()
    {
        Instance = this;
    }


    private  void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = gameObject.GetComponentInParent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        PlayerSprite = GetComponent<SpriteRenderer>();


        state.canInput = true;
        state.canJump = true;
        state.canSlide = true;
        state.canTurn = true;
        state.canAttackAgain = true;
        state.isOnGround = true;
        state.isAttacking = false;
        state.canBeHurt = true;
        state.isAlive = true;
        state.isUnderAttacking = false;

        moveX = 0;
        SpdMul = 1f;
        direction = new Vector2(rigidbody2d.transform.localScale.x, 0);

        MyInpulse = GetComponent<Cinemachine.CinemachineCollisionImpulseSource>();

        //初始化生命值系统
        InitHealth();
    }

    private void Update()
    {



        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //根据所在位置(空中/地面)自动改变移动速度倍率
        if (state.isOnGround)
        {
            if (state.isAttacking)
                SpdMul = 0.1f;
            else
                SpdMul = 1.0f;
        }
        else if (!state.isOnGround)
        {
            if (state.isAttacking)
                SpdMul = 0.1f;
            else
                SpdMul = 1.0f;
        }


        //生命值小于0后死亡
        if (state.isAlive && Health <= 0)
            Death();

        //受伤害期间不可进行任何输入
        if (state.canBeHurt && state.isAlive)
        {
            AttackOnAir();
            AttackOnGround();
            InputMove();
        }

        //受伤害闪烁
        if (state.isUnderAttacking)
            SpriteBlink();
        else
            PlayerSprite.enabled = true;

        //Player方向矫正
        if (transform.parent.transform.eulerAngles != Vector3.zero)
        {
            transform.parent.transform.eulerAngles = Vector3.zero;
        }

       
        //Test----------------------------
        Damage damage = new Damage { HitPoint = 10f, Pos = pos.position,pushForce=1f };
        if (Input.GetMouseButtonDown(1) && state.isAlive)
            GetHurt(damage);

    }
    
    private void FixedUpdate()
    {


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Background"))
        {
            state.isOnGround = true;
            animator.SetBool("onGround", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Background"))
        {
            state.isOnGround = false;
            animator.SetBool("onGround", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Background"))
        {
            //从跳跃状态落地，变为Player层
            gameObject.layer = 10;
        }
    }


    #region 输入及攻击系统

    //移动输入
    private void InputMove()
    {
        int InputX = (int)Input.GetAxisRaw("Horizontal");

        if (state.canTurn)
        {
            moveX = InputX;
        }
        else
            moveX = 0;

        //输入在任何时候（地面/空中）都可以移动玩家：
        if (state.canInput)
        {
            transform.parent.position += new Vector3(InputX * SpdMul, 0, 0) * Time.deltaTime;
            animator.SetInteger("Horizontal", InputX);
        }

        //但只有在地面时才进行玩家转向
        if (state.canTurn && state.canInput)
        {
            if (moveX > 0)
            {
                direction.x = 1;
                transform.parent.localScale = Vector3.one;
            }
            else if (moveX < 0)
            {
                direction.x = -1;
                transform.parent.localScale = new Vector3(-1, 1, 1);
            }
        }

        //在地面上方可输入跳跃或滑动
        if (state.isOnGround && state.canInput)
        {
            InputJumpOrSlide();
        }
    }

    //滑步及跳跃输入：
    private void InputJumpOrSlide()
    {
        //滑步
        if (state.canSlide && Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state.canTurn = false;
                state.canSlide = false;
                state.canJump = false;
                state.canInput = false;
                animator.SetTrigger("slide");

                //滑步中无敌
                state.canBeHurt = false;

                //滑步中处于Ignore层忽略与Enemy的碰撞
                //transform.parent.gameObject.layer = 11;
                gameObject.layer = 11;

                AddMoveDelta(direction, 2f, 0f);
                StartCoroutine(StopSliding());
            }
        }
        else if (state.canJump && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S))
        {
            //障眼法：jump动画时间和滞空时间大致相同，避免复杂状态
            animator.SetBool("onGround", false);
            //rigidbody2d.AddForce(Vector2.up);
            rigidbody2d.AddForce(new Vector2(direction.x * 20f, 10f));
            rigidbody2d.velocity += Vector2.up * 2f;
            state.canTurn = true;

            //跳跃中处于Ignore层
            gameObject.layer = 11;
        }
    }

    //空中攻击
    private void AttackOnAir()
    {
        //空中攻击判断：1、2结束时
        if ((stateInfo.IsName("attack_air_1") || stateInfo.IsName("attack_air_2")) && stateInfo.normalizedTime > 1f)
        {
            count = 0;
            animator.SetInteger("attack", count);
            state.isAttacking = false;
            state.canJump = true;
            state.canAttackAgain = false;
        }
        else if ((stateInfo.IsName("jump") || stateInfo.IsName("jump_fall") || stateInfo.IsName("attack_air_1") || stateInfo.IsName("attack_air_2")) && state.isOnGround)
        {
            //空中攻击判断：空中几个普通状态时着陆
            count = 0;
            animator.SetInteger("attack", count);
            state.isAttacking = false;
            state.canJump = true;

            state.canAttackAgain = true;
        }
        else if (stateInfo.IsName("attack_air_3") && state.isOnGround)
        {
            //attack_air_3持续进行直至接触地面，自动切换到attack_air_4
            //对attack_air_4动画延时结束

            //最终一击期间无敌
            state.canBeHurt = false;

            //相机抖动
            MyInpulse.GenerateImpulse();
            StartCoroutine(Attck_3CD());
        }


        //空中攻击
        if (!state.isOnGround && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.V)) && state.canAttackAgain)
        {
            state.isAttacking = true;
            state.canJump = false;

            //攻击阶段一：
            if (count == 0)
            {
                count = 4;
                animator.SetInteger("attack", count);
                SetMoveDelta(Vector2.up, 1.0f, 5.0f);
            }
            else if (stateInfo.IsName("attack_air_1") && count == 4 && stateInfo.normalizedTime > 0.4f)
            {
                //攻击阶段二：
                count = 5;
                animator.SetInteger("attack", count);
                SetMoveDelta(Vector2.up, 1.6f, 10f);
            }
            else if (stateInfo.IsName("attack_air_2") && count == 5 && stateInfo.normalizedTime > 0.4f)
            {
                //攻击阶段三：
                count = 6;
                animator.SetInteger("attack", count);
                SetMoveDelta(Vector2.down, 3f, 0f);

                //三阶段攻击时禁止输入
                state.canInput = false;
            }
            else if (count == 6)
            {
                //state.canAttackAgain = true;
            }
        }
    }


    //地面攻击
    private void AttackOnGround()
    {
        //地面攻击判断：任意动画结束
        if ((stateInfo.IsName("attack_1") || stateInfo.IsName("attack_2") || stateInfo.IsName("attack_3")) && stateInfo.normalizedTime > 1f)
        {
            count = 0;
            animator.SetInteger("attack", count);

            state.isAttacking = false;
            state.canJump = true;
            state.canInput = true;
            state.canTurn = true;
            ResetMoveDelta();
        }


        //Input地面攻击：
        if (state.isOnGround && Input.GetMouseButtonDown(0))
        {
            rigidbody2d.velocity = Vector2.zero;
            state.isAttacking = true;
            state.canJump = false;
            state.canInput = false;
            state.canTurn = false;

            //攻击阶段一： idle ---> attack_1
            if (count == 0)
            {
                count = 1;
                animator.SetInteger("attack", count);
                state.canAttackAgain = true;
            }
            else if (stateInfo.IsName("attack_1") && count == 1)
            {
                //攻击阶段二： attack_1 ---> attack_2
                count = 2;
                animator.SetInteger("attack", count);
                AddMoveDelta(direction, 0.5f, 0f);
            }
            else if (stateInfo.IsName("attack_2") && count == 2 && stateInfo.normalizedTime > 0.2f)
            {
                //攻击阶段三： attack_2 ---> attack_3
                count = 3;
                animator.SetInteger("attack", count);
                //AddMoveDelta(direction, 0.5f, 0f);
            }
            else if (count == 3)
            {
                //AddMoveDelta(direction, 1.7f, 0f);
                if (state.canAttackAgain)
                {
                    //此处不可再上面攻击阶段三写：添加位移是即时的，但应等attack_3播放一定时长再产生位移
                    AddMoveDelta(direction, 1f, 0f);
                    state.canAttackAgain = false;
                }

            }
        }
    }


    //设置位移函数：仅用在空中
    private void SetMoveDelta(Vector2 dir, float velMul, float force)
    {
        rigidbody2d.velocity = dir * velMul;
        rigidbody2d.AddForce(dir * force);
    }

    //添加位移函数：仅用在地面
    public void AddMoveDelta(Vector2 dir, float velMul, float ForceMul)
    {
        //切勿设置velocity：因为物体的vel不一定是在 direction方向（rigidbody内查看），强行设置会造成误差累计，最终可能造成滑移
        //同理，AddFore也要考虑物体本身方向
        rigidbody2d.velocity += dir * velMul;
        //rigidbody2d.AddForce(dir * ForceMul);
    }

    //位移归零：
    //由于对rigidbody直接添加速度，力等，多次累计会使其归零速度极慢，因此在动作完成后都应该对其手动归零
    private void ResetMoveDelta()
    {
        rigidbody2d.velocity = Vector2.zero;
    }


    IEnumerator StopSliding()
    {
        //0.4s为slide的时长
        yield return new WaitForSeconds(0.5f);
        state.canTurn = true;
        state.canJump = true;
        state.canInput = true;
        state.canBeHurt = true;
        animator.ResetTrigger("slide");

        //滑步后更正为Player层
        //transform.parent.gameObject.layer = 10;
        gameObject.layer = 10;


        ResetMoveDelta();
        StartCoroutine(SlideCD());
    }

    IEnumerator SlideCD()
    {
        yield return new WaitForSeconds(0.3f);
        //state.canTurn = true;
        state.canSlide = true;
    }

    IEnumerator Attck_3CD()
    {
        yield return new WaitForSeconds(0.5f);

        //延时后归位attack
        count = 0;
        animator.SetInteger("attack", count);
        state.canInput = true;
        state.canTurn = true;
        state.isAttacking = false;
        state.canJump = true;
        state.canAttackAgain = true;
        state.canBeHurt = true;
    }

    #endregion

    #region 伤害系统

    //传入伤害参数，对Player造成伤害及后退
    protected override void GetHurt(Damage damage)
    {
        //在地面受到伤害
        if(state.isOnGround && state.isAlive && state.canBeHurt)
        {
            if (Time.time - lastHurtTime > InvincibleTime)
            {                
                //重置无敌时间、生命值减少
                lastHurtTime = Time.time;
                Health -= damage.HitPoint;

                //受伤后清零其他状态及其parameters
                GettingHurtState();

                //开启受伤状态及CD协程
                state.canBeHurt = false;
                state.isUnderAttacking = true;
                StartCoroutine(HurtCD());

                //开启受伤动画
                animator.SetTrigger("hurt");

                //计算出敌人与Player坐标差，根据x方向判断受伤害后退方向
                Vector2 delta;
                float deltaX = (damage.Pos - transform.position).x;          
                if (deltaX > 0)
                    delta = new Vector3(-1f, transform.parent.position.y);
                else
                    delta = new Vector3(1f, transform.parent.position.y);
                SetMoveDelta(Vector2.zero, 0, 0);
                AddMoveDelta(delta, damage.pushForce, 0f);
            }
        }

        //在空中受到伤害
        if(!state.isOnGround && state.isAlive && state.canBeHurt)
        {
            if (Time.time - lastHurtTime > InvincibleTime)
            {
                //重置无敌时间、生命值减少
                lastHurtTime = Time.time;
                Health -= damage.HitPoint;

                //受伤后清零其他状态及其parameters
                GettingHurtState();

                //开启受伤状态及CD协程
                state.canBeHurt = false;
                state.isUnderAttacking = true;
                StartCoroutine(HurtCD());

                //开启受伤动画
                animator.SetTrigger("hurt");

                //计算出敌人与Player坐标差，根据x方向设置受伤害后退方向
                //空中受到伤害y<0
                Vector2 delta;
                float deltaX = (damage.Pos - transform.position).x;
                if (deltaX > 0)
                    delta = new Vector3(-1f, -1);
                else
                    delta = new Vector3(1f, -1);
                SetMoveDelta(Vector2.zero, 0, 0);
                AddMoveDelta(delta, damage.pushForce, 0f);
            }
        }
    }

    //受到伤害时清零其他状态：
    private void GettingHurtState()
    {
        //清零攻击参数
        count = 0;
        animator.SetInteger("attack", count);
        state.isAttacking = false;

        //清零Input相关参数
        state.canJump = true;
        state.canInput = true;
        state.canTurn = true;
        animator.SetInteger("Horizontal", 0);
    }

    //死亡函数
    protected override void Death()
    {        
        Debug.Log("Die");
        animator.SetTrigger("die");
        state.isAlive = false;
        state.canBeHurt = false;
    }

    //受伤闪烁
    public void SpriteBlink()
    {
        BlinkTime += Time.deltaTime * 2f;
        if (BlinkTime % 0.05f > 0.03f)
        {
            PlayerSprite.enabled = false;
        }
        else
            PlayerSprite.enabled = true;
    }

    //受伤硬直时间：
    IEnumerator HurtCD()
    {
        //0.3s后可Input输入
        yield return new WaitForSeconds(0.5f);
        state.canBeHurt = true;
        state.isUnderAttacking = false;
    }

    #endregion
}
