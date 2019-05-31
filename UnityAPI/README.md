* [事件顺序](#1)
* [Time类](#2)
* [游戏对象](#3)
	* [基本属性](#3.1)
	* [创建、实例化](#3.2)
	* [销毁](#3.3) 


<h2 id="1">事件顺序</h2>

Version:2019.2b - [Order of Execution for Event Functions](https://docs.unity3d.com/2019.2/Documentation/Manual/ExecutionOrder.html)

![](https://docs.unity3d.com/2019.2/Documentation/uploads/Main/monobehaviour_flowchart.svg)

**首个场景加载时：**下列函数会在场景Scene开始时实行
- `Awake()`:在任何Start()之前，和在预制体prefab实例化instantiated后(若开时游戏对象GameObject未被激活inactive，则无法触发Awake)，仅执行一次
- OnEnable():仅当对象激活active时才触发执行，每激活一次就执行一次

**编辑**：
- `Reset()`:当脚本首次挂载在对象上且点击了Reset脚本命令时触发，Reset()的触发会初始化脚本内的所有内容

**在第1帧刷新前**：
- `Start()`: 在第1帧率先你前触发Start()事件(前提脚本可执行)，仅执行一次

**刷新次序**：
- `FixedUpdate()`:它可以每帧调用多次，关键在于强制每帧执行次数，不受到机器性能或者框架的影响，所有的物理过程计算都发生在FixedUpdate()后
- `Update()`:每帧调用一次，是框架跟更新的主要函数
- `LateUpdate()`:LateUpdate()每帧调用一次，调用时间在Update()后，所有的物理过程计算都在LateUpdate()开始前就计算完毕；该函数的主要用法是第三人称视角，例如你可在Update()内执行角色的移动，在LateUpdate()内进行相机的移动、旋转

其他参考：[Unity编程篇 MonoBehaviour 类](http://baijiahao.baidu.com/s?id=1601985096147802045&wfr=spider&for=pc)


<h2 id="2">Time 类</h2>

- `Time.time`:这一帧开始的时间，Update()内帧数不定，故该值非整数变化，可表示游戏进行的时间(较常用)
- `Time.fixedTime`：最近FixedUpdate()开始的时间，由于FixedUpdate()每次执行间隔固定，故该值规律整数变化，表示从游戏开始到现在的时间
- `Time.realtimeSinceStartup`:游戏启动到现在的时间，即便被暂停或者后台，也会持续计时，可用在计算某函数执行所用的时间 `( 类似C的 start=clock(),end=clock()，然后用时为(double)(end - start) / CLOCKS_PER_SEC) )`
- `Time.deltaTime`:时间增量，上一帧完成的用时，一般在1/60s左右波动
- `Time.fixedDeltaTime`：固定的时间增量，为0.2s定值(Unity默认值，在Edit->Project Setting->Time里面查看)
- `Time.frameCount`:游戏开始到现在执行了多少帧
- `Time.timeScale`:调整时间的比例，用于游戏中的慢动作效果；当时间比例为1.0时，时间的流逝与实时一样快，当时间比例为0.5时，时间比实时慢2倍


<h2 id="3">GameObject 类</h2>

**GameObject** 是场景里所有实体的**基类**，但是在 `GameObject类` 中许多`变量`被移除，取而代之的是另一种访问方式：例如 `GameObject.Renderer` 以 `GetComponent<Renderer>()` 代替等。此外GameObject类 继承自 Object 类

<h3 id="3.1">基本属性</h3>

- `GameObject.activeSelf`:表征此游戏对象自身的活动状态
- `GameObject.activeInHierarchy`:表征游戏对象及其所有父物体和祖先物体或者子物体的activeself状态(较为常用在物体数量众多的情况)
- `GameObject.layer`：游戏对象所在层次
- `GameObject.scene`：游戏对象所在场景
- `GameObject.tag`:游戏对象的标签属性
- `GameObject.Transform`:游戏对象的基本坐标属性(所有对象必有)


<h3 id="3.2">创建游戏对象</h3>

- `Instantiate(T original, Vector3 position, Quaternion rotation)`：克隆T游戏对象original，返回克隆体，常用于实例化对象
- `GameObject.CreatePrimitive(PrimitiveType.type)`：创建一个带有原始网络渲染器Mesh Renderer 和对应碰撞器collider 的对象，type类型如Cube，Sphe，Plane，capsule等
- `AddComponent<>()`：给游戏对象添加各类组件或者脚本

<h3 id="3.3">销毁游戏物体或组件</h3>

- `Destroy(Object, float t)`：在延时ts后销毁游戏对象Object，也可以是其他组件或脚本，例如 `Destroy(GetComponent<MoveScript>(),5f);`


<h3 id="3.4">公用方法</h3>

- `GameObject.AddComponent(string className)`：将名为classname的组件类添加到游戏对象中
- `GameObject.CompareTag(string tag)`：判断游戏对象的标签是否为 tag
- `GameObject.GetComponent<T>()`：返回挂在在游戏物体上的 T 组件，这是访问组件的主要方式，也称为泛型方法(generic methods)
	- `GameObject.GetComponentInChildren<T>()`：返回游戏对象或者其子物体上挂载的 T 组件（深度优先遍历，找到的对象必须为active状态）
	- `GameObject.GetComponentInParent<>()`：返回游戏对象或者其父母物体上挂载的 T 组件（向上遍历，找到的对象必须为active状态）
- `GameObject.SetActive(bool value)`：根据接收的布尔值 激活/取消激活物体的active状态
- `GameObject.BroadcastMessage(string methodName)`：广播消息，在所有继承MonoBehaviour类脚本的游戏对象或者其子物体的脚本内(且必须为active状态)，调用名为methodName的方法

<h3 id="3.5">静态方法</h3>

- `GameObject.CreatePrimitive(PrimitiveType.type)`：创建一个带有原始网络渲染器Mesh Renderer 和对应碰撞器collider 的游戏对象，type类型如Cube，Sphe，Plane，capsule等
- `GameObject.Find(string name)`：查找名字为name的游戏对象，若存在且对象状态为active 则返回该对象；若不存在则返回空值（耗费性能，不推荐）
- `GameObject.FindWithTag(string tag)`：查找、返回标签为tag的active对象，没有则返回空值（推荐方法）
- `GameObjectFindGameObjectsWithTag(string tag)`：查找返回标签为tag的所有游戏对象的列表

<h3 id="3.6">继承成员</h3>

由于`GameObject`继承自`Object`，故持有 Object 的一些方法：

- 属性：
	- `Object.name`：对象的名称
- 公用方法：
	- `Object.ToString()`：返回对象的名称
	- `Object.GetInstanceID()`：返回对象的实例标识符
- 静态方法：
	- `Object.Destroy(obj, t)`：立即销毁对象obj，或者在t秒之后销毁；销毁对象同一放置于垃圾池，待后续处理
	- `Object.DestroyImmediate(obj, allowDestroyingAssets = false)`：立即销毁对象obj，用在编辑器代码内
	- `Object.DontDestroyOnLoad(target)`：在加载新场景时阻止销毁target对象，因为每次加载新场景都会销毁所有旧场景的对象
	- `Object.FindObjectOfType(Type type)`：搜索1个挂载有type类型组件的对象，并返回它（前提不被禁用）
	- `Object.FindObjectsOfType(Type type)`：返回挂载有type类型组件的对象的列表
	- `Object.Instantiate(Object original, Vector3 position, Quaternion rotation`：实例化一个original的对象，初始坐标是position，初始旋转角度rotation；返回实例化对象的克隆体


## 延时机制

- Invoke("Function", 5f)：Unity的一种委托机制，可在 Start、Update、OnGUI、FixedUpdate、LateUpdate 中被调用，功能是在5s后调用Function()方法，常用于普通性延时