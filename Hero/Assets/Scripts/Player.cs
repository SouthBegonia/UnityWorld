using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private int count = 0;
    private AnimatorStateInfo stateInfo;

    private bool attacking;         //是否在攻击状态 
    public Vector3 attackMoveDelta; //攻击位移

    private bool canAttackAgain;    //防止第三段连击被多次按下，进而导致多段位移

    private void Start()
    {
        animator = GetComponent<Animator>();
        attackMoveDelta = Vector3.zero;
        //moveAttackDeltaTime
    }

    private void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        HandleInput();

        if (count == 0)
            attacking = false;
    }

    private void FixedUpdate()
    {
        if (!attacking)
        {
            Run();
        }
        else if(attacking)
        {
            transform.parent.position += attackMoveDelta;
            attackMoveDelta.x = Mathf.Lerp(attackMoveDelta.x, 0f, 0.1f);

            if (Mathf.Abs(attackMoveDelta.x) < 0.001)
            attackMoveDelta.x = 0f;
        }

    }

    private void Run()
    {
        int x = (int)Input.GetAxisRaw("Horizontal");
        transform.parent.position += new Vector3(x, 0, 0) * Time.deltaTime;
        animator.SetInteger("Horizontal", x);
        
        if (x != 0)
        {
            if (x > 0)
            {
                transform.parent.localScale = Vector3.one;
                if (Mathf.Abs(GetAttackMoveDelta()) < 0.04)
                {
                    AddAttackMoveDelta(0.06f);
                }
            }
            else if (x < 0)
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

    private void HandleInput()
    {
        if ((stateInfo.IsName("attack_1") || stateInfo.IsName("attack_2") || stateInfo.IsName("attack_3")) && stateInfo.normalizedTime > 1f)
        {
            //将count重置为0，即Idle状态
            count = 0;
            animator.SetInteger("attack", count);
            attacking = false;
        }

        //按下键盘空格键攻击
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            attacking = true;
            HandleAttack();
            //SetAttackAttackMoveDelta(0f);
            //AddAttackMoveDelta(0.3f);

        }
    }

    private void HandleAttack()
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
            AddAttackMoveDelta(0.5f);
        }
        else if (stateInfo.IsName("attack_2") && count == 2 && stateInfo.normalizedTime>0.2f)
        {
            //攻击阶段三： attack_2 ---> attack_3
            count = 3;
            animator.SetInteger("attack", count);
            AddAttackMoveDelta(2.5f);
        }
        else if(count==3)
        {
            if (canAttackAgain)
            {
                AddAttackMoveDelta(1.5f);
                canAttackAgain = false;
            }
                
        }else
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

}
