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
        HandleInput();
    }

    private void HandleInput()
    {
        //若动画为三种状态之一并且已经播放完毕
        if ((stateInfo.IsName("attack_1") || stateInfo.IsName("attack_2") || stateInfo.IsName("attack_3")) &&  stateInfo.normalizedTime > 1f)
        {
            count = 0;   //将count重置为0，即Idle状态
            animator.SetInteger("attack", count);
            //attack = false;
        }

        //按下键盘J键攻击
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("ssuu");
            HandleAttack();
        }
    }

    private void HandleAttack()
    {
        //若处于Idle状态，则直接打断并过渡到attack_a(攻击阶段一)
        if (stateInfo.IsName("Idle") && count == 0)
        {
            count = 1;
            animator.SetInteger("attack", count);
            Debug.Log(stateInfo.ToString() + count);
        }
        //如果当前动画处于attack_a(攻击阶段一)并且该动画播放进度小于80%，此时按下攻击键可过渡到攻击阶段二
        else if (stateInfo.IsName("attack_1") && count == 1)
        {
            count = 2;
            animator.SetInteger("attack", count);
            Debug.Log(stateInfo.ToString() + count);
        }
        //同上
        else if (stateInfo.IsName("attack_2") && count == 2)
        {
            count = 3;
            animator.SetInteger("attack", count);
            Debug.Log(stateInfo.ToString() + count);
        }
        else if(stateInfo.IsName("attack_3") && count == 3)
        {
            count = 0;   //将count重置为0，即Idle状态
            animator.SetInteger("attack", count);
        }
    }
}
