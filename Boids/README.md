1987年Craig W.Reynolds发表一篇名为《鸟群、牧群、鱼群：分布式行为模式》的论文，描述了一种非常简单的、以**面向对象**思维**模拟群体类行为**的方法，称之为 **Boids** ，Boids 采用了三个核心的规则：
- **排斥性**：避免与群体内邻近个体发生碰撞
- **同向性**：趋向与邻近的个体采用相同的速度方向
- **凝聚向心性**：向邻近个体的平均位置靠近

由此我们采用Unity来实现算法并演示，演示结果：
![](https://img2018.cnblogs.com/blog/1688704/201906/1688704-20190604200829027-1225145671.gif)

![](https://img2018.cnblogs.com/blog/1688704/201906/1688704-20190604200858310-1613840651.gif)


## 制作思路

每个boid对象，每帧都有2个关键的表：与该boid邻近的boids的表及与该boid最近的boids的表。根据两个表求得一些确定该boid位置、方向、速度的因素（例如其他boids的平均速度，其他boids的平均位置，该boid与其他boid的平均距离等），根据所提出的三规则，设置各影响因素的权重比例，最终所有影响因素加和成为确定的、该boid下一帧的方向、位置、速度。

## Boid模型的创建与配置

1. 创建空对象取名Boid，再创建其空对象子物体，取名Fuselage
	- Fuselage->position(0,0,0)，Rotation(7.5,0,0)，Scale(0.5,0.5,2)
2. 创建一个Cube作为Fuselage子物体，移除 Box Collider，并添加拖尾渲染器 TrailRenderer
	- Cube->position(0,0,0)，Rotation(45,0,45)，Scale(1,1,1)
	- Component->Effects->TrailRenderer，选择材质Defualt-Particle(Material)，Time值0.5，End Witdth值0.25
3. 复制Fuselage创建另一个名为Wing的组件添加到Boid，两个都属于Boid的子物体
4. 修改主相机位置到顶视图大范围
5. 将boid设为预制体，**boid** 模型制作完毕

## 脚本配置

- `BoidSpawner.cs` 绑定于主相机
- `Boid.cs` 绑定于预制体Boid上
- 主相机检视面板中，设定 `boidPrefab` 变量为预制体 **boid**

## 参考

《游戏设计、原型与开发》 - Jeremy Gibson