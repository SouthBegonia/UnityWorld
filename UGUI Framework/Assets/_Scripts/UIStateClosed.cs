using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateClosed : StateMachineBehaviour
{
    /// <summary>
    /// 当Close动画播放完后调用
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}
