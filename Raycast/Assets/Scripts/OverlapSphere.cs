using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//相交球检测
public class OverlapSphere : MonoBehaviour
{
    private SphereCollider  SphereCollider;

    private void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, SphereCollider.radius, LayerMask.GetMask("Anchor"));

        if(hits.Length>0)
        {
            foreach(Collider collider in hits)
            {
                if (collider == SphereCollider)
                    continue;
                Debug.Log("OverlapSphere Hit collider = " + collider.gameObject.name);
            }
        }
    }
}
