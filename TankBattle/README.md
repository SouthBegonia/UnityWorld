# 目录
* [游戏原型](#1)
* [项目演示](#2)
* [绘图资源](#3)
* [代码实现](#4)
* [技术探讨](#5)
* [参考来源](#6)

-----------

<h1 id="1">游戏原型</h1>
- 游戏玩法：在有界的战场上，玩家将驾驶坦克，代表绿色阵营，与你的队友一起击溃红蓝阵营的敌人，在这场三方大战中夺得胜利！
- 操作指南：
	- 移动：WASD
	- 开火：Space
	- 第一/第三人称视角转换：PgDn
	- 第三人称下的视角转动：← →

------------------------

<h1 id="2">项目演示</h1>
![](https://img2018.cnblogs.com/blog/1688704/201907/1688704-20190727160643934-6854301.png)

![](https://img2018.cnblogs.com/blog/1688704/201907/1688704-20190727160659744-2016240780.png)


Github项目地址：[3D坦克大战 TankBattle](https://github.com/SouthBegonia/UnityWorld/tree/master/TankBattle)

--------------------

<h1 id="3">绘图资源</h1>
主要素材来源于官方[Tanks教程](https://learn.unity.com/project/tanks-tutorial)中的[Tanks!Tutorial素材包](https://assetstore.unity.com/packages/essentials/tutorial-projects/tanks-tutorial-46209)

- **战场搭建**：直接使用素材包内的LevelArt即可，当然也可在此基础上修改、扩展
- **玩家**：
	- 物体：选用素材包内绿色坦克模型作为Player，除了模型外还包含子物体：发射点ShootPoint，第一人称摄像机FirstCamera，血条画布HealthCanvas
	- 组件/脚本挂载：AudioSource，Tank.cs，TankWeapon.cs
- **敌人/队友**：
	- 物体：除了模型外还包含子物体血条画布HealthCanvas
	- 组件/脚本挂载：AudioSource，NavMeshAgent，AITank.cs，TankWeapon.cs
- **相机**：默认开局为第三人称视角
	- ThirdCamera：采用Orthographic模式，作为TankCamera的子物体，在TankCamera挂载TankCamera.cs脚本进行控制
	- FirstCamera：采用Perspective模式，作为玩家坦克Player的子物体，调整适当位置及角度
- **阵营**：
	- 创建Layer：Ground、Players、Blue、Green、Red，并对应设置
	- 将创建多个敌人归并于各自阵营的空物体下，本例为BlueEnemy，RedEnemy，GreenEnemy
- **UI**：
	- 全局UI：建立Canvas，用于显示屏幕左下角玩家血量，及左侧操作指导，此外还包括死亡、胜利等UI
	- 坦克外围血条UI：新建HealthCanvas，包含血量背景条Health Background及实时血条HealthSlider（素材均来自官方包内）。后将HealthCanvas设于各方坦克预制体下，调整合适大小、高度及显示方式
- **其他**：
	- 背景音乐：另设空物体BackGroundMusic添加AudioSource，开始时即循环播放
	- GameManager空物体：挂载GameManager.cs及全局UI的脚本UI.cs
	- 导航网络烘培：Window->AI->navigation内设置
	- 坦克残骸：另取坦克模型保留基本的物件，修改为灰暗色，修改BoxCollider保持间隔不相互碰撞，添加素材包内TankExplosion爆炸特效。死亡后实现爆冲力将各部件被冲散

![](https://img2018.cnblogs.com/blog/1688704/201907/1688704-20190727160810260-1886037316.jpg)

--------------------

<h1 id="4">代码实现</h1>

|脚本名|挂载于|功能|
|----|----|----|
|shell.cs|炮弹Shell预制体|实现发射的炮弹的爆炸（特效，冲击力，销毁炮弹）|
|TankWeapon.cs|所有坦克的预制体|实现坦克发射炮弹的过程（实例化炮弹，赋予初速度，开火声，炮弹冷却）|
|Tank.cs|玩家Player|实现玩家与坦克的交互（前后左右移动，空格开火）|
|TankCamera.cs|TankCamera|实现第三人视角跟随玩家坦克移动，且还进行第一/第三人称视角的切换控制|
|Unit.cs|作为Tank.cs和AItank.cs的父类|实现所有坦克的基本属性（阵营，血量，死亡爆炸特效，伤害计算）|
|AITank.cs|除玩家外的所有坦克预制体|实现npc坦克的功能（设置阵营、搜索敌人、自动导航、攻击、自身血条UI）|
|UI.cs|GameManager|实现主界面下玩家的血条UI及玩家坦克外围的血条UI，胜利及失败UI，此外还有胜利判定 |
|LayerManager.cs|设置静态方法供其他脚本调用|实现设置阵营(设置Layer)的函数|
|GameManager.cs|GameManager|判定玩家是否死亡，若死亡则重置游戏|

--------------------

<h1 id="5">技术探讨</h1>
**1.炮弹实现射出的方法**：
- 赋予炮弹初速度：`ShellRigidbody.velocity = shootPoint.forward * shootPower;`，（本例方案）
- 施加力：`ShellRigidbody.AddForce(shootPoint.forward * shootPower * 50f);`


**2.开火时炮弹的实例化位置**：
- 产生问题：运动中开火时，实例化的炮弹与自身碰撞
- 问题分析：炮弹发射机理就是炮弹shell在炮口前一定位置实例化并赋予初速度，但是坦克也存在移动，因此假若距离较近，坦克移动速度较快时，容易造成实例化的炮弹立即与自身碰撞
- 解决方案：需要留意位置调整、角度及坦克运动速度，本例实例化炮弹位置即为shootPoint的position

**3.AI坦克对敌判定**
- 产生问题：原思路想采用射线检测来判定前面一定范围是否存在敌人，即`if (hit.collider.gameObject.layer == enemy.layer)`，但现象是：敌人/队友是否开火呈随机发生，无规律
- 问题分析：射线检测到的一直是tank上的部件，而那些部件物体不是enemy，if不满足无法进行射击
- 解决方案：在AITank.cs脚本内另设SearchEnemy()函数，功能是搜索返回距离自己最近的敌人；在Update内首先导航跟踪这个敌人，然后判断当这个搜索到的敌人在开火范围内即可开火，否则重新SearchEnemy()，详细描述见脚本内注释

**4.AI坦克导航**：
- 产生问题：敌人及队友坦克的NavMeshAgent导航混乱
- 问题分析：很大原因在于导航网络的烘培，及NavMeshAgent的参数设置
- 解决方案：方法就是合理设置Navigation内的各项参数及NavMashAgent上的参数，也可直接采用官方教程视频内的参数

**5.第一人称视角**：
- 产生问题：玩家在第一人称视角下被销毁是报错，第一人称视角消失，再无法切换视角
- 问题分析：FirstCamera挂载于Player，当玩家死亡时Player销毁，造成FirstCamera的销毁
- 解决方案：在TankCamera.cs内添加判定，若玩家不存在则启用第三人称视角相机
- 理想方案：两个视角的相机都挂载在物体TankCamera上，统一由本脚本使用；FirstCamera不仅跟随Player正常移动、旋转，且不会在玩家死亡时销毁；该方案暂未较好实现

--------

<h1 id="6">参考来源</h1>
- [Unity3D坦克大战项目实例 - TRULYSPINACH](https://www.bilibili.com/video/av7097738)
- [Unity设置物体旋转角度误区 - feng](https://gameinstitute.qq.com/community/detail/121547)
- [Unity3D中的Layers和LayerMask解析 - AaronBlogs](https://www.cnblogs.com/AaronBlogs/p/8260361.html)
- [Unity3D射线碰撞检测+LayerMask的使用 - 丁小未](https://blog.csdn.net/dingxiaowei2013/article/details/27107209)
- [Tanks - unityLearn](https://learn.unity.com/project/tanks-tutorial)
- [Tanks!Tutorial - unityAssetStore](https://assetstore.unity.com/packages/essentials/tutorial-projects/tanks-tutorial-46209)