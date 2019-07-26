using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Tank脚本：实现玩家与操作坦克的交互（前后左右移动，空格开火）
*/
public class Tank : Unit
{
    public float MoveSpeed = 8.0f;      //移动速度
    public float RotateSpeed = 90f;     //旋转速度

    private TankWeapon tw;              //武器脚本
    //private float timer;
    //public float shootCoolDown;         //玩家的射击冷却时间
    //private bool canShoot;              //玩家是否可以射击

    private void Start()
    {
        //初始化生命值
        base.Start();

        //取得武器脚本
        tw = GetComponent<TankWeapon>();

        //设置玩家的敌人
        tw.Init(team);

        //初始化涉及射击冷却时间参数
        //timer = 0f;                 
        //shootCoolDown = 1f;
        //canShoot = true;

    }

    private void Update()
    {
        ////冷却计时方法1(弃用，改在TanjWeapon内)：timer
        //timer += Time.deltaTime;

        ////检测射击冷却时间是否完毕
        //if (timer > shootCoolDown)
        //{
        //    canShoot = true;
        //    timer = 0f;
        //}

        ////空格键射击
        ////问题：当在FixedUpdate()内进行射击判定时，不可实现连续快速键入空格
        //if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        //{
        //    tw.Shoot();
        //    canShoot = false;
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tw.Shoot();
        }
    }

    private void FixedUpdate()
    {
        //实现键位读取方法：
        //1.GetKey() ：当按键被按下时返回true
        //该方法使得坦克按照每秒 MoveSpeed 匀速移动
        //if(Input.GetKey(KeyCode.W))
        //    transform.Translate(new Vector3(0, 0, 1) * MoveSpeed * Time.deltaTime);
        //if(Input.GetKey(KeyCode.S))
        //    transform.Translate(Vector3.forward * -MoveSpeed * Time.deltaTime);
        //if (Input.GetKey(KeyCode.A))
        //    transform.Rotate(new Vector3(0, 1, 0) * -RotateSpeed * Time.deltaTime);
        //if (Input.GetKey(KeyCode.D))
        //    transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);

        //2.GetAxis() : 每帧根据按下松开过程返回-1~1的浮点数
        float horizontal = Input.GetAxis("Horizontal1");
        float vertical = Input.GetAxis("Vertical1");
        transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime * vertical);
        transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime * horizontal);
    }
}