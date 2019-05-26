# C#在Unity3d的应用
参考：
- 《Unity5.x 从入门到精通》
- [siki学院 - Unity中的C#编程](http://www.sikiedu.com/my/course/83)
- [菜鸟教程 - C#教程](http://www.runoob.com/csharp/csharp-tutorial.html)
-----------------

# C#基本语法
前提：
- 环境：VS2017
- 脚本默认绑定在空的游戏对象 **Player** 下
- 保留核心代码，省略命名空间(特殊情况有备注)，原代码见对应文件


## 命名空间
C#文件头部 `using` 开头的代码即为命名空间，例如
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
```

## 类
脚本名必须与类名保持一致，例如文件名 *Players.cs*，则代码有：
```
public class Players : MonoBehaviour { }
```

## 变量
- 创建变量：`变量类型 变量名;`，例如 `string note = "Hello";`
- 作用域：在创建变量时加前缀实现

|前缀|作用域|
|---|---|
|public|可以在 **Insperctor** 视图查看和修改|
|private|不添加作用域则默认为 private |
|protected| ...|

## 数组
C#只能使用内建数组。例如：
```
//CSharpArray.cs
public class CSharpArray : MonoBehaviour {

    public int[] array = new int[5];

	void Start () {
	    for(int i=0;i<array.Length;i++)
        {
            array[i] = i;
        }
        foreach (int item in array) Debug.Log(item);	//打印array里面的内容
	}
}
```
![](https://i.imgur.com/LfK6Mlp.png)


也可使用**ArrayList、List**等容器实现数组：
```
//CSharp2Array2.cs
using System.Collections.Generic;   //使用List容器需要添加该命名空间

public class CSharpArray2 : MonoBehaviour {

    public List<int> list = new List<int>();    //声明一个元素类型为 int 的 List 容器

	void Start () {
	    for(int i=10;i>0;i--)
        {
            list.Add(i);    //按10，9，8...1的顺序往 list 里面添加内容
        }
        list.Sort();    //排序

        foreach (int item in list) Debug.Log(item);     //打印 list 里的内容
	}
}
```
![](https://i.imgur.com/hBmYWqD.png)

## 运算、比较、逻辑操作符
类似 C语言

|类型|字符|
|---|---|
|算术运算符|`+`,`-`,`*`,`/`,`%`,`++`,`--`|
|比较操作符|`>`,`<`,`>=`,`<=`,`==`,`!=`|
|逻辑操作符|`!`,`||`, `&&`|
|条件操作符|`?:`|

## 语句
- **条件语句**：类似 C语言

```
	if(true)	Debug.Log("True");
	else	Debug.Log("Flase");
``` 

- **循环语句**：类似 C语言

```
//CSLoop.cs
public class CSLoop : MonoBehaviour {
    private int i = 0;
    private int x = 0;
    private int y = 0;

	void Start () {
		while(i<10)
        {
            Debug.Log(i);
            i++;
        }

        for(; x<10;++x)
        {
            Debug.Log(x);
        }

        do
        {
            Debug.Log(y);
            ++y;
        } while (y < 10);
	}
}
//循环打印 0~9 三次
```
其次还有**For-in**循环语句：
```
//CSLoop2.cs
public class CSLoop2 : MonoBehaviour {
    private string[] nameArray = { "Jack", "Tom", "Rose" };

	void Start () {
		foreach(string str in nameArray)
        {
            Debug.Log(str);     //遍历数组并打印
        }
	}
}
```
![](https://i.imgur.com/TLfGv8o.png)


- **switch语句**：

```
//CSSwitch.cs
public class CSSwitch : MonoBehaviour {
    private string player = "Jack";

	void Start () {
		switch(player)
        {
            case "Tom":
                Debug.Log("This is Tom");
                break;
            case "Jack":
                Debug.Log("Hi, Jack");
                break;
            case "Rose":
                Debug.Log("Nice to meet you");
                break;
            default:
                break;
        }
	}
}
//输出： Hi, Jack
```

## 函数
参数可以使用 **ref** 关键字声明为传引用参数：
```
//CSharpRef.cs
public class CSharpRef : MonoBehaviour {

	void Start () {
        int score = 110;
		
        ClampScore(ref score);	//传参数的引用

        Debug.Log(score);
	}
	
    void ClampScore(ref int num)
    {
        num = Mathf.Clamp(num, 0, 100);     //限定值在0~100范围内
    }
}
```

也可以使用out关键字实现返回多个数值：
```
//CSharpOut.cs
public class CSharpOut : MonoBehaviour {

	void Start () {
        float num1 = 2f, num2 = 3f;
        float multiply, sum;
        Calculate(num1, num2, out multiply, out sum);
        Debug.Log(multiply);    //输出 6
        Debug.Log(sum);     //输出 5
	}
	
    void Calculate(float num1, float num2, out float multiply, out float sum)
    {
		//将相乘结果和相加结果返回
        multiply = num1 * num2;
        sum = num1 + num2;
    }
}
```
![](https://i.imgur.com/goHgPIY.png)

## C#脚本
1. 凡是需要添加到游戏对象的C#脚本类都需要直接或间接地从MonoBehaviour类继承。
2. 使用`Start` 或者 `Awake` 函数来初始化，避免使用构造函数。原因在于Unity无法确定构造函数何时被调用。
3. 类名要与脚本名相同。如脚本名 *CSharpArray.cs* ，则代码内的类为 `public class CSharpArray : MonoBehaviour { }`
4. 协同函数(`Coroutines`)返回类型必须是`IEnumerator`,并用`yield return` 代替 `yield`

---------------------------

# 访问游戏对象和组件
## MonoBehaviour类
> Unity 中的脚本都是继承自 MonoBehaviour，它定义了基本的脚本行为。

|事件响应函数|说明|
|---|---|
|`OnTriggerEnter /Stay /Exit`|当其他碰撞体进入 /停留 /离开触发器时调用|
|`OnCollisionEnter /Stay /Exit`|当碰撞体或刚体与其他碰撞体或刚体接触 / 保持接触 /停止接触时调用|

## 访问游戏对象

1. 通过名字查找
	- `GameObject.Find()`:若找到对应对象返回该对象的引用，失败则返回空值null，若存在多个重名对象则返回第一个对象的引用。
```
//游戏对象 Cube
GameObject player;
void Start() {
	player = GameObject.Find("Cube");
}
```
2. 通过标签查找
	- `GameObject.FindWithTag()`
```
//游戏对象 Cube 的标签 Enemy
GameObject player;
void Start() {
	player = GameObject.FindWithTag("Enemy");
```

## 访问组件
`GetComponent<>()`：得到对象上的组件，不存在则返回null
```
//继上文得到游戏对象 Player(含Transform组件和Example脚本组件) 后
Transform t;	//声明组件变量
Example script;
t = GetComponent<Transform>();	//得到对象上的Transform组件
t = DoSomething;	//访问组件变量
script  =GetComponent<Example>();	//得到对象上的Example脚本组件
```
设置变量为 public 后：
![](https://i.imgur.com/hpz8gcp.png)


---------------
# 常用脚本API
## TransForm组件
> Transform组件控制游戏对象在Unity场景中的位置、旋转和大小比例，每个游戏对象都包含此组件。


|成员函数|功能|
|----|----|
|Translate|按指定的方向和距离平移|
|Rotate|按指定的欧拉角旋转|
|RotateAround|按给定的旋转轴和旋转角进行旋转|

```
//方块向z轴平移
void Update() {
	float speed = 2.0f;
	transform.Translate(Vector3.forward * Time.deltaTime * speed);
}

//方块绕Y轴自转
transform.Rotate(Vector3.up * Time.deltaTime * speed);

//方块绕世界坐标的Y轴公转
transform.RotateAround(Vector3.zero, Vector3.up, speed * Time.deltaTime);
```


## Time组件
> Unity中通过Time类来获取和时间相关的信息，可以用来计算帧速率、调整时间流逝速度等功能


|成员变量|功能|
|----|----|
|time|游戏从开始到现在经历的时间(秒)(只读)|
|deltaTime|上一帧消耗的时间(秒)(只读)|

## Random类
> Random类可以用来生成随机数

|成员函数|功能|
|----|----|
|Range|返回min和max之间的一个随机浮点数，包含min和max|
```
//设定随机数(全为float)
num = Random.Range(Min, Max);

//随机载入不同的游戏场景
void Example() {
	Application.LoadLevel(Random.Range(0,Application.LevelCount));
}
```

## Mathf类
> Unity中的Mathf类提供了复杂数学公式额解决方法

|变量|说明|
|----|----|
|PI|圆周率(只读)|
|Infinity|正无穷大(只读)|
|NegativeInfinity|负无穷大(只读)|
|Sin,Cos,Tan|计算正弦，余弦，正切值(弧度制)|
|Sqrt|计算平方根|
|Abs|计算绝对值|
|Min,Max|返回若干数值中的最小值，最大值|

```
//Mathf.SmoothDamp函数可以制作相机的缓冲跟踪效果：将脚本绑定在相机上，指定target
public class SmoothDamp : MonoBehaviour {
    public Transform target;
    public float smoothTime = 0.3F;
    private float yVelocity = 0.0F;
	
	void Update () {
        float newPosition = Mathf.SmoothDamp(transform.position.y, target.position.y, ref yVelocity, smoothTime);
        transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);

	}
```

## Input类
> Input类是处理输入的接口

|成员函数|功能|
|----|----|
|GetMouseButton|指定的鼠标按键是否按下|
|GetMouseButtonDown/Up|指定的鼠标按键被按下/松开的那一帧返回true|
|GetKey|当指定的按键被按下时放回true|
|GetKeyDown/Up|当指定的按键被按下/松开的那帧返回true|


## Coroutine协同程序
> Coroutine 也称为协同程序或者协程，协同程序可以和主程序并行运行，和多线程类似。

