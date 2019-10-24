using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapBoxNonAlloc : MonoBehaviour
{
    private int ColliderAmount;
    private Collider[] colliders;

    private void Start()
    {
        ColliderAmount = 0;
        colliders = new Collider[10];
    }

    void Update()
    {
        ColliderAmount = Physics.OverlapBoxNonAlloc(transform.position, transform.localScale / 2, colliders, Quaternion.identity, LayerMask.GetMask("Anchor"));
        Debug.Log("colliderAmount = " + ColliderAmount);
        if (colliders.Length > 0)
        {
            foreach(Collider coll in colliders)
            {
                if (coll != null)
                    Debug.Log("OverlapBoxNonAlloc hit collider = " + coll.name);
            }
        }
    }
}
