using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//球体投射
public class SphereCast : MonoBehaviour
{
    private float radius;
    private SphereCollider SphereCollider;
    private RaycastHit RaycastHit;

    private void Start()
    {
        radius = GetComponent<SphereCollider>().radius;
        SphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        //SphereCastAll方法：返回的时RaycastHit[]
        //RaycastHit[] hits =  Physics.SphereCastAll(this.transform.position, radius, Vector3.one, LayerMask.GetMask("Anchor"));

        //SphereCast方法：返回bool
        Physics.SphereCast(this.transform.position, radius, Vector3.forward, out RaycastHit, 1f, LayerMask.GetMask("Anchor"));
        if (RaycastHit.collider != null)
            Debug.Log("SphereCast Hit collider = " + RaycastHit.collider.gameObject.name);

        //RaycastAll遍历：
        //if (hits.Length > 0)
        //{
        //    foreach(RaycastHit hit in hits)
        //    {
        //        if (hit.collider == SphereCollider)
        //            continue;
        //        Debug.Log("SphereCast = " + hit.collider.gameObject.name);
        //    }
        //}

    }
}
