using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----Spiker敌人:只会在固定路径上往复运动,速度快,且无法被消灭-----*/
public class EnemySpiker : PT_MonoBehaviour,Enemy
{
    [SerializeField]
    private float _touchDamage = 1f;     //EnemySpiker每次造成伤害
    public float touchDamage
    {
        get { return (_touchDamage); }
        set { _touchDamage = value; }
    }

    public string typeString
    {
        get { return (roomXMLString); }
        set { roomXMLString = value; }
    }

    public float speed = 5f;
    public bool NormalMove = true;      //Spiker是否被正常愚弄电脑
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
        //NormalMove标记其是否正常运动,否则即为被水魔法减速
        Move(NormalMove);
    }

    //控制Spiker的运动状况
    void Move(bool status)
    {
        //如果spiker为被水魔法击中,则保持正常运动,否则减速运动
        if (status)
            GetComponent<Rigidbody>().velocity = moveDir * speed;
        else
            GetComponent<Rigidbody>().velocity = moveDir * speed / 4;
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
