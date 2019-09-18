using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//显示浮动文本信息的脚本:
public class FloatingText 
{
    public bool active;     //是否启用
    public GameObject go;   //文本对象(全部归于FloatingTextManager物体下)
    public Text text;       //文本信息
    public Vector3 motion;  //文本移动方向
    public float duration;  //文本显示持续时间
    public float lastshown;

    //显示Text
    public void Show()
    {
        active = true;
        lastshown = Time.time;
        go.SetActive(active);
    }

    //隐藏Text
    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    //刷新显示Text
    public void UpdateFloatingText()
    {
        if (!active)
            return;

        //Text持续显示时间
        if (Time.time - lastshown > duration)
            Hide();

        //如何实现Text文本固定在某物体处,而不是Player移动时也跟随移动
        //Debug.Log("go.pos= " + go.transform.position);
        //go.transform.position = Camera.main.WorldToScreenPoint(position);

        //Text显示速度
        go.GetComponent<Transform>().position += motion * Time.deltaTime;
    }
}
