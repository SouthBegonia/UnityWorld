using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//第三人视角模式，挂载于TankCamera(第三人视角)
public class TankCamera : MonoBehaviour
{ 
    public GameObject tank;
    public float CameraRotateSpeed = 90f;     //视角旋转速度

    private void Update()
    {
        //小键盘左右键位调整视角旋转
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.down * CameraRotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up * CameraRotateSpeed * Time.deltaTime);
    }
    private void LateUpdate()
    {
        //相机坐标跟随坦克坐标，实现第三人固定视角
        transform.position = tank.transform.position;
        
        //视角也跟随wasd旋转而旋转(非本例用法)
        //transform.rotation = tank.transform.rotation;
    }
}