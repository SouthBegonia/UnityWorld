# Unity3D

**Unity操作**：
* [调试](#1)
* [碰撞体](#2)
* [触发器](#3)
* [视角](#4)
	* [键盘视角平移](#4.1) 
* [光照贴图](#5)
* [游戏对象Gameobject](#6)
	* [访问对象](#6.1) 
	* [实体化对象 Instantiate](#6.2) 
	* [得到组件](#6.3)
	* [对象的移动](#6.4)
* [交互类](#7)
	* [鼠标输入](#7.1)
	* [键盘输入](#7.2)	 


<h2 id="1">调试</h2>
在某函数内进行`Debug.Log(...)`可实现调试检测，例如：

```
public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Hello");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(" World");
	}
}
//在控制台查看调试信息
```

<h2 id="2">碰撞体</h2>

前提：碰撞两者都有碰撞器 Collider ，至少一物体有刚体 Rigidbody（详细前提见 [Unity-Manual-Colliders](https://docs.unity3d.com/Manual/CollidersOverview.html) ）

```
// 碰撞触发检测
 private void OnCollisionEnter(Collision collision)
    {
        print(collision.collider);		//获取碰撞体+碰撞器类型
        print(collision.collider.name);		//碰撞体名称
        print(collision.collider.tag);		//碰撞体标签
    }

// 碰撞结束检测
private void OnCollisionExit(Collision collision)
        print("OnCollisionExit");

// 碰撞持续检测
private void OnCollisionStay(Collision collision)
        print("OnCollisionSaty");
```

<h2 id="3">触发器</h2>

存在于碰撞体collider页面栏，勾选is Trigger 则表示物体为触发器(可以穿过)，否则物体为碰撞体。

<h2 id="4">视角</h2>

游戏视角的移动

<h3 id="4.1">键盘视角平移</h3>
对`Main Camera`创建脚本，在`Update`内调用：`transform.Translate()`实现视角移动

<h2 id="5">光照贴图</h2>

灯光是实时计算的(默认设置：`Light|Mode|Realtime`)，对不变动的灯光进行贴图可以节省资源，在`Windows|Rendering|Lighting Setting` 打开了`Lighting Setting` 页面点击 `Generate Lighting` 实现灯光贴图。

<h2 id="6">游戏对象</h2>

关于Gameobject的一些列操作


<h3 id="6.1">访问对象</h3>

在脚本内创建一个字段，例如子弹`bullet`：`public Gameobject bullet;`，在Unity页面通过将`Prefab`赋到脚本内`bullet`一栏，从而实现在脚本内通过`bullet`字段访问到`Prefab`。

<h3 id="6.2">实体化游戏对象</h3>

实例化用到了`GameObject.Instantiate(m_object, m_transform.position,m_transform.rotation)`
- GameObject.Instantiate()：创建实体化函数
- m.object：所创建的预制体
- m_transform.position，m_transform.rotation：预制体位置及旋转角度，默认下为`transform.position,transrotation`说明地址为脚本所在对象的位置；也可自行修改

用法：

```
//创建对象 b 接收实体化的预制体 bullet
GameObject b = GameObject.Instantiate(bullet, transform.position, transform.rotation);
```

<h3 id="6.3">得到组件</h3>

```
//rgd 得到对象 b 的刚体组件
Rigidbody rgd = b.GetComponent<Rigidbody>();
```

<h3 id="6.4">对象的移动</h3>


<h2 id="7">交互类</h2>

<h3 id="7.1">鼠标输入</h3>

`Input.GetMouseButtonDown()`：读取鼠标按下情况，括号内容即为鼠标不同状态：
- 0：左键
- 1：右键

<h3 id="7.2">键盘输入</h3>

`Input.GetAxis("")`：返回键盘输入的浮点数值
- Horizontal：读取键盘AD左右移动的值(反映在Unity中的X方向)
- Vertical：读取键盘WS上下移动的值反映在Unity中的Y方向)

用法：`float h = Input.GetAxis("Horizontal");`