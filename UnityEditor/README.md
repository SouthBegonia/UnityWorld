# 前言
对于Unity编辑器的扩展方法众多，扩展对象包括Inspector页面及顶部菜单栏。定制方法有两种：
- Attributes属性进行定制
- 继承Editor类，重写OnInspectorGUI()进行定制

项目地址：[UnityEditor - SouthBegonia](https://github.com/SouthBegonia/UnityWorld/tree/master/UnityEditor)

## Attributes属性进行定制
该部分的扩展方法集中在Inspector中脚本面板，使用了Unity中的 **Attributes 属性** ，使得脚本在Inspector面板中规范化、便捷化。

### [Header] 属性标题
为后续区域代码拟订一个标题，用于区分和概述该区域代码含义
```
[Header("武器")]
public int weapon;
public int ammunition;
public int aurability;
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200405151749141-1928331816.png)


### [Tooltip] 属性提示
实现在Inspector中，鼠标位于该变量名字上时，提示该变量的描述信息
```
[Tooltip("玩家的名字，肯定不再是JOJO对吧")]
public string playerName;
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200405151759240-634109056.png)


### [Space] 空行属性
在Inspector脚本页面创建空行以隔开上下可视参数
```
[Space]
public int health = 100;
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200405151806785-1474475001.png)

### [Range()] 范围值属性
使得变量的值仅可在该范围内修改，并且可以在Inspector页面呈现滑动修改变量的效果
```
[Range(0, 1000)]
public int exp = 0;
```

### [Foldout] 属性折叠
使得多个的变量在Inspector页面实现集合、可折叠效果。（注：本方法并非Unity自带，而是源自项目[InspectorFoldoutGroup - PixeyeHQ](https://github.com/PixeyeHQ/InspectorFoldoutGroup)，如需使用该方法，仅需要将项目的脚本配置到自身unity项目下即可）
[](https://github.com/PixeyeHQ/InspectorFoldoutGroup)
```
[Pixeye.Unity.Foldout("Enemys")]
public GameObject a, b, c, d, e;
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200405151813751-122587915.png)


### [SerializeField] 强制序列化
使得private变量在Inspector脚本页面可见，同时也称为强制序列化
```
[SerializeField]
private int coins;
```

### [HideInInspector] 隐藏属性
使得public变量在Inspector页面不可视，进而实现保护变量
```
[HideInInspector]
public int maxHealth = 100;
```

### [TextArea] 输入域
对于字数较长的字符串，扩展其在Inspector中的编辑区大小（原本仅能单行，且无法自动换行）
```
[TextArea]
public string gameDescribe = "";
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200405151849374-2025023061.png)


### [AddComponentMenu] 添加组件到菜单
写于类名前，可以将该类直接添加到Add Component菜单中
```
[AddComponentMenu("Managers/demo1")]
public class demo1 : MonoBehaviour
{
	// ...
}
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200405151856809-1323145254.png)


### OnValidate() 数据检查
对编辑状态下、Inspector中输入的数据进行检查的函数
```
private void OnValidate()
{
    if (health < 0)
    {
        Debug.LogError("生命值不可为负");
        health = 0;
    }        
    else if (health > 100)
    {
        Debug.LogError("生命值不可超过最大值 " + maxHealth);          
        health = 100;
    }        
}
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200405151904221-1202016204.png)


### [ContextMenu] 上下文菜单
可以为类增加一个上下文弹出菜单，在Inspector页面对当前脚本右键（或者单击脚本图标右侧三个竖点）即可弹出自定义的上下文菜单
```
[ContextMenu("显示当前生命值")]
public void PrintHealth()
{
    Debug.Log("Health = " + health);
}
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200405151911049-1273731685.png)

### [ContextMenuItem] 字段上下文菜单
区别于上面的`ContextMenu`是在脚本名字处右键打开上下文菜单，该方法适用于在某一字段名字上右键打开上下文菜单：参数类型string，前name，后function
```
[ContextMenuItem("ResetHealth", "ResetHealth")]
public int health = 100;

public void ResetHealth()
{	
	health = maxHealth;	
}
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200412142502084-422869231.png)


## 重写OnInspectorGUI()进行定制
除了于上文运用Attributes属性对脚本栏进行定制，也可以将脚本 **继承自Editor类，并重写 OnInspectorGUI()**，从而实现在Inspector页面进行GUI绘制。大部分方法可以使用`EditorGUILayout`类，或是`GUILayout`类，与GUI在Scene中绘制的原理大致相同。

![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200412142514722-107888367.png)
完整代码：[PlayerInspector.cs](https://github.com/SouthBegonia/UnityWorld/blob/master/UnityEditor/Assets/_Scripts/demo2/PlayerInspector.cs)

## 菜单栏的扩展

### [MenuItem] 菜单栏
在编辑状态可点击上方自定义的菜单栏项实现特定功能，即扩展菜单栏项
```
[MenuItem("调试/查看版本信息")]
static void PrintSomething()
{
    // 注意：仅有静态函数才可使用该属性
    Debug.Log("当前Unity版本：" + Application.unityVersion);
}
```
三种重载方法：
```
public MenuItem(string itemName);	
public MenuItem(string itemName, bool isValidateFunction);	//不要把isValidateFunction设为true
public MenuItem(string itemName, bool isValidateFunction, int priority);	//priority为该项在菜单栏中显示顺序
```
![](https://img2020.cnblogs.com/blog/1688704/202004/1688704-20200405151918374-1617159848.png)

# 总述
两种方法皆是对Inspector脚本页面进行定制的方法：Attributes是官方提供的定制属性类，简单便捷五脏俱全；而对Editor类的继承重写则具有多的可扩展性，也相对较麻烦，此外**继承重写Editor的方法不能与Attributes属性共存**，所以在进行各种的定制前请**慎重选择定制的方法**。

# 参考

- [独立游戏开发 - IndiaNova](https://indienova.com/u/dev)
- [InspectorFoldoutGroup - PixeyeHQ](https://github.com/PixeyeHQ/InspectorFoldoutGroup)
- [你不可不知的Unity C#代码小技巧 - Michael Wang](https://mp.weixin.qq.com/s?__biz=MzU5MjQ1NTEwOA==&mid=2247503312&idx=1&sn=f547e5a6dd9c8551ef028c330b5a74f1&chksm=fe1df97bc96a706df98e6d761aefbaff1270432676727eca883c6a426fafeaa1fa76728d26f3&mpshare=1&scene=1&srcid=&sharer_sharetime=1579407428670&sharer_shareid=3700fe0c888383356811eb94c58328eb#rd)
- [Unity Editor基础篇 - Unity墙外世界](https://mp.weixin.qq.com/s/4kporY-PCScRAESy4WSpmA)
- [Unity: make your lists functional with ReorderableList - valentin](https://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/)
- [勾選Bool後才出現的變數！Unity Editor Script入門教學 - OneCredit](https://www.bilibili.com/video/BV1E54y1B7Z5)