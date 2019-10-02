# Unity - 存读档机制简析

本文旨在于简要分析Unity中的两种存档机制，即：**PlayerPrefs数据持久化方法**及**Serialization数据序列化方法** 

较比与源项目，我另加了JSON方法、XML方法等及一些Unity设置，更便于读者在使用中理解Unity的存档机制。核心脚本为`Game.cs`
- 源项目地址：[How to Save and Load a Game in Unity - raywenderlich](https://www.raywenderlich.com/418-how-to-save-and-load-a-game-in-unity)
- 个人项目地址：[BattleSave - SouthBegonia](https://github.com/SouthBegonia/UnityWorld/tree/master/BattleSave)

![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191002093232581-2052765411.jpg)

![](https://img2018.cnblogs.com/blog/1688704/201910/1688704-20191002093241521-1652804985.jpg)

--------------

# 一、PlayerPrefs 数据持久化方法

1. 存储原理：采用键值对(key与value)的方法，将游戏数据储存到本地，是一种Unity自带的储存方法。
2. 储存类型：仅支持int、float、string三种
3. 储存地址：详见官方文档 [PlayerPrefs - Unity Documentation](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html)
4. 读写示例：

```
//项目内未展示该用法，但以下代码即为常规用法
//新建存档
PlayerPrefs.SetInt("Score", 20);
PlayerPrefs.SetFloat("Health", 100.0F);
PlayerPrefs.SetString("Name",m_PlayerName);

//检验存档信息
if(!PlayerPrefs.HasKey("Name"))
	return;

//读取存档
socre = PlayerPrefs.GetInt("Score");
health = PlayerPrefs.GetFloat("Health");
m_PlayerName = PlayerPrefs.GetString("Name");

//删除存档
PlayerPrefs.DeleteKey("Score");
```

- 优缺点：虽然以这种方式存储游戏数据方便快捷，但是当数据量庞大以后，键值对的大量创建使用，不仅脚本控制繁琐，也有可能造成资源的浪费。因此，只建议对一些基础数据，例如图像设置、声音设置等采用该方法存储。

# 二、Serialization 序列化方法

1. 存储原理：将对象(Object)转换为数据流(stream of bytes)，再经过文件流存储到本地的方法。
	- 对象(Object)：可以是Unity中的任何文件或是脚本
	- 数据流(stream of bytes)：
2. 序列化反序列化：
	- Serialization：对象-->数据流
	- Deserialization：数据流-->对象
3. 序列化的方法：
	- 二进制方法
	- JSON方法
	- XML方法

### 1. 二进制存储（Binary Formatter):
```
//存档信息的类：
[System.Serializable]
public class Save
{
    public int hits = 0;
    public int shots = 0;
    public List<int> livingTargetPositions = new List<int>();
    public List<int> livingTargetsTypes = new List<int>();
}

//设置游戏数值
public void SetGame(Save save)
{
    hits = save.hits;
    shots = save.shots;

    for (int i = 0; i < save.livingTargetPositions.Count; i++)
    {
        int position = save.livingTargetPositions[i];
        Target target = targets[position].GetComponent<Target>();
        target.ActivateRobot((RobotTypes)save.livingTargetsTypes[i]);
        target.GetComponent<Target>().ResetDeathTimer();
    }
}

//存档函数：
public void SaveGame()
{
	//1. 序列化过程
    //创建save对象保存游戏信息
    Save save = CreateSaveGameObject();
	string filePath = Application.dataPath + "/gameSaveBySerialize.save";

    //2. 创建二进制格式化程序及文件流
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(filePath);

    //3. 将save对象序列化到file流
    bf.Serialize(file, save);
    file.Close();
}

//读档函数：
public void LoadGame()
{
	string filePath = Application.dataPath + "/gameSaveBySerialize.save";

    //1. 检验目标位置是否有存档
    if (File.Exists(filePath))
    {
        //2. 创建二进制格式化程序，打开文件流
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);

		//3. 将file流反序列化到save对象		
        Save save = (Save)bf.Deserialize(file);
        file.Close();

		//从save对象读取信息到本地
		SetGame(save);
    }
    else  
        Debug.Log("No gamesaved!"); 
}
```

### 2. JSON方法：

```
/*
 * 注意：使用JSON存档方法需要用到LitJson库，LitJson.dll文件可在项目Assets目录下找到。
 * 使用方法：将LitJson.dll拖拽到个人项目Assets目录下即可
*/

//JSON存档函数：
public void SaveAsJson()
{	
	//1. 创建save对象保存游戏信息
    Save save = CreateSaveGameobject();
	string path = Application.dataPath + "/gameSaveByJson.json";

    //2. 利用JsonMapper将save对象转换为Json格式的字符串
    string saveJsonStr = JsonMapper.ToJson(save);

    //3. 创建StreamWriter，将Json字符串写入文件中
    StreamWriter sw = new StreamWriter(path);
    sw.Write(saveJsonStr);
    sw.Close();
}

//JSON读档函数：
public void LoadAsJson()
{ 
	string path = Application.dataPath + "/gameSaveByJson.json";

	//1. 检验目标位置是否有存档
    if(File.Exists(path))
    {
        //2. 创建一个StreamReader，用来读取流
        StreamReader sr = new StreamReader(path);

        //3. 将读取到的流赋值给jsonStr
        string jsonStr = sr.ReadToEnd();
        sr.Close();

        //4. 将字符串jsonStr转换为Save对象
        Save save = JsonMapper.ToObject<Save>(jsonStr);
		
		//从save对象读取信息到本地
		SetGame(save);
    }
    else
		Debug.Log("No gamesaved!"); 
}
```
#### JSON存档格式：
```
{
	"livingTargetPositions":[0,1,2,4],
	"livingTargetsTypes":[2,2,2,1],
	"hits":1,
	"shots":8
}
```

### 3. XML方法:

```
//XML存储
public void SaveAsXml()
{
    Save save = CreateSaveGameObject();

    //创建XML文件的存储路径
    string filePath = Application.dataPath + "/gameSaveByXML.txt";

    //创建XML文档
    XmlDocument xmlDoc = new XmlDocument();

    //创建根节点，即最上层节点
    XmlElement root = xmlDoc.CreateElement("save");

    //设置根节点中的值
    root.SetAttribute("name", "saveFile1");

    //创建XmlElement
    XmlElement target;
    XmlElement targetPosition;
    XmlElement targetType;

    //遍历save中存储的数据，将数据转换成XML格式
    for (int i = 0; i < save.livingTargetPositions.Count; i++)
    {
        target = xmlDoc.CreateElement("target");
        targetPosition = xmlDoc.CreateElement("targetPosition");

        //设置InnerText值
        targetPosition.InnerText = save.livingTargetPositions[i].ToString();
        targetType = xmlDoc.CreateElement("targetType");
        targetType.InnerText = save.livingTargetsTypes[i].ToString();

        //设置节点间的层级关系 root -- target -- (targetPosition, monsterType)
        target.AppendChild(targetPosition);
        target.AppendChild(targetType);
        root.AppendChild(target);
    }

    //设置射击数和分数节点并设置层级关系
    XmlElement shots = xmlDoc.CreateElement("shoots");
    shots.InnerText = save.shots.ToString();
    root.AppendChild(shots);

    XmlElement hits = xmlDoc.CreateElement("hits");
    hits.InnerText = save.hits.ToString();
    root.AppendChild(hits);

    xmlDoc.AppendChild(root);
    xmlDoc.Save(filePath);

    if (File.Exists(Application.dataPath + "/gameSaveByXML.txt"))
    {
        Debug.Log("Saving as XML");
    }
}

//XML读取
public void LoadAsXml()
{
    string filePath = Application.dataPath + "/gameSaveByXML.txt";
    if (File.Exists(filePath))
    {
        Save save = new Save();

        //加载XML文档
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);

        //通过节点名称来获取元素，结果为XmlNodeList类型
        XmlNodeList targets = xmlDoc.GetElementsByTagName("target");

        //遍历所有的target节点，并获得子节点和子节点的InnerText
        if (targets.Count != 0)
        {
            foreach (XmlNode target in targets)
            {
					//把得到的值存储到save中
                XmlNode targetPosition = target.ChildNodes[0];
                int targetPositionIndex = int.Parse(targetPosition.InnerText);                    
                save.livingTargetPositions.Add(targetPositionIndex);

                XmlNode targetType = target.ChildNodes[1];
                int targetTypeIndex = int.Parse(targetType.InnerText);
                save.livingTargetsTypes.Add(targetTypeIndex);
            }
        }

        //得到存储的射击数和分数
        XmlNodeList shoots = xmlDoc.GetElementsByTagName("shoots");
        int shootNumCount = int.Parse(shoots[0].InnerText);
        save.shots = shootNumCount;

        XmlNodeList hits = xmlDoc.GetElementsByTagName("hits");
        int hitsCount = int.Parse(hits[0].InnerText);
        save.hits = hitsCount;

        SetGame(save);
    }
    else
    {
        Debug.Log("No game saved!");
    }
}
```

#### XML存档格式：
```
<save name="saveFile1">
  <target>
    <targetPosition>0</targetPosition>
    <targetType>2</targetType>
  </target>
  <target>
    <targetPosition>1</targetPosition>
    <targetType>2</targetType>
  </target>
  <target>
    <targetPosition>2</targetPosition>
    <targetType>2</targetType>
  </target>
  <target>
    <targetPosition>3</targetPosition>
    <targetType>2</targetType>
  </target>
  <shoots>13</shoots>
  <hits>3</hits>
</save>
```
-------------

# 三、总述
无论是数据持久化方法还是序列化方法都可以实现Unity的存档机制。数据持久化方法操作方便，适用于数值较少的小项目。序列化方法的存档格式较为规范，其中二进制方法操作简单，但可读性差；JSON方法存档格式规范易读，具有一定的可读性；XML方法操作繁琐，但是存档格式可读性强，JSON和XML存档都可以用文本读取便于查看。
综上所述，Unity存档机制众多，但还应按照个人项目需求选择合适的存档方法。

---------------

# 四、参考
- [PlayerPrefs - Unity Documentation](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html)
- [How to Save and Load a Game in Unity - raywenderlich](https://www.raywenderlich.com/418-how-to-save-and-load-a-game-in-unity)
- [对于PlayerPrefs学习以及储存的研究 - 果vinegar](https://blog.csdn.net/yeluo_vinager/article/details/50074461)
- [Save&Load Unity存档读档的学习总结 - JoharWong](https://www.cnblogs.com/JoharWong/p/9867394.html)
- [C#中File和FileStream的用法 - 忆汐辰](https://blog.csdn.net/qq_41209575/article/details/89178020)
- [Application.dataPath - Unity Documentation](https://docs.unity3d.com/ScriptReference/Application-dataPath.html)