using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//第三人视角模式，挂载于TankCamera
public class TankCamera : MonoBehaviour
{ 
    public GameObject tank;

    private void LateUpdate()
    {
        transform.position = tank.transform.position;
        //不可再进行视角旋转
        //transform.rotation = tank.transform.rotation;
    }
}