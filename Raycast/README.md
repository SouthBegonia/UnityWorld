本文简要分析了Unity中**射线检测**的基本原理及用法，包括：
- Ray 射线
- RaycastHit 光线投射碰撞信息
- Raycast 光线投射
- SphereCast 球体投射
- OverlapSphere 相交球等

**博客园**：[Unity - Raycast](https://www.cnblogs.com/SouthBegonia/p/11732340.html)
**项目地址**：[Raycast - SouthBegonia](https://github.com/SouthBegonia/UnityWorld/tree/master/Raycast)

![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191024150900466-224325821.gif)
![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191024150906994-693623880.gif)

------

# Ray 射线
- 含义：官方解释为一条无穷的线，开始于origin点，朝向direction方向（但是，根据项目验证来看其默认长度为单位向量，只有对direction进行乘以倍率，才可实现延长射线，而非无穷）
- 用法：
	- `Ray ray = new Ray(transform.position,transform.forward)`：从物体中心创建一条指向前方的射线ray
	- `Ray camerRay = Camera.main.ScreenPointToRay(Input.mousePosition)`：产生一条从摄像机产生、经过屏幕上光标的射线。当相机为perspective模式，射线在相机梯形视野内发散；若为orthoGraphic，则为垂直与相机面的直线段（见上图）

------

# RaycastHit 光线投射碰撞信息
- 含义：取得从Raycast函数中得到的碰撞信息（注意不是collider哈，是包含collider信息）
- 关键变量：point、collider、rigidbody、transform

------

# 检测方法 - 线型检测

### Physics.Raycast 光线投射
- 功能：在已有一条射线（也可无）的基础上，使用射线（新建射线）进行一定距离内的定向检测。可修改射线长度，限制其检测的Layer层，并且可以得到射线检测到的碰撞信息。但仅能检测到**第一个被射线碰撞的物体**，后面的物体无法被检测到
- 用法：
	- `Raycast(transform.position, Vector.forward, distance, LayerMask.GetMask("Enemy"))`： 从物体中心点起，朝向Vector3.forward方向发射一条射线，该射线长度为distance，射线可检测到的层为Enemy层，返回bool类型
	- `Raycast(transform.position, Vector.forward, distance, out RaycastHitInformation ,LayerMask.GetMask("Enemy")）`：从物体中心点起，朝向Vector3.forward方向发射一条射线，该射线长度为distance，将碰撞信息反馈到RaycastHitInformation上，射线可检测到的层为Enemy层，返回bool类型
	- `Raycast (MyRay, distance, LayerMash.GetMask("Enemy"))`：从已有的射线MyRay出发，长度延伸至distance，射线可检测到的层为Enemy层，返回bool类型
	- `Raycast (MyRay, out RaycastHitInformation, distance, LayerMask.GetMask("Enemy"))`
- 适用场合：配合相机坐标转换实现各类交互

### Physics.RaycastAll 所有光线投射
- 功能：机理用法大致同Raycast，当区别在于可检测射线路径上的**所有物体**，返回检测信息的数组。其他带All后缀的方法也同理
- 用法：
	- `RaycastHit[] hits = RaycastAll(Vector3.zero, Vector.forward, distance, LayerMask.GetMask("Enemy"))` 
	- `RaycastHit[] hits =  RaycastAll(MyRay, distance, LayerMash.GetMask("Enemy"))`
- 适用场合：穿透性检测


### Physics.Linecast 线段投射
- 功能：建立**某两点之间**的射线进行检测，返回bool类型
- 用法：
	- `Linecast(startPos, endPos, LayerMask.GetMask("Enemy"))`
	- `Linecast(startPos, endPos, out RaycastHit, LayerMask.GetMask("Enemy"))` 
- 适用场合：特定地点局部距离射线检测

------

# 检测方法 - 体型检测

### Physics.SphereCast 球体投射
- 功能：扩展检测范围为球形，返回bool类型。但是该投射用法需要万分小心，见下
- 用法：
	- `SphereCast (originPos, radius, direction, out RaycastHit, distance, LayerMask.GetMask("Enemy"))`：含义是在originPos点创建半径为radius的球体；以朝向direction方向的球面为起始面（另一面舍弃），移动distance距离，期间半球面经过的区域即为检测区域。那么originPos到originPos+radius内的半球区域呢？答案是舍弃，用官方的话来说，是边界而不是包围体。(立体结构：以左右球球心为轴线，建立半径为radius、高为distance的圆柱体，左球挖去右半体积，右球添加右半体积）
	- `SphereCast (Ray, radius, out RaycastHit, distance, LayerMask.GetMask("Enemy"))`
- 适用场合：检测目的地是否可抵达，从而判断可移动性


### Physics.Boxcast 块体投射/ CapsuleCast 胶囊体投射
- 功能：同球体投射
- 用法：同球体投射

### Physics.OverlapSphere 相交球
- 功能：同样检测球形，但与SphereCast存在较大区别
- 用法：
	- `Collider[] hits = Physics.OverlapSphere(startPos, radius, LayerMask.GetMask("Enemy"))`：以startPos为原点，创建半径为radius的球形，检测区域为整个球形包围体（实心），检测Enemy层上的物体，返回所有碰撞物体的collider而不是RaycastHit
- 适用场合：检测挂载物体球形范围内是否存在碰撞，常用方法

### Physics.IgnoreCollision 忽略碰撞
- 功能：屏蔽两个collider的碰撞，第三个参数为bool
- 用法： `IgnoreCollision (collider1, collider2, ignore)`

-----

# 参考
- [Physics.Raycast - UnityDocumentation](https://docs.unity3d.com/2018.3/Documentation/ScriptReference/Physics.Raycast.html)
- [Unity常用射线检测方法 - 昵称好难写](https://blog.csdn.net/qq_36274965/article/details/79456449)
- [Unity的学习笔记(射线检测) - 小鸟游](https://www.cnblogs.com/takanashi/p/11028786.html)
- [Unity3DAPI射线检测 - 码迷](http://www.mamicode.com/info-detail-2514998.html)