using UnityEngine;
using System.Collections;

//绘制连线脚本,实现两物体之间的连线
public class Line : MonoBehaviour
{
    public GameObject gameObject1;
    public GameObject gameObject2;
    private LineRenderer line;

    void Start()
    {
        //添加LineRenderer组件到物体上
        line = this.gameObject.AddComponent<LineRenderer>();

        //设置线渲染器的宽度
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;

        //设置线条渲染器的顶点数
        line.positionCount = 2;
    }

    void Update()
    {
        if (gameObject1 != null && gameObject2 != null)
        {
            //分别设置两个顶点坐标
            line.SetPosition(0, gameObject1.transform.position);
            line.SetPosition(1, gameObject2.transform.position);
        }
    }
}
