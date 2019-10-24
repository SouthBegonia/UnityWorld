using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//鼠标单击屏幕产生射线，设定distance检测碰撞的物体：
public class Player : MonoBehaviour
{
    public static Player instance;
    private Ray camerRay;                       //声明射线   
    private RaycastHit cameraHit;               //声明射线检测

    [Range(1, 20)]
    public float RayDistance;
    public Material material;

    private Vector3 mousePos;   //记录将鼠标

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {

        //当点击鼠标左键的时候，以鼠标在摄像机屏幕位置发射一个射线进行检测
        if (Input.GetMouseButton(0))  
        {
            //mousePosition坐标范围： 左下0,0 ~ 右上屏幕像素(width,height)
            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;
            mousePos.z = 0;


            //若相机为perspective模式，射线为发散形状；若为orthoGraphic，则为垂直与相机面的直线段
            //此外，默认长度为单位向量
            camerRay = Camera.main.ScreenPointToRay(mousePos);

            //物理检测射线，out一个RaycastHit类型的 hitInfo 信息，float distance是射线长度，layerMask为可检测的Layer层
            if (Physics.Raycast(camerRay, out cameraHit, RayDistance, LayerMask.GetMask("Anchor")))
            {
                Debug.Log(cameraHit.transform.gameObject.name);
                cameraHit.transform.gameObject.GetComponent<MeshRenderer>().material = material;
                
            }          
        }

        //绘制射线
        Debug.DrawRay(camerRay.origin, camerRay.direction * RayDistance, Color.red);
        //Debug.DrawRay(camerRay.origin , camerRay.direction, Color.red, 1f);
    }
}
