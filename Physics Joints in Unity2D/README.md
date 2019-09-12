# Unity2D中的物理关节
- **本文概述:** 分析Unity中几个**2D物理关节组件**的基本功能、使用方法、运用场景等
- **开发环境:**Unity2019.3.0a2 / VS2017
- **项目资源包:** [2D Joints Starter](https://www.raywenderlich.com/1766-physics-joints-in-unity-2d)
- **说明:** 较比于源项目,我自行做了如下设置
	- 主场景为Demo
	- 对Unity新版本下的新参数进行解释
	- 简单做了新的UI
	- 对各脚本的注释及修改
	- 对场景下物体有序归类: 将各组件的示例物体归于对应组件名称的空物体下,例如涉及DistanecJoint2D组件的物体存放于Distance Joint 2D物体下等

![](https://img2018.cnblogs.com/blog/1688704/201909/1688704-20190912111042477-397631045.gif)

----------------

## Distance Joint 2D 距离关节
- **基本功能:**
	- 使两物体被距离关节限制,保持一定距离
	- 一个物体可对另一个物体做基于物理特性的环绕运动(环绕物体与被环绕物体)
	- 注意:环绕物体自身不旋转
- **使用方法:** 
	- **Enable Collision**:被关节连接的两个物体是否能相互碰撞
	- **Connected Rigid Body**:定义被环绕物体的锚点到指定物体上
	- **Auto Configure Connected**:自动配置锚点与世界空间中的锚点匹配(暂没用过,但我相信点了你会Ctrl+Z的)
	- **Anchor**:环绕物体锚点的坐标
	- **Connected Anchor**:被环绕物体锚点的坐标
	- **Auto Configure Distance**:自动计算并设置两物体的距离,即在运行时拖拽环绕物体则会改变Distance,若关闭则Distance始终为预设值
	- **Distance**:两物体的距离
	- **Max Distance Only**:使得物体摆动到较高位置时会沿关节连线方向下沉稍许距离.若关闭,物体在环绕运动中的距离始终为Distance
- **实现用途:**
	- 摆锤

![](https://img2018.cnblogs.com/blog/1688704/201909/1688704-20190912111101408-1861916546.gif)

## Spring Joint 2D 弹簧关节
- **基本功能:**
	- 使两物体被弹簧关节限制,保持一定范围距离
	- 一个物体可对另一个物体做基于物理特性的弹簧拉伸压缩运动(副物体与主物体)
	- 注意:副物体在围绕主物体作类弹簧运动时,自身也会旋转
- **使用方法:** 
	- **Enable Collision**:被关节连接的两个物体是否能相互碰撞
	- **Connected Rigid Body**:定义主物体的锚点到指定物体上
	- **Auto Configure Connected**:自动配置锚点与世界空间中的锚点匹配
	- **Anchor**:副物体锚点的坐标
	- **Connected Anchor**:主物体锚点的坐标
	- **Auto Configure Distance**:自动计算并设置两物体的距离,即在运行时拖拽环绕物体则会改变Distance,若关闭则Distance始终为预设值
	- **Distance**:两物体的距离
	- **Damping Ratio**:阻尼比,值范围[0,1]
	- **Frequency**:震动频率,单位Hz,值范围[0,1000000],但个人认为[0,10]较为常用
- **实现用途:**
	- 弹簧
	- 弹射器

![](https://img2018.cnblogs.com/blog/1688704/201909/1688704-20190912111117794-1654017808.gif)

## Hinge Joint 2D 铰链关节
- **基本功能:**
	- 使物体围绕一个固定点旋转
- **使用方法:** 
	- **Edit Joint Angular Limits** :手动调节旋转中心及可旋转角度范围
	- **Enable Collision**:物体是否能相互碰撞(但相比于前2个关节,此处用法暂不明)
	- **Connected Rigid Body**:定义物体的锚点到指定物体上
	- **Auto Configure Connected**:自动配置锚点与世界空间中的锚点匹配
	- **Anchor**:物体旋转中心较于物体自身锚点的坐标.通常调节此参数
	- **Connected Anchor**:暂不明具体含义,总之使用Edit Joint Angular Limits足矣
	- **Use Motor**:是否使用马达.若使用,则使铰链关节保持稳定转速;若不适用,则就是简单受到重力因素而摆动的铰链关节
		- **Motor Speed**:旋转速度.单位米/秒或者角度/秒,取决于是线性马达还是角电机.可为负值表示反方向
		- **Maximum Motor Force**:最大扭矩值,表示到最大旋转速度的难易程度.建议默认值
	- **Use Limits**:是否使用角度限制.若使用则铰链关节旋转到限制角度后就停止.参数包含Lower Angle及Upper Angle
- **实现用途:**
	- 横版游戏中的障碍,跳台
	- 门

![](https://img2018.cnblogs.com/blog/1688704/201909/1688704-20190912111131691-732444775.gif)

## Slider Joint 2D 滑动关节
- **基本功能:**
	- 使物体可在某方向上进行滑动(自动或手动)
- **使用方法:** 
	- **Enable Collision**:物体是否能相互碰撞
	- **Connected Rigid Body**:定义物体的锚点到指定物体上
	- **Auto Configure Connected**:自动配置锚点与世界空间中的锚点匹配
	- **Anchor**:滑动物体的锚点,默认0为物体中心
	- **Connected Anchor**:物体滑向的锚点
	- **Auto Configure Angle**:根据当前Scene内的坐标,自动调节物体起始的角度
	- **Angle**:自行设置并固定物体起始时的角度
	- **Use Motor**:是否使用马达.若使用,则可自动滑动到终点
		- **Motor Speed**:滑动速度,单位同上.可正可负
		- **Maximum Motor Force**:最大扭矩值,含义同上
	- **Use Limits**:是否使用距离限制,限制滑块两端的距离.也就是说,Connected Anchor锚点始终在Lower Translation和Upper Translation两端内,就在这样一个范围内进行滑动.
- **实现用途:**
	- 自动或者手动的滑块(机关)

![](https://img2018.cnblogs.com/blog/1688704/201909/1688704-20190912111144366-755821371.gif)

## Wheel Joint 2D 车轮关节
- **基本功能:**
	- 模拟实现车轮的运动状态,即车轮以车轴为圆心旋转
- **使用方法:** 
	- **Enable Collision**:被关节连接的两个物体是否能相互碰撞
	- **Connected Rigid Body**:定义车轮的锚点到车轴上
	- **Auto Configure Connected**:自动配置车轮锚点与世界空间中的锚点匹配
	- **Anchor**:车轮锚点的坐标
	- **Connected Anchor**:车轴锚点的坐标
	- **Suspension**:悬架,用于配置车轮震动效果等
		- **Damping Ratio**:阻尼比,值范围[0,1]
		- **Frequency**:震动频率,单位Hz,值范围[0,1000000],此处建议(0,10]
		- **Angle**:可以调节车轮的角度,但尚未知晓具体作用
	- **Use Motor**:是否使用马达.若使用,则可实现自动旋转
		- **Motor Speed**:转动速度,单位同上.可正可负
		- **Maximum Motor Force**:最大扭矩值,含义同上
- **实现用途:**
	- 汽车车轮
	- 不规则自转
	- 某些旋转特效
- **备注**:在demo2场景下还有一个Wheel Joint2D的实例,详细配置过程不再赘述

![](https://img2018.cnblogs.com/blog/1688704/201909/1688704-20190912111607802-589332371.gif)
![](https://img2018.cnblogs.com/blog/1688704/201909/1688704-20190912111227695-1373695724.gif)

--------------

# 参考
- [Physics Joints in Unity2D - raywenderlich.com](https://www.raywenderlich.com/1766-physics-joints-in-unity-2d)
- [Unity2D 中的物理关节 - Rickshao1993](https://blog.csdn.net/rickshaozhiheng/article/details/78509632)