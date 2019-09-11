using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂在于Hexagon_Sljoint物体上,打算实现空格改变motorSpeed方向,但暂未实现
public class Movement_SliderJoint : MonoBehaviour
{
    private SliderJoint2D sliderJoint;
    private JointMotor2D motor2D;

    public void Start()
    {
        sliderJoint = GetComponent<SliderJoint2D>();
        motor2D = sliderJoint.motor;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            motor2D.motorSpeed = -motor2D.motorSpeed;

            Debug.Log(motor2D.motorSpeed);
        }
    }
}
