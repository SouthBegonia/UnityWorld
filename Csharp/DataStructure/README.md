## Array数组

**1.特性:**
- 在内存上连续分布
- 元素都为同类型或者类型的衍生类型
- 创建时必须指定长度
- 通过下标访问

**2.机理:**
创建数组时,在**托管堆**中分配一块连续的内存空间,长度为size

**3.优缺点**
- 连续存储使得插入和删除不方便
- 创建即声明长度,可能存在浪费或者溢出

**4.示例:**
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

**1.特性:**
- **元素可为不同类型**,因为全都当做Object来处理
- 创建时不必须指定长度
- 数组长度动态增减

**2.机理:**
创建ArrayList时,需要引入 `System.Collections` ,将所有元素作为 `Obejct类` 来处理,实现了储存不同类型的数据

**3.优缺点**
- ArrayList是类型不安全的
- 插入值类型时发生装箱操作;索取元素时发生拆箱操作,产生额外开销


**4.示例:**
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

**1.特性:**
- **元素为指定类型**
- 创建时无须指定长度
- 数组长度动态增减

**2.机理:**
`List<T>`可理解为ArrayList类的泛型等效类,融合了Array和ArrayList的优点,较为常用

**3.优缺点**
- `List<T>`是类型安全的
- 无装箱拆箱操作,引入泛型无需运行时类型检查,高效
- 既有Array快速访问优点,又有ArrayList长度灵活变化性

**4.示例:**
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