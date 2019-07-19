# 目录
* [游戏原型](#1)
* [项目演示](#2)
* [绘图资源](#3)
* [代码实现](#4)
* [注意事项](#5)
* [技术探讨](#6)
* [参考来源](#7)

-----------

<h1 id="1">游戏原型</h1>
**爆破任务**是一款核心机制类似于愤怒的小鸟的游戏，玩家将**用弹弓发射炮弹**，摧毁城堡，最终目标是**让发射的炮弹抵达城堡中心的目标区域**。我们所希望实现的有：
1. 当玩家鼠标光标处于弹弓区域内时，弹弓高亮，表示此时可以进行射击操作。
2. 当玩家在该区域内按下左键，会实例化弹丸。玩家持续按住左键并且在一定范围内移动光标，实现不同角度拉伸弹弓。
3. 当玩家松开左键时，弹弓将弹丸弹射出去，并显示弹丸运动的轨迹。
4. 不同关卡有不同的城堡样式，但我们的目的是一致的：摧毁城堡，让发射中的炮弹抵达城堡核心区域，否则炮弹将熄灭，无法摧毁目标。


------------------------

<h1 id="2">项目演示</h1>
![](https://img2018.cnblogs.com/blog/1688704/201907/1688704-20190713151333154-1164345528.gif)

Github项目地址:[MissionDemolition 爆破任务](https://github.com/SouthBegonia/UnityWorld/tree/master/MissionDemolition/)
试玩下载：[MissionDemolition 爆破任务 提取码3wq7](https://pan.baidu.com/s/1vRktj0c0ALS9F_WvfE1EhA)
--------------------

<h1 id="3">绘图资源</h1>
- **地面**：用极长条状Cube作为地面，赋予地面材质
- **摄像机**：设置MainCamera的P[0,0,-10]，投影方式Projection为Orthographic，Size为10，可另行修改天空盒
- **弹弓绘制**：用3个圆柱体成一定角度组合而成弹弓，添加弹弓材质，设置为触发器。最终3个部件存于Slingshot物体下，完成弹弓
- **弹丸绘制**：用Cube加刚体组件加暗色材质即可
- **云朵绘制**：用移除碰撞器的球体，Shader组件执行Legacy Shaders->self-Illiumin->Diffuse，为其添加漫射光，调整拾色器为偏向灰白色。创建多个同类物体，进行拉伸缩放调整，最终创建4个云朵预制体
- **城堡绘制**：用Cube加刚体组件，锁Z轴旋转，给城墙添加材质或物理材质，最终创建多个城堡作为预制体；还得添加城堡核心Goal，用其他颜色得Cube填充，设定为触发器，设为预制体
- **ProjectileLine物体**：仅添加Line Renderer组件、Material及后续ProjectileLine.cs脚本添加尾拖
- **CloudAnchor物体**：不添加其他组件，用于后续CloudCrafter.cs脚本添加云朵
- **UI**：Canvas内的Text有3个，包括：得分GT_Level，当前关卡GT_Score，胜利页面GT_Win


--------------------

<h1 id="4">代码实现</h1>

|脚本名称|实现功能|
|----|----|
|Slingshot.cs|挂载于Slingshot，实现弹弓激活时的高光、弹丸从实例化到发射、验证弹丸触及目标区域的合法性|
|FollowCam.cs|挂载于MainCamera，实现相机平滑跟踪发射出去的弹丸，在弹丸静止或者一定时限后回视角到弹弓|
|Goal.cs|挂载于Castle内的Goal，验证炮弹触及此目标区域的合法性，若验证成功则改变其颜色表示通过此关卡|
|ProjectileLine.cs|挂载于ProjectileLine物体，用于配置尾拖参数，实现尾拖特效|
|CloudCrafter.cs|挂载于CloudCrafter物体，实现不同类型云朵的实例化，并赋予其运动效果|
|MissionDemolition.cs|挂载于MainCamera，实现关卡切换，游戏判定，UI更新，添加顶端button实现视角切换|

--------------------

<h1 id="5">注意事项</h1>
- **Orthographic正交投影相机**：其Size是指摄像机视野中心到底部或者顶部的距离，即Size是摄像机视野高度的一半
- **IsKinematic运动学刚体**：刚体的运动不会受到碰撞和重力的影响，但仍会影响其他非运动学的刚体。本例中处于拉伸瞄准状态的弹丸即是运动学刚体，当弹丸发射出去后立即改变状态为非运动学刚体，受到重力影响下坠
- **UI自适应**：设置Canvas内Canvas Scaler组件上UI Scale Mode为Scale With Screen Size，实现不同分辨率下UI大小自适应


-------------

<h1 id="6">技术探讨</h1>
1. **城堡不稳定性**：
	- 问题描述：城堡堆到到一定层次后，会产生滑移，最终还未被弹丸碰撞即自行倒下
	- 问题分析：首先可能为结构受力问题，头重脚轻；其次可能为物理材质内摩擦力的问题
	- 解决方案：目前稍微可行的方案有：调整城堡堆垛结构，遵循三角形结构，底部材料可加重，顶部材料可减轻；其次设置物理材质，增大动静摩擦力，阻碍滑移(当复杂城堡仍未较好解决)
2. **相机平滑跟踪**：
	- 问题描述：若只是单纯的将相机的position跟随弹丸的position，显得画面机械化，粗糙化
	- 问题分析：position简单跟随无法实现平滑视角
	- 解决方案：采用插值法，即 `destination = Vector3.Lerp(transform.position, destination, easing);` ；`Vector3.Lerp()` 返回两点之间的一个线性插值位置，取两点位置的加权平均值,当 easing=0 时，返回 transform.position；当 easing=0.05时，表示让相机从 **当前位置**向 **目的地位置** 移动，每帧靠近5%的距离。概念描述：设初始时当前位置p，目的地位置q，两点间距 d1=q-p；第二帧时，d2=(1-0.05)d1；第三帧时，d3=(1-0.05)d2，...，由此下去，相机每帧都会更靠近目标地点一定距离，由此，我们可以看到平滑跟随的视角

![](https://img2018.cnblogs.com/blog/1688704/201907/1688704-20190713151455738-902762440.gif)

3. **相机视角缩放**：
	- 问题描述：当弹丸发射到较高高度后，此时我们只能看到弹丸在天空中缓慢移动，无法直观判断快慢
	- 问题分析：原本设置得摄像机正交视角Size=4，当弹丸过高时，地面脱离出视角
	- 解决方案：首先限制相机目的地destination得x,y坐标，限制其不移动到弹弓左侧或是地面以下，即x,y的值不会为负： `destination.x = Mathf.Max(minXY.x, destination.x);  destination.y = Mathf.Max(minXY.x, destination.y);` 。其次，设置相机Size的大小：`GetComponent<Camera>().orthographicSize = destination.y + 10;`，因为初始时Size的大小即为10,destination.y 由于上面代码的限制初始时为0，故无论初始时或者落地时，Size最小为10，且能随着弹丸飞高而扩大Size

![](https://img2018.cnblogs.com/blog/1688704/201907/1688704-20190713151516092-930025080.gif)

--------

<h1 id="7">参考来源</h1>
- 《游戏设计、原型与开发》 - Jeremy Gibson
- [unity 打包时UI的自适应](https://blog.csdn.net/qq_42419143/article/details/90776390)
- [Unity 项目发布后屏幕自适应问题](https://blog.csdn.net/Skying_/article/details/79704524)
- [Unity - 物理材质](https://blog.csdn.net/qq_21397217/article/details/80277857)
- [unity5 Orthographic模式相机视截体尺寸计算](https://www.cnblogs.com/wantnon/p/4365538.html)