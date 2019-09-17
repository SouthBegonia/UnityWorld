using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//相机跟随脚本:
public class CameraFollow : MonoBehaviour
{
    private Transform lookAt;           //相机跟随的目标(Player)
    public float boundX = 0.3f;         //X轴差值范围
    public float boundY = 0.15f;        //Y轴差值范围

    private void Start()
    {
        lookAt = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {
        //移动的差值delts:
        //实现在一定范围内Player移动时相机不跟随,但是当超出一定范围时相机跟随移动
        //也就是说:根据相机与Player的间距值,判断是否跟随移动
        Vector3 delts = Vector3.zero;

        //X轴的差值:若相机与Player间距超过范围值,则对差值delts赋值
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX)
        {
            if (transform.position.x < lookAt.position.x)
                delts.x = deltaX - boundX;
            else
                delts.x = deltaX + boundX;
        }

        //Y轴的差值:若相机与Player间距超过范围值,则对差值delts赋值
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY)
        {
            if (transform.position.y < lookAt.position.y)
                delts.y = deltaY - boundY;
            else
                delts.y = deltaY + boundY;
        }

        //设定相机的最新位置
        transform.position += new Vector3(delts.x, delts.y, 0);
    }
}
