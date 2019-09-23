## Array数组

**特性:**
- 在内存上连续分布
- 元素都为同类型或者类型的衍生类型
- 创建时必须指定长度
- 通过下标访问

**机理:**
创建数组时,在**托管堆**中分配一块连续的内存空间,长度为size

**优缺点**
- 连续存储使得插入和删除不方便
- 创建即声明长度,可能存在浪费或者溢出

**示例:**
```
//声明
int[] array1 = new int[5];
int[] array2 = new int[] {1,2,3,4,5};
int[] array3 = {1,2,3,4,5};
int[,] array4 = new int[1,2];
int[,] array5 = { {1,2}, {3,4} };

int[][] array6 = new int[4][];
aarray6[0] = new int[2] {1,2};

//赋值,修改
string[] s = new string[2];
s[0] = "Hello ";
s[1] = "World!";
s[1] = "Hi!";
```

## ArrayList数组

**特性:**
- **元素可为不同类型**,因为全都当做Object来处理
- 创建时不必须指定长度
- 数组长度动态增减

**机理:**
创建ArrayList时,需要引入 `System.Collections` ,将所有元素作为 `Obejct类` 来处理,实现了储存不同类型的数据

**优缺点**
- ArrayList是类型不安全的
- 插入值类型时发生装箱操作;索取元素时发生拆箱操作,产生额外开销


**示例:**
```
//声明
ArrayList arrList = new ArrayList();

//添加元素
arrList.Add("South");
arrList.Add("Begonia");
arrList.Add(9);

//修改元素
arrList[2] = 12;

//删除元素
arrList.RemoveAt(2);
```

## List`<T>`数组

**特性:**
- **元素为指定类型**
- 创建时无须指定长度
- 数组长度动态增减

**机理:**
`List<T>`可理解为ArrayList类的泛型等效类,融合了Array和ArrayList的优点,较为常用

**优缺点**
- `List<T>`是类型安全的
- 无装箱拆箱操作,引入泛型无需运行时类型检查,高效
- 既有Array快速访问优点,又有ArrayList长度灵活变化性

**示例:**
```
//声明
List<string> list = new List<string>();

//添加元素
list.Add("South");
list.Add("Begonia");
list.Add("wow");

//修改元素
list[2] = "!";

//删除元素
list.RemoveAt(2);
```


## LinkedList`<T>`链表

**机理:**
同C/C++中的链表构造,采用各对象的指针进行结点访问.但在C#中已经封装成为链表的类:`LinkedList<T>`,及结点的类:`LinkedListNode<T>`.注意`LinkedList<T>`链表为双向链表

**优缺点**
- 双向列表
- 进行插入和删除方便

**示例:**
```
//创建链表
string[] str = {"H","E","L","L","O"};
LinkedList<string> sentence = new LinkedList<string>(str);

//插入结点
sentence.AddFirst("*");
sentence.AddLast("*");

//删除结点
sentence.RemoveFirst();
sentence.RemoveLast();

/查找并插入结点
LinkedListNode<string> current = sentence.Find("H");
sentence.AddBefore(current,"G");
sentence.AddAfter(current,"J");

//访问前后结点
current = current.Next;
current = current.Previous;

//倒序链表
sentence.Reverse();

```

## Queue`<T>` 队列

**特性:**
- 前进先出,FIFO
- 可动态调节长度(但扩展是重新分配数组空间)
- 默认下初始容量32

**机理:**
`Queue<T>`由类型为T的对象的环形数组组成,head标记队首,tail标记队尾.


**示例:**
```
//声明
Queue<string> numbers = new Queue<string>();

//元素入队
numbers.Enqueue("H");
numbers.Enqueue("i");

//元素出队
Debug.log("出列: {0}",numbers.Dequeue());

//复制队列
Queue<string> queueCopy = new Queue<string>(numbers.ToArray());
```

## Stack`<T>` 栈

**特性:**
- 后进后出LILO
- 扩展也是重新分配数组空间

**机理:**
`Stack<T>`由类型为T的对象的数组组成,具备入栈Push和出栈Pop操作


**示例:**
```
//声明
Stack<string> numbers = new Stack<string>();

//元素入栈
numbers.Push("Z");
numbers.Push("W");

//元素出栈
Debug.log("出栈: {0}",numbers.Pop());

//复制栈
Stack<string> stackCopy = new Stack<string>(numbers.ToArray());
```

## Hash Table 哈希表

**特性:**
- 散列表
- 根据关键码/值直接进行访问

**机理:**
通过吧关键码/值映射到表中的一个位置来进行访问,进而替代Array数组的遍历查找特性.

**一些概念:**
- 哈希转换(Hasing):又称为哈希函数压缩序列.即为减少关键码/值得区间跨度,对其进行特征片段截取的方法.
- 哈希冲突(Hash Collision):即通过哈希计算后,得到同一个结果,进而导致元素入表异常的情况.
	- 出现原因:哈希转换
	- 解决方法:
		- 冲突避免机制(Collision Avoidance):优化哈希函数,尽量选择数据非均匀分布的片段作为关键码/值
		- 冲突解决机制(Collision Resolution):将哈希位置被占用的待插入元素存放到另一块空间. 常用的方法有开放寻址法(Open Addressing)、二次探查(Quadratic Probing)及二度哈希(double hashing)
- C#中的Hashtable类:定义在System.Cdlections命名空间.添加元素时需给定元素Item及其键Key,进而通过Key检索Item.

**示例:**
```
//创建哈希表
Hashtable itemDic = new Hashtable();

//向实例内添加数据,格式 hashtable.Add(Key,Item)
itemDic.Add("11-1234","Item1");
itemDic.Add("12-1234","Item2");
itemDic.Add("13-1237","Item3");
itemDic.Add("14-1235","Item4");
itemDic.Add("15-1235","Item5");

//通过Key访问数据
if(itemDic.ContainsKey("11-1234"))
{
	string itemName = (string)itemDic["11-1234"];
	Debug.log("编号为11-1234的物品是: " + itemName);
}else
	Debug.log("未找到编号为11-1234的物品");
```