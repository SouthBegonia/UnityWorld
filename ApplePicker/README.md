# 捡苹果 Apple Picker

## 项目展示
![](https://img2018.cnblogs.com/blog/1688704/201906/1688704-20190607131214137-1515753363.gif)

Github项目地址：[Apple Picker](https://github.com/SouthBegonia/UnityWorld/tree/master/ApplePicker)

## 涉及知识

- 正投视图
- 3D场景内树与苹果的图层
- 记录最高分到本地

## 准备工作

**模型制作：**
1. 基本模型创建
	- 树叶：sphere 拉伸为椭圆形，绿色材质球
	- 树干：cylinde 修改为合适尺寸，棕色材质球
	- 苹果：sphere 附加深红金属光泽球
	- 篮筐：cube 修改为合适尺寸，黄色材质球
2. 位置、层级关系
	- 调整树叶树干的位置搭配，呈现简单树木形态
	- 另设的空物体`AppleTree`包含着树叶和树干
3. 标签、图层
	- 新建标签Apple标记苹果物体
	- 新建图层 AppleTree、Apple和Basket，后在Edit->Project Setting->Physics内取消Apple与AppleTree图层的碰撞。后修正对应物体图层
4. 相机及预制体
	- 调整主摄像机为正投影(Orthographic)，移动到合适位置
	- 创建苹果的预制体

## 游戏逻辑

- 苹果树：
	- 每帧都以一定的速度移动，当碰到左右边界则转向，且在左右移动过程中有概率改变运动方向
	- 每一定时间间隔落下一个苹果
- 苹果：
	- 从树叶位置实例化并落下(本例采用rigidbody重力)
	- 与篮筐产生碰撞即被销毁
	- 若篮筐未与其碰撞，则当苹果下落到一定距离就自动销毁
- 篮筐：
	- 开局有3个篮筐，且均实例化出来
	- 篮筐跟随鼠标限制在界面的x轴方向移动
	- 篮筐用于接住苹果，与苹果碰撞后销毁苹果
	- 当漏接苹果(也就是当苹果下降到自行销毁的底线时)，罗筐数减少(相当于玩家的生命值)
- 机制：
	- 开局，苹果树左右移动，生成苹果下落
	- 玩家操作鼠标使篮筐左右移动
	- 接到苹果时得分增加，超过历史分值就刷新记录
	- 当没接到苹果时，篮筐数减少(生命值减少)，且场景内还在下落中的苹果消失
	- 当3个篮筐都没了，游戏结束，几秒后自动重新开局

## 代码相关

- AppleTree.cs：绑定于苹果树AppleTree
- Apple.cs：绑定于苹果Apple
- Basket.cs：绑定于篮筐Basket
- ApplePicker.cs：绑定于主摄像机；BasketPrefab 挂载Basket，EndUI 挂载Canvas->End
- HighScore.cs：绑定于 Canvas->HighScore

## 问题探讨

**苹果树移动时概率转向问题：**
- 问题分析：在AppleTree.cs内，原计划实现每秒转向概率为`chanceToChangeDirecyions=0.02`(如下原版代码)，但是发现，在FixedUpdate()内，每秒执行50次，也就会导致AppleTree平均每秒改变一次方向：(设概率为t) *50 x (t/1) = 1* ，因此需要修改。
- 解决方案：对随机数Random.value进行修改，即 Random.value/Time.deltaTime，此时每秒转向概率为： *50 x (t / (1/0.02)) = 50 x 0.02(t/1) = 0.02* ，至此解决问题
- 其他：但试验下来0.02概率过低，我已调为0.4，且速度为5
```
private void FixedUpdate()
{
	/*-----原版-----*/
    //随机改变运动方向，概率为 chanceToChangeDirecyions
    if ((Random.value) < chanceToChangeDirecyions)
        speed *= -1;

	/*-----修正版-----*/
    if ((Random.value / Time.deltaTime) < chanceToChangeDirecyions)
        speed *= -1;
}
```

**苹果下落被销毁就不再生成问题：**
- 问题分析：当第二个苹果还未实例化，第一个实例化的苹果就因下降越线被销毁或者被篮筐接住销毁，此刻场景内就再无Apple预制体，也就是说，AppleTree脚本 就失去对象applePrefab，无法再实例化产生苹果
- 解决方案：围绕保证场景内任意时刻存在一个applePrefab
	- 调高树的高度(本例方案)
	- 缩短实例化苹果的间隔
	- 先隐藏再延时销毁苹果(理论可行，但感觉大材小用)



## 参考

- 《游戏设计、原型于开发》 - Jeremy Gibson
- [unity如何调用另一个脚本中的变量](https://blog.csdn.net/liumou111/article/details/46754051)
- [Unity5.x的GUIText被UI Text所取代](https://blog.csdn.net/u014800094/article/details/52329809)