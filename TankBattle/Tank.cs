using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public float MoveSpeed = 8.0f;      //移动速度
    public float RotateSpeed = 90f;     //旋转速度
    public GameObject CameraFirst;      //第一人视角
    public GameObject CameraThird;      //第三人视角
    private bool isCameraActive;        //是否启动视角

    private void Start()
    {
        isCameraActive = true;      //初始化启动视角(第三人)
    }

    private void Update()
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
        
        
        //PageDown实现第一人和第三人视角的切换
        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            CameraFirst.SetActive(isCameraActive);  //true
            isCameraActive = !isCameraActive;
            CameraThird.SetActive(isCameraActive);  //false
        }
    }
}
