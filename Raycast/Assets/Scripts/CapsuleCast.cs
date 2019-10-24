using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCast : MonoBehaviour
{
    private RaycastHit RaycastHit;
    private Vector3 pos1;
    private Vector3 pos2;

    private void Start()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        pos1 = transform.position - new Vector3(0, 0.5f, 0);
        pos2 = transform.position + new Vector3(0, 0.5f, 0);
    }

    void Update()
    {
        UpdatePos();
        //Physics.SphereCast(this.transform.position, radius, Vector3.forward, out RaycastHit, 1f, LayerMask.GetMask("Anchor"));
        Physics.CapsuleCast(pos1, pos2, 0.5f, Vector3.forward, out RaycastHit, 0.1f, LayerMask.GetMask("Anchor"));
        if (RaycastHit.collider != null)
        {
            Debug.Log("CapsuleCast Hit collider = " + RaycastHit.collider.gameObject.name);
           RaycastHit.collider.gameObject.GetComponent<MeshRenderer>().material = Player.instance.material;
        }
            

    }
}
