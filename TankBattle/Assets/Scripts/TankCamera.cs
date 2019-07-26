using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
第三人视角模式，挂载于TankCamera(第三人视角)
*/
public class TankCamera : MonoBehaviour
{
    public GameObject player;                 //玩家
    public float CameraRotateSpeed = 90f;     //视角旋转速度
    public Transform target;

    public GameObject CameraFirst;            //第一人视角
    public GameObject CameraThird;            //第三人视角
    private bool CameraActive;                //是否启动视角

    private void Start()
    {
        //初始化启动视角(默认为第三人视角)
        CameraActive = true;
    }

    private void Update()
    {
        //小键盘左右键位调整视角旋转
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.down * CameraRotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up * CameraRotateSpeed * Time.deltaTime);

        //PageDown 实现第一人和第三人视角的切换
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            CameraFirst.SetActive(CameraActive);
            CameraActive = !CameraActive;
            CameraThird.SetActive(CameraActive);
        }

        /* 关于第一人与第三人视角问题：
           问题来源：当游戏内切换至第一人视角CameraFirst时，若该视角下玩家死亡，玩家会被销毁，
                    问题在于玩家销毁时附带也销毁了它的子物体CameraFirst(因为第一视角跟踪Player)，
                    而销毁时，CameraFirst 为 active状态，而 CameraTir为 非active 状态。
           问题解决：视角切换功能由Tank.cs搬移至此脚本，判断：当Player不存在，则开启第三人视角
        */

        //当玩家不存在与场景(被销毁)，重启第三人视角
        //if (GameObject.FindWithTag("Player") == null)
        //{
        //    CameraThird.SetActive(true);
        //}
        if (player == null)
            CameraThird.SetActive(true);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        //相机坐标跟随坦克坐标，实现第三人固定视角
        transform.position = player.transform.position;

        //视角也跟随wasd旋转而旋转(本例不适用)
        //transform.rotation = tank.transform.rotation;


        //产生问题：非子物体的物体如何跟随玩家position和rotation
        //         说到底就如何实现 非Player子物体的第一人视角相机
        //问题分析：第一人视角CameraFirst虽然挂载于Player，跟随运动旋转，但不希望其跟随Player一起被销毁，
        //         因为它被一起销毁时，还得上述代码实现第三人视角切换，而Camera类不应该被销毁，只应该启用或禁用
        //解决方案：目前仅有上述判断Player被销毁时启用第三人视角的办法，
        //理想方案：两个相机都挂载在物体TankCamera上，统一由本脚本使用，关键就是CameraThird能跟随Player运动旋转
        //         目前就是下述代码未实现这个跟随功能
        //pos = new Vector3(0f, 2.74f, -1.23f);
        //pos = pos + tank.transform.position;
        ////rot = Quaternion.Euler(9.713f, 0f, 0f);
        ////rot = rot + tank.transform.rotation;
        ////transform.localEulerAngles = new Vector3(0, 90, 0);
        //rot = new Vector3(9.713f, 0f, 0f);
        //rot = rot + tank.transform.localPosition;

        //CameraFirst.transform.position = pos;
        //CameraFirst.transform.localPosition = rot;
    }
}