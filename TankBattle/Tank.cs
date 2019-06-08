using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public float MoveSpeed = 7.0f;      //移动速度
    public float RotateSpeed = 90f;     //旋转速度

    private void Update()
    {
        //键位读取
        if(Input.GetKey(KeyCode.W))
            transform.Translate(new Vector3(0, 0, 1) * MoveSpeed * Time.deltaTime);
        if(Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.forward * -MoveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(new Vector3(0, 1, 0) * -RotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
    }
}
