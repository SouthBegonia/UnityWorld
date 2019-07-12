using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;     //单例对象

    public float minDist = 0.1f;
    public bool ____________________;
    public LineRenderer line;
    private GameObject _poi;
    public List<Vector3> points;

    private void Awake()
    {
        S = this;

        //获取对线渲染器的引用
        line = GetComponent<LineRenderer>();
        //在需要使用LineRenderer之前将其禁用
        line.enabled = false;
        //初始化三维向量点的List
        points = new List<Vector3>();
    }

    //属性(伪装成字段的方法)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //当把_poi设置为新对象时，将复位其所有内容
                line.enabled = false;
                points = new List<Vector3>();               
            }
        }
    }

    //清除线条函数
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
            return;

        if (points.Count == 0)
        {
            //如果当前是发射点
            Vector3 launchPos = Slingshot.S.launchPoint.transform.position;
            Vector3 launchPosDiff = pt - launchPos;

            //则添加一根线条，辅助瞄准
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //弃用方法：设置线段数line.SetVertexCount(2)

            //设置前2个点
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);

            //启用线渲染器
            line.enabled = true;
        }
        else
        {
            //正常添加点
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //返回最近添加的点的位置
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
                return (Vector3.zero);
            return (points[points.Count - 1]);
        }      
    }

    private void FixedUpdate()
    {
        if(poi == null)
        {
            //如果兴趣点不存在则找出一个
            if (FollowCam.s.poi != null)
            {
                if (FollowCam.s.poi.tag == "Projectile")
                    poi = FollowCam.s.poi;
                else
                    return; //未找到兴趣点则返回
            }
            else
                return; //未找到兴趣点则返回
        }

        //如果存在兴趣点，则在FixedUpdate中在其位置上添加一个点
        AddPoint();

        //当兴趣点静止时，将其清空
        if (poi.GetComponent<Rigidbody>().IsSleeping())
            poi = null;

        /*此处不宜实现尾拖重置，移步Slingshot.cs内，当发射出一发弹丸后，即可重置下一发尾拖
          且此处相机坐标是动态变化的(见FollowCam.cs内线性插值法实现平滑移动)，用vector3判定是错误的
        if (GameObject.FindWithTag("MainCamera").GetComponent<Transform>().position == new Vector3(0,0,-10))
        {
            //若timeLimit时限内未完成静止，则强制返回
            //poi = null;
        }
        */
    }
}
