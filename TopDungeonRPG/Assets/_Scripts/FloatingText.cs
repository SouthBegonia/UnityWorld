using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText 
{
    public bool active;
    public GameObject go;
    public Text text;
    public Vector3 motion;
    public float duration;
    public float lastshown;

    public void Show()
    {
        active = true;
        lastshown = Time.time;
        go.SetActive(active);
    }

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
