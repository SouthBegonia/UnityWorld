using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpiker : PT_MonoBehaviour,Enemy
{
    [SerializeField]
    private float _touchDamage = 0.5f;     //EnemySpiker每次造成伤害0.5
    public float touchDamage
    {
        get { return (_touchDamage); }
        set { _touchDamage = value; }
    }

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
        //基于Room.xml中的角色设置移动方向
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
        //EnemySpiker往复运动过程
        GetComponent<Rigidbody>().velocity = moveDir * speed;
    }

    public void Damage(float amt,ElementType eT,bool damageOverTime = false)
    {
        //EnemySpiker不收任何攻击
    }

    private void OnTriggerEnter(Collider other)
    {
        //判断是否撞墙
        GameObject go = Utils.FindTaggedParent(other.gameObject);
        if (go == null)
            return;

        //当撞击到枪毙后则反向再次运动
        if (go.tag == "Ground")
        {
            float dot = Vector3.Dot(moveDir, go.transform.position - pos);

            if (dot > 0)
                moveDir *= -1;
        }
    }
}
