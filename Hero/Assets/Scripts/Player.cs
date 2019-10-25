using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private int count = 0;
    private AnimatorStateInfo stateInfo;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(stateInfo.ToString()+ stateInfo.normalizedTime);
        HandleInput();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(x, 0, 0) * Time.deltaTime;
    }

    private void HandleInput()
    {
        //按下键盘空格键攻击
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            HandleAttack();
        }

        //if(stateInfo.normalizedTime>1f)
        //{
        //    count = 0;
        //    animator.SetInteger("attack", count);
        //}
        //若动画为三种状态之一并且已经播放完毕
        if ((stateInfo.IsName("attack_1") || stateInfo.IsName("attack_2") || stateInfo.IsName("attack_3")) && stateInfo.normalizedTime > 1f)
        {
            //将count重置为0，即Idle状态
            count = 0;
            animator.SetInteger("attack", count);
        }


    }

    private void HandleAttack()
    {
        //攻击阶段一： idle ---> attack_1
        if ((stateInfo.IsName("Idle") ||stateInfo.IsName("Idle_2")) && count == 0)
        {
            count = 1;
            animator.SetInteger("attack", count);
    
        }
        //攻击阶段二： attack_1 ---> attack_2

        if (stateInfo.IsName("attack_1") && count == 1)
        {
            count = 2;
            animator.SetInteger("attack", count);
 
        }

        //攻击阶段三： attack_2 ---> attack_3
        if (stateInfo.IsName("attack_2") && count == 2)
        {
            count = 3;
            animator.SetInteger("attack", count);

        }

        ////攻击阶段完毕： attack_3 ---> idle
        //if (stateInfo.IsName("attack_3") && count == 3 && stateInfo.normalizedTime>1f)
        //{
        //    count = 0;
        //    animator.SetInteger("attack", count);
        //}
    }

    //private void HandleAttack()
    //{
    //    //攻击阶段一： idle ---> attack_1
    //    if (stateInfo.IsName("Idle") && count == 0)
    //    {
    //        count = 1;
    //        animator.SetInteger("attack", count);

    //    }
    //    //攻击阶段二： attack_1 ---> attack_2

    //    if (stateInfo.IsName("attack_1") && count == 1 && stateInfo.normalizedTime > 0.3f)
    //    {
    //        count = 2;
    //        animator.SetInteger("attack", count);

    //    }

    //    //攻击阶段三： attack_2 ---> attack_3
    //    if (stateInfo.IsName("attack_2") && count == 2 && stateInfo.normalizedTime > 0.1f)
    //    {
    //        count = 3;
    //        animator.SetInteger("attack", count);

    //    }

    //    //攻击阶段完毕： attack_3 ---> idle
    //    if (stateInfo.IsName("attack_3") && count == 3 && stateInfo.normalizedTime > 0.9f)
    //    {
    //        count = 0;
    //        animator.SetInteger("attack", count);
    //    }
    //}

    void GoToNextAttackAction()
    {
        animator.SetInteger("attack", count);
    }

}
