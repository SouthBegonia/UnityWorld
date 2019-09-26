# TileMap的注意事项

本文记述了一些在使用Tilemap绘制场景时的需要注意的细节问题。
关于Tilemap的创建及使用本文不做说明，但推荐佳作：[Unity中使用Tilemap快速创建2D游戏世界 - feng](https://gameinstitute.qq.com/community/detail/121256)
本文项目地址：[Tilemap - SouthBegonia](https://github.com/SouthBegonia/UnityWorld/tree/master/Tilemap)

## Q1：瓦片匹配问题

- **发生情景：**在创建Tilemap及一个palette后，我们想把已有的美术资源(Jungle_Tileset.png)做成瓦片，在对图片进行切片、拖入Palette后、进行绘制地图时会发现，**瓦片并不与Scene场景内的unit单元格匹配**,例如下图:

![](https://img2018.cnblogs.com/blog/1688704/201909/1688704-20190926162210739-1102159657.png)

- **问题原因：**原因在于我们对图片进行切片时，**Pixels Per Unit** 数值的问题。
- **问题分析：**它的含义是**每个unit单元格所能容纳该图片的多少个像素**。上图依次为Pixels Per Unit为不同值时每个瓦片与Scene场景下一个Unit的比例。拿Pixels Per Unit=43来说，其含义是每个unit只够装下43个像素，而我们的美术图片（左）像素为1024x1024，算下来每个瓦片有128像素，我们却只给每个Unit43像素，所以从最右边图片我们可以看出大概9个unit才可以放下一个瓦片。
- **如何解决：**对每张将要被做成瓦片的美术资源进行Pixels Per Unit的计算。如本例最合理的数值为 1024/8 = 128
- 那么非瓦片的Sprite呢？：直接拖拽进入Scene进行缩放操作即可。

## Q2：Tiles的选择问题

- **发生情景：**在导入2d-extras包后，我们可以在Project内右键添加各类Tiles（见下图）。假如我们要做一个**带有Animation的瓦片**，是否直接可以直接使用AnimationTile类型呢？答案是肯定的，但完美的做法是按需使用。

![](https://img2018.cnblogs.com/blog/1688704/201909/1688704-20190926162233932-2092282446.png)

- **问题原因：**AnimatedTile的RuleTile的功能存在差异
	- **AnimatedTile：**
		- 瓦片数目：对单块瓦片进行操作
		- 动画速度：MinimumSpeed ~ MaximumSpeed
		- 起始时间：Start Time
	- **RuleTile** (前提是设置瓦片output类型为Animation)：
		- 瓦片数目：多片，且能同时实现规则瓦片功能
		- 动画速度：Speed
		- GameObject：同时实现PrefabBrush功能
- **总结：**对于带有Animation的瓦片，我们应该**按需选择Tile的类型**。例如：不同启用时间的地刺，我们应当选择AnimatedTile进而调节StartTime参数；例如较长的瀑布，我们应该选用RuleTile，既能实现Animation效果又能方便画多个瓦片；例如蜡烛，我们想实现燃烧动画不一致，就应当采用AnimatedTile进而调节两个Speed参数等等。但总的来说，**RuleTile集成了几类extra瓦片的基础功能**，较为常用。

## 快捷键

旋转绘制中的瓦片的方向：[ ] 键
删除当前绘制的瓦片： 按住shift+左键点击

## 参考

- [Unity中使用Tilemap快速创建2D游戏世界 - feng](https://gameinstitute.qq.com/community/detail/121256)
- [Unity Tilemap模块全攻略 - 北交点教育](https://www.bilibili.com/video/av46182904/?p=12)
- 2d-extra资源包：[2d-extras - Unity-Tecnologies](https://github.com/Unity-Technologies/2d-extras)