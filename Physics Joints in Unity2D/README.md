# Unity2D中的物理关节
- **本文概述:**分析Unity中几个**2D物理关节组件**的基本功能、使用方法、运用场景等
- **开发环境:**Unity2019.3.0a2 / VS2017
- **项目资源包:**[2D Joints Starter](https://www.raywenderlich.com/1766-physics-joints-in-unity-2d)
- **说明:**较比于源项目,我自行做了如下设置
	- 对Unity新版本下的新参数进行解释
	- 简单做了新的UI
	- 对各脚本的注释及修改
	- 对场景下物体有序归类: 将各组件的示例物体归于对应组件名称的空物体下,如涉及DistanecJoint2D组件的物体存放于Distance Joint 2D物体下等

## Distance Joint 2D 距离关节
- **基本功能:**
	- 使两物体被距离关节限制,保持一定距离
	- 一个物体可对另一个物体做基于物理特性的环绕运动(环绕物体与被环绕物体)
- **使用方法:** 
	- **Enable Collision**:被关节连接的两个物体是否能相互碰撞
	- **Connected Rigid Body**:定义被环绕物体的锚点到指定物体上
	- **Auto Configure Connected**:自动配置锚点与世界空间中的锚点匹配(暂没用过,但我相信点了你会Ctrl+Z的)
	- **Anchor**:环绕物体锚点的坐标
	- **ConnectedAnchor**:被环绕物体锚点的坐标
	- **Auto Configure Distance**:自动计算并设置两物体的距离,即在运行时拖拽环绕物体则会改变Distance,若关闭则Distance始终为预设值
	- **Distance**:两物体的距离
	- **Max Distance Only**:使得物体摆动到较高位置时会沿关节连线方向下沉稍许距离.若关闭,物体在环绕运动中的距离始终为Distance
- **实现用途:**
	- 摆锤

## Spring Joint 2D 弹簧关节
- **基本功能:**
	- 使两物体被弹簧关节限制,保持一定范围距离
	- 一个物体可对另一个物体做基于物理特性的弹簧拉伸压缩运动(副物体与主物体)
- **使用方法:** 
	- **Enable Collision**:被关节连接的两个物体是否能相互碰撞
	- **Connected Rigid Body**:定义主物体的锚点到指定物体上
	- **Auto Configure Connected**:自动配置锚点与世界空间中的锚点匹配
	- **Anchor**:副物体锚点的坐标
	- **ConnectedAnchor**:主物体锚点的坐标
	- **Auto Configure Distance**:自动计算并设置两物体的距离,即在运行时拖拽环绕物体则会改变Distance,若关闭则Distance始终为预设值
	- **Distance**:两物体的距离
	- **Damping Ratio**:阻尼系数,值范围[0,1]
	- **Frequency**:震动频率,单位Hz,值范围[0,1000000],但个人认为[0,10]较为常用
- **实现用途:**
	- 弹簧
	- 弹射器