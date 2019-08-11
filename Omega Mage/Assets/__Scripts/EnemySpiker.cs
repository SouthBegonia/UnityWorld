using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpiker : PT_MonoBehaviour
{
    public float speed = 5f;
    public string roomXMLString = "{";

    public bool _________________;

    public Vector3 moveDir;
    public Transform characterTrans;

    private void Awake()
    {
        characterTrans = transform.Find("CharacterTrans");
    }

    private void Start()
    {
        switch (roomXMLString)
        {
            case "^":
                moveDir = Vector3.up;
                break;
            case "v":
                moveDir = Vector3.down;
                break;
            case "{":
                moveDir = Vector3.left;
                break;
            case "}":
                moveDir = Vector3.right;
                break;
        }
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = moveDir * speed;
    }

    public void Damage(float amt,ElementType eT,bool damageOverTime = false)
    {
        //
    }

    private void OnTriggerEnter(Collider other)
    {
        //判断是否撞墙
        GameObject go = Utils.FindTaggedParent(other.gameObject);
        if (go == null)
            return;

        if (go.tag == "Ground")
        {
            float dot = Vector3.Dot(moveDir, go.transform.position - pos);

            //如果攻击
            if (dot > 0)
            {
                moveDir *= -1;
            }
        }
    }
}
