using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    private bool IsOverlapAnyCollider;

    void Update()
    {
        //自身的collider也会被检测到哈
        IsOverlapAnyCollider = Physics.CheckBox(transform.position, transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Anchor"));
        Debug.Log("isOverlapAnyCollider? : " + IsOverlapAnyCollider);
    }
}
