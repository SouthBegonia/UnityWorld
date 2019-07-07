# 项目展示

![](https://img2018.cnblogs.com/blog/1688704/201907/1688704-20190707185502830-227795879.jpg)


# 制作流程
**表盘绘制：**
采用[Aseprite 像素绘图软件](https://www.aseprite.org)绘制表盘及指针。本例钟表素材大小 *256x256*，存储格式为png，但发现导入Unity后较为失真，建议256+像素或调整Unity内相关参数。

**代码配置：**
设置表盘、指针到合适位置，创建`Clock.cs`脚本挂载于表盘，编写代码：

```
public class Clock : MonoBehaviour
{
    //接收三个指针
    public GameObject hourHand;
    public GameObject minuteHand;
    public GameObject secondHand;

    //小时、分钟、秒钟
    private int h;
    private int m;
    private int s;

    void Update()
    {
        //读取本地时间
        GetNowTime();

        //更新针轴旋转：
        //Quaternion.AngleAxis(angle : float, axis : Vector3)：绕 axis轴旋转 angle角度，创建一个旋转
        //其中绕 axis轴方向：左手拇指指向axis方向，四指所环绕的方向(类似左手螺旋定则)
        hourHand.transform.rotation = Quaternion.AngleAxis((30 * h + 0.5f * m + (30.0f / 3600.0f) * s), Vector3.back);
        minuteHand.transform.rotation = Quaternion.AngleAxis((6 * m + 0.1f * s), Vector3.back);
        secondHand.transform.rotation = Quaternion.AngleAxis((6 * s), Vector3.back);
    }

    //读取本地时间信息
    private void GetNowTime()
    {
        //例如本地时间为 10:23:12
        h = DateTime.Now.Hour;        //h = 10
        m = DateTime.Now.Minute;    //m = 23
        s = DateTime.Now.Second;    //s = 12
    }
}
```

# 问题探讨

- Quaternion.AngleAxis (angle : float, axis : Vector3)：以自身原点为基点，创建返回一个绕axis轴旋转了angle角度旋转的四元数
- transform.Rotate(eulerAngles : Vector3)：以自身原点为基点，应用一个欧拉角的旋转角度，eulerAngles.z度围绕z轴，eulerAngles.x度围绕x轴，eulerAngles.y度围绕y轴。常用于物体简单旋转

Unity中，Transform.rotation是**四元数(Quaternion)**，但Unity以**欧拉角(Vector3)**的形式表示。因此，当我们打算像position赋值那样给rotation直接赋值时，需要赋予Quaternion类型的值(例如`rotation.eulerAngles = new Vector3(90, 0, 0);`)。本例采用的AngleAxis方法就是如此，其**创建返回**了一个新的已经旋转到目标角度的**四元数**，我们用物体的**rotation接收**这个新的四元数即可实现钟表旋转，本质为**修改rotation**。而Rotation方法则是普通常用较为安全的绕某方向旋转的函数，便于设置旋转速度等，可用于人物控制旋转、赛车游戏转向等等，本质为**旋转**

# 参考
- [Unity基础篇：四元数（Quaternion）和欧拉角（Eulerangle）讨论](https://blog.csdn.net/qq_15020543/article/details/82834885)