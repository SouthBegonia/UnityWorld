using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapBox : MonoBehaviour
{
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    void Update()
    {
        //halfExtents为块体边长的一半，此处localScale/2及代表检测的块体和物体模型大小一致
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale/2, Quaternion.identity, LayerMask.GetMask("Anchor"));

        if (hits.Length > 0)
        {
            foreach (Collider collider in hits)
            {
                if (collider == boxCollider)
                    continue;
                Debug.Log("OverlapBox Hit collider = " + collider.gameObject.name);
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        //指定地点绘制大小size的方块
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
