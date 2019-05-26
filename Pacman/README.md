## 项目展示
![](https://img2018.cnblogs.com/blog/1688704/201905/1688704-20190526211931790-1526219776.jpg)

![](https://img2018.cnblogs.com/blog/1688704/201905/1688704-20190526211939061-236408026.gif)


## 涉及知识

- 切片制作 Animations 
- 状态机设置，any state切换，重写状态机
- 按键读取进行整数距离的刚体移动
- 用射线检测碰撞性
- 渲染顺序问题
- 单、多路径的实现
- Button 按键功能

## 准备工作

**Pixels Per Unit：**多少像素相当于Unity一个单位，迷宫Maze大小232x256，

**Pivot**：设置贴图的零点，Bettom Left左下角

**物理化：**墙，import package->custom package，导入已经设置好碰撞体的墙

**pacman切图,动画片段：**Sprite Mode->multiple，Pixels Per Unit=8，进行Sprite Editor，显示其窗口。选择Slice切片，Type为Grid By Cell Count，切割参数3行4列，Apply后可在Pacman下面看到切割好的12张照片。

**动作制作：**12张照片每3张为一个动作，分别是右，左，上，下，每次将3张拖入Hierarchy面板，保存在Animations文档下，各自命名。可在Project面板Animations文件夹下包含4个动画文件，说明每次保存的3张图片生成一个动画，还包含4个动画机(但只需要设置一个。其余可删除)


## 初始状态机设置

**状态机：**在主角Pacman内添加Animator组件，添加上述留下的动画机，打开Animator页面，看到4个组件，初始情况为：当游戏物体进入状态机，默认状态转变为PacmanRight；后拖拽其他3个状态到状态机页面

**分析主角移动：**仅仅能横x纵y向移动，当持续按住某方向键位，速度为每0.3s移动 1 Unit
**
Any State：**切换连接4个状态，点击连线可看到说明：无论在任何状态只要达到连线内条件，即转变状态到所指对象状态

**Any State 切换条件：**在Parameters内添加float型DirX，DirY值用来判断(持续按键产生的是浮点数)。例如PacmanRight的判断，添加DirY，当DirY>0.1(浮点数不精确性质，留一定范围)。并且取消状态机Settings内Can Transition To（考虑到帧数问题，和重复播放初始动作问题），其次2D动画将融合调0

其他：可以调节动画的speed以调节播放该动作的速度



## 吃豆人 Pacman

**吃豆人的实体化**：
- 加碰撞器，circle collider，添加rigdbody2D，设置重力0


**吃豆人的移动方法**：
1. 修改**transform瞬移**，修改坐标，多用在生成位置
2. **rigidbody2D移动**，物理移动，推荐使用

**具体实现移动过程：**
1. 调用 `Vector2.MoveTowards(transform.position,dest,speed)` ，使得返回**起始点到目标点的中间值**，另设 `temp` 接收这个值；再对**刚体进行移动**操作`GetComponent<Rigidbody2D>().MovePosition(temp)；`
	- **Vector2.MoveTowards**(transform.position,dest,speed)：表示以 浮点数型 speed 的速度，从起点 transform.position 移动到终点 dest ，返回的值为两点坐标的中间值
2. 初始时 `transform.position = test` ，故不会移动，因此需要**按键检测**以改变 `dest` 的值：
	- `Input.GetKey(KeyCode.UpArrow)` 或者 `Input.GetKey(KeyCode.W)` 实现**读取键位**
	- 然后赋值 `dest：(Vector2)transform.position + Vector2.up;`

**实现吃豆人每次单位移动**：`(Vector2)transform.position + Vector2.up` ，表示 当前坐标position 加 向上一个单位量；每次读取键盘方向信息，将**当前坐标** + **某方向单位量** = **目的地位置**


*产生问题1：此时移动会造成吃豆人旋转问题*
原因：Pacman 与墙壁碰撞时Z轴坐标改变造成旋转
解决：冻结Z轴 Rigdibody2D->Constraints->Freeze Rotation Z

*产生问题2：卡槽间移动容易卡住，非规则移动*
原因：问题在于**按键过程时刻改变dest** ，造成 temp = Vector2.MoveTowards（）的时刻改变
解决：判断当上一个dest抵达时才读取新的键位if ((Vector2)transform.position == dest)

*产生问题3：首次测试按键移动发现撞墙后就不可再移动*
原因：抵达墙边时，键盘读取的dest到了墙以外，if判断永远无法transform.position==dest，无法在键盘读取
解决：检测目的地合法性

```
//检查目的地是否合法 dir方向值(上述的Vector.XXX)
private bool Valid(Vector2 dir)
{
    //pos 存储当前位置(墙内的合法位置)
    Vector2 pos = transform.position;

    //从 当前值pos+方向值dir 的位置发射一条射线到Pacman 当前的位置pos
    RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);

    //射线打到的碰撞器 是否等于 吃豆人的碰撞器：
    //若射线从墙中心(不合法位置)射出，hit.collider为墙的，不等于Pacman的，返回fault
    //若射线从路面(合法位置)射出，hit.collider等于Pacman的，返回true
    return (hit.collider == GetComponent<Collider2D>());
}
```

## 状态机的切换

**实现不同动作状态机的切换：**
- 获取移动的方向：`Vector2 dir = dest - (Vector2)transform.position;`
- 把获取到的方向设置到状态机：`GetComponent<Animator>().SetFloat("DirX", dir.x);`
![](https://img2018.cnblogs.com/blog/1688704/201905/1688704-20190526210629994-352869969.png)



**2D游戏Z轴问题：**若在不同层级碰撞功能失效，若在同一层级，则存在渲染顺序问题(谁覆盖谁)

**渲染顺序问题：**Sprite Renderer->Order in Layer 
- 小的先渲染，大的后渲染：迷宫Maze 0，豆子pacdot 1，敌人 2~5，Pacman 6
- 小的被覆盖，大的覆盖：先渲染的存在于底层，后渲染的位于上层(类似ppt中的图层)
![](https://img2018.cnblogs.com/blog/1688704/201905/1688704-20190526210728877-485783599.png)
![](https://img2018.cnblogs.com/blog/1688704/201905/1688704-20190526210733986-1977340104.png)


## 豆子及敌人的创建、移动

**豆子：**
1. Pacdot 即为豆子图标，拖入页面内创建对象
2. 对其添加碰撞器 Box Collider 2D，设置为**触发器**
3. 对所有Pacdot添加脚本 `Pacdot.cs` 
	- 脚本内创建**碰撞检测** ：`OntriggerEnter2D(Collider2D collision)` 函数用来检测**触发 Pacdot 的物体是否为Pacman** ，是则销毁Pacdot

**敌人的创建：**
1. 重复切图，合成动作，设置图层，安放位置
2. 关于状态机，采用 **重写状态机**：
	- 在 Animation文档内 create->**Animator Override Controller**，设置状态机参照 Controller 为 Pacman的，可看到 Original 为 Pacman内的动作，**Override** 内的就设置为每个敌人的不同动作（注意，删除原有的状态机使得物体的 Animator 内 Controller 找不到状态机组件，此时需要将重写后的状态机设置到它身上
3. 再设置 Rigidbody 和 CircleCollider(注意此处要设置为**触发器Trriger**而不是碰撞器，范围0.8
![](https://img2018.cnblogs.com/blog/1688704/201905/1688704-20190526210531304-842430402.png)
![](https://img2018.cnblogs.com/blog/1688704/201905/1688704-20190526210646767-560170477.png)


**敌人的移动(单路径)：**
1. 创建与豆子坐标一致的、始末位置同点的闭合路径(用空物体作路径点即可)，统一储存于 way 结构内作为一条路径
2. 编写 `EnemyMove.cs` 
	- 创建循环队列保存所有路径点 `Transform[] WayPoint` ，及`index` 标记敌人在前往哪个路径点的途中；
	- 创建 `FixedeUpdata()` ，判断：若怪物没抵达目标位置，则从当前位置持续移动直到抵达(同Pacman的移动，但没有输入检测)，若抵达，则index++，前往下一个点；此外动画状态的检测、切换也同Pacman的；
	- 设置触发检测：当检测到触发的物体是 Pacman ，则销毁 Pacman
3. 在Unity页面将全部路径点拖拽到怪物脚本的Way Point处实现赋值数组
![](https://img2018.cnblogs.com/blog/1688704/201905/1688704-20190526210840146-903114872.png)


**敌人的移动(多路径)**：高级的方法是AI，但本例采用多路线随机分配实现
1. 先创建对象 `wayPointsGo` 用来接收为预制体的路线 `Way`；
2. 创建**表** `wayPoints` ，在 start() 内调用**foreach方法**，将 `wayPointGo` 内的组件取出到 `t`，将 `t.position` 坐标顺序添加到`wayPoints` 表内(还需修改FixedUpdate()函数内: wayPoints[index] 此时为表，存储的是坐标，无需再.position，后续的wayPoints.Length也改为了wayPoints.Count),由此实现了一条路径；
3. 修改EnemyMove.cs，游戏对象 wayPointsGo 改为**数组形式** 实现存储多路线
4. 根据前面路径Way预制体的制作方法，再次制作多条路径

```
private void LoadApath(GameObject go)
{
    //将wayPointsGo数组内某一路径的子物体(路径点)的Transform组件取出，依次将其position赋值到Ways表中
    //修改为多路径后随机从5条路径走
    foreach (Transform t in wayPointsGo[Random.Range(0, 4)].transform)
    {
        wayPoints.Add(t.position);
    }
}
```
- 创建LoadApath函数：首先清除上调路径遗留再List中的信息，后foreach()加载路径到List内，而后再Start内每次调用随机，传入随机一条路径。

*产生问题1：不同怪物出门都与 Blinky红色敌人 同一点问题*
原因：因为做预制体way1,way2,...,wayn 时，路径始末两点坐标都是 Blinky红色敌人 上方3个单位，所以其他敌人起始移动就会先进行穿墙到那个点
解决：修改 `EnemyMove.cs` ，创建一个坐标变量 `startPos` 用在存放每个敌人路径的始末位置点，在 `Start()` 函数内初始化设置 `satrtPos` 为怪物起始坐标+向上3个单位；再后续 `foreach()` 内插入该点到List表头，及在List末尾添加该点，但注意每次要调用LoadApath()函数要清除上一次路径信息

```
//清空List内前次路径的信息
wayPoints.Clear();

//添加首末路径点到List内
wayPoints.Insert(0, startPos);
wayPoints.Add(startPos);
```

*产生问题2：即便随机路径下不同怪物选到同路径问题*
原因：每个敌人进行 `Random.Range(0,n)` 随机分配路径时可能抽到一样的随机数
解决：添加 `GameManager.cs` ，调用如下代码

```
public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3};

    private void Awake()
    {
        _instance = this;
        int tempCount = rawIndex.Count;

        for (int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }
    }
}

//再修改EnemyMove,Start内
LoadApath(wayPointsGo[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder - 2]]);
```

## 超级豆子

**超级豆子的生成：**
- GameManager.cs内：
	1. 创建pacdotGos，foreach()存储所有豆子；
	2. 生成超级豆子：CreateSuperPacdot()
	3. 创建布尔变量isSuperPacman(初始为false)；
- Pacdot.cs内：
	- 修改碰撞触发判定：if(是超吃豆人状态){} else 被敌人消灭
- EnemyMove.cs内:
	- 修改碰撞触发判定：if(碰到的是超级吃豆人){} else 消灭吃豆人

**超级豆子带来的超级吃豆人状态**：敌人静止，且可以被吃掉
- Pacman的超级状态：OnEatSuperPacdot()
	- `GameManager.cs` 内添加布尔变量 `isSuperPacman` 判定是否超级状态
	- 当吃到超级豆子后启用该函数，变更状态标记 `isSuperPacman = true` 
	- 启用静止敌人函数 `FreeEnemy()`(下有说明)
	- 实现保持超级状态4s：协程延时 `StartCoroutine(Recover());` （下面说明）
	- 4s时间结束后取消状态(同在协程函数Recover()内进行)
- 敌人静止： FreezeEnemy()
	- `Blinky.GetComponent<EnemyMove>().enabled = false;` 禁用怪物移动脚本的update方法
	- `Blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);` 敌人图标变暗淡
- 敌人被吃掉：
	- 在敌人移动脚本 `EnemyMove.cs` 内检测，若碰撞检测到的 Pacman 是超级状态 `GameManager.Instance.isSuperPacman` 则自身回到出生点

**协程延时：**
- 协程的作用：当 启动 `OnEatSuperPacdot()` 变更为超级状态状态时，启用协程函数 `StartCoroutine(Recover())` ，表示该协程函数与 `OnEateSuperPacdot()` **同时启用，并行运行**
- 实现功能：在吃到超级豆子瞬间即开始计时，计时完后取消敌人静止状态`Dis_FreezeEnemy()` 及恢复吃豆人状态 `isSuperPacman = false`
- 延时代码：`yield return new WaitForSeconds(4f);`


**吃豆过程概述：**
- 开局一段时间后生成超级豆子
- 豆子被吃掉后，从表内移除该豆子，销毁对象，延时10s后准备下一个超级的生成，与此同时改变吃豆人状态isSuperPacman=true(两过程不干涉，并行执行)
- 若在超级吃豆人状态期间吃到敌人，则敌人位置回归到初始
- 持续超级吃豆人状态4s后取消敌人的冻结，并调用状态恢复函数



## UI设计

**Start 与 Exit 图标：**
1. 创建 `UI->Canvas`，作为UI工作区域；
2. 添加image作为logo；创建空物体命名StartPanel，包含2个 `UI->text，start和exit` ，修改字体，调整位置
3. 创建空物体命名GamePanel，包含3个UI->text，remain，eaten，修改字体，score
4. 倒计时321动画：同理将素材文件Start切片，设置动作，修改每个动作间隔时长为1s(Animation->Samples)
5. GameManager.cs 内持有UI面板及动画、音乐


**建立 Button 按键跳转：**
1. 对`Start、Exit` 两UI添加 `Button(script)`组件，设置 `Target Graphic->Start,Exit`；
2. 在`GameManager.cs`内添加 `OnStartButton()` 和 `OnExitButton()`对接按键脚本,如下图代码：
3. 启用Button功能：对UI图标添加 `On Click->GameManager->OnStartButton` 启用该函数
3. 其他：按键或者其他UI都必须存在于 **画布Canvas** 内；画布Canvas下一级是 **画板Panel** ；在下一级就是**各类UI组件** 
![](https://img2018.cnblogs.com/blog/1688704/201905/1688704-20190526210817610-1887387326.png)



```
//当点击 Start 
public void OnStartButton()
{
	//与点击开始按钮后同步进行的函数
	StartCoroutine(PlayStartCountDown());

	//Start声音，声音源在原点位置
	AudioSource.PlayClipAtPoint(startClip, 	Vector3.zero);

	//隐藏开始按钮的页面
	startPanel.SetActive(false);
}


//点击Exit后退出游戏
public void OnExitButton()
{
    Application.Quit();
}
```

## 其他

**网页跳转：**
调用 `Application.OpenURL("https://www.cnblogs.com/SouthBegonia/");` 即可实现，可用在Button 也可用在其他触发时间