using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Colliderable
{
    public string[] messages;           //所有信息段
    private int msgNow = 0;             //当前显示的信息

    public float showTime;              //显示信息的持续时间
    public float coolDown = 4.0f;       //显示信息的间隔
    private float lastShout;

    public bool ______________;

    private float posDelta;             //玩家与NPC的距离差(仅用x轴的)

    protected override void Start()
    {
        base.Start();
        lastShout = -coolDown;
    }

    protected override void Update()
    {
        base.Update();

        //切换NPC方向始终朝向Player
        posDelta = GameManager.instance.player.transform.position.x - transform.position.x;
        if (posDelta > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (posDelta < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    //碰撞触发NPC对话函数:
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
            return;

        if (Time.time - lastShout > coolDown)
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(messages[msgNow++], 30, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, showTime);

            if (msgNow == messages.Length)
                msgNow = 0;
            Debug.Log("msgNow = " + msgNow);
        }
        
    }
}
