//Input中的类函数
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClassFunctions : MonoBehaviour
{
    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;

    void Update()
    {
        /* 轴axes：
         "Horizontal" 和"Vertical" : A、W、S、D和方向键
         "Mouse X" 和"Mouse Y" : 鼠标
         "Fire1", "Fire2" "Fire3" : 键盘的Ctrl、Alt、Cmd键和鼠标中键或控制器的按钮
         在Edit->project Setting->Input 内查看及修改
        */

        //1. Input.GetAxis():根据坐标轴名称返回虚拟坐标系中的值，采用了平滑滤波器
        //返回值类似于 0.0->0.2->0.6->0.1->0.0
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(v, h, 0);

        //   Input.GetAxisRaw():根据坐标轴名称返回一个不使用平滑滤波器的虚拟坐标值
        //返回值仅有 -1、0或1
        float speed = 10 * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        transform.Rotate(0, speed, 0);

        /*---------------------------------*/
        //2. Input.GetButton():当对应的虚拟按钮被按住时返回true
        //系统默认Fire1 为 左键mouse0 或者 left ctrl
        if (Input.GetButton("Fire1"))
            print("Fire1 key is held down");

        //   Input.GetButtonDown():当对应的虚拟按钮被下/松开的那一帧返回true
        if (Input.GetButtonDown("Fire1"))
            print("Fire1 was pressed");
        if (Input.GetButtonUp("Fire1"))
            print("Fire1 was released");

        /*---------------------------------*/
        //3. Input.GetKey():当指定的按键被用户按住时返回true
        if (Input.GetKey("up"))
            transform.Translate(Vector3.up * Time.deltaTime);
        if (Input.GetKey("down"))
            transform.Translate(Vector3.down * Time.deltaTime);
        if (Input.GetKey("left"))
            transform.Translate(Vector3.left * Time.deltaTime);
        if (Input.GetKey("right"))
            transform.Translate(Vector3.right * Time.deltaTime);

        //   Input.GetKeyDown():当指定的按键被按下/松开的那一帧返回true
        if (Input.GetKeyDown("space"))
            print("space key was pressed");
        if (Input.GetKeyUp("space"))
            print("space key was released");

        /*---------------------------------*/
        //4. Input.GetMouseButton():当指定的鼠标按钮被按住时返回true
        /*  0左键 1右键 2中键(注意是中键按下不是滚动) */
        if (Input.GetMouseButton(0))
            print("Pressed left click.");
        if (Input.GetMouseButton(1))
            print("Pressed right click.");
        if (Input.GetMouseButton(2))
            print("Pressed middle click.");

        //  Input.GetMouseButtonDown()：当制定的主表按键被按下/松开的那一帧返回true
        if (Input.GetMouseButtonDown(0))
            print("Pressed left click.");
        if (Input.GetMouseButtonUp(0))
            print("Released left click.");
    }
}
