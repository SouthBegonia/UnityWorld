本文详细分析了AnimatorController中动画切换过渡问题，即Translation过渡及hasExitTime的问题。方法为对实际项目中的所有情况进行分类，规划逻辑图，可视化分析解决这些问题。
- 博客园：[Unity HasExitTime用法 - SouthBegonia](https://www.cnblogs.com/SouthBegonia/p/11748429.html)
- 项目地址：[Hero - SouthBegonia](https://github.com/SouthBegonia/UnityWorld/tree/master/Hero)

---------

## AnimatorController 动画机控制器
- **功能**：对已有的Animation片进行逻辑连接，连接过渡方式为**Translation有向线段**，其切换方法包括**启用hasExitTime**和**关闭hasExitTime**
- **Translation**：通过设置Parameters的参数进行切换过渡，可单线上有多条件，也可单方向上有多单条件线。选中任意Translation可在Inspecter窗口看到两个动画片段和区间的窗口，及Setting参数表
- **hasExitTime**：是否有退出时间。简单理解：开启表示等待当前动画进行完才可进行下一个动画；关闭表示可以立即打断当前动画并播放下一个动画

----------------

## 项目实战
- **背景**：存在idle、idle_2、attack_1、attack_2、attack_3动画片段实现三连击的攻击效果，根据attack值切换。我们仅关注几个attack的切换，其中
	- **attack_1->attack_2：关闭hasExitTime，切换条件attack=2，Translation未超**
	- **attack_2->attack_3：开启hasExitTime，切换条件attack=3，Translation未超**
    - **attack_1、attack_2、attack_3切换到idle_2：切换条件attack=0，代码控制判断任意动画播放完毕则attack=0**

AnimatorController：
![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191027180036461-786016180.png)

attack_1->attack_2（左）、attack_2->attack_3（右）的Translation：
![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191027182108495-1054387769.png)


### 1. 关闭 has exit time

关闭has Exit Time：无退出等待时间，立即开启下一动画
- 情况一：当attack_1正常运转，在**左标记前**达成转行条件
	- 立即开启Translation（而不是等待运行到左标记）和attack_2，三者并行运转
		- 若Translation还在运行中，attack_1却先运行完，代表当前动画片attack_1运行完（代码：`(stateInfo.IsName("attack_1") && stateInfo.normalizedTime > 1)) == true` ），会对逻辑上的判断造成影响，这种情况非常容易出错
		- 若attack_1还在运行，Translation却先运行完，则立即停止attack_1片段。逻辑正常
	- 此后整体动画全权由attack_2运转
- 情况二：当attack_1正常运转，在**左边标记后**（左右标记中或是右标记后）达成转换条件
	- 立即开启Translation和attack_2，三者并行运转
		- 若Translation还在运行中，attack_1却先运行完，代表当前动画片attack_2运行完，结果同上
		- 若attack_1还在运行中，Translation却先运行完，则立即停止attack_2片段。逻辑正常
	- 此后整体动画全权由attack_2运转

逻辑图：（修正：count=0 表示attack=0）
![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191027180154319-163912164.png)

项目实例图：左标前达成（先），左标后达成且Translation后完成（后）
![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191027180312554-640103723.gif)
![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191027180316681-1151924338.gif)


### 2. 开启 has exit time

当has exit time：有退出等待时间，需等待目前动画完成到一定阶段才可切换至下一动画。过程：
- 情况一：attack_2正常运转，若在左标记前达成转换条件
	- 等待attack_2中动画片进行到左标记，开启Translation过渡和attack_3动画，，三者动画并行运转
		- 若Translation还在运行中，attack_2却先运行完，代表当前动画片attack_2运行完（代码：`(stateInfo.IsName("attack_1") && stateInfo.normalizedTime > 1)) == true` ），会对逻辑上的判断造成影响，这种情况非常容易出错
		- 若attack_2还在运行，Translation却先运行完，则立即停止attack_2片段。逻辑正常
	- 此时整体动画全权由attack_3运转；
- 情况二：attack_2正常运转，若在左标记后达成条件
	- 无论是在标记期间或是右标记后达成条件，再也无法触发Translation转换，因为可过渡时间片已经错过了

逻辑图：（修正：count=0 表示attack=0）
![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191027180337291-1432920725.png)

项目实例图：左标前达成（先），左标后达成（后）
![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191027180720415-724195617.gif)
![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191027180753152-653391457.gif)

----------------
## 总结

### Translation内左右标记问题：
- **左右标记范围含义**：
	- Translation时长，两个动画的过渡时间长度
- **左右标记位置含义**：
	- 对于当前Animation：左标记表示开启Translation和下一Animation的时间点；右标记表示当前动画片所能进行到的最晚时间点
	- 对于下一Animation：左标记为当前动画开启时间点，右标记为当前动画全权运转时间点
	- 通俗解释：**何时转换、转换多久、转换时两动画所占比例**
- **区间意义**：
	- 例如attack_1和attack_2过渡的时间片段，过渡期的动画由Unity根据attack_1和attack_2动画混合而成，因此**所谓三动画并行执行，本质为执行Translation的混合动画**。其外需要注意：**过渡期间仍处于attack_1**，代码就是：`stateInfo.IsName("attack_1")==true`，仅当Translation结束才算处于attack_2

### Translation后于Animation错误问题：
该错误直观上看就是两动画和Translation时间先后问题，数字上看就是Translation首尾点、区间大小、条件达成时机。因此解决办法就围绕这几点，此处给出几点建议：
- Translation右标记最好 <= 前Animation末端，也就是说**Translation不要超出动画长度**
- **最好在左标记点前达成转换条件**，以便我们对转换过程进行掌控
- AnimatorController的设置不是唯一途径，**代码操控、动画内插入事件**等，都可以较好辅助我们实现安全的动画切换

![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191027180829999-734068984.png)


## 参考
- [Unity5.X 从入门到精通 - UnityTechnologies]()
- [Unity2D实现人物三连击 - 逐影](https://www.cnblogs.com/undefined000/p/unity2D-combo-attack.html)
- [Unity3d基础:Animator动画三连击 - 考班格](https://blog.csdn.net/QQhelphelp/article/details/89517362)
- [Unity游戏动画 从入门到住院:动画机状态 - GameRes](https://www.gameres.com/680484.html)