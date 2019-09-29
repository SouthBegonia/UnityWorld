# Unity - 存读档机制简析

本文旨在于简要分析Unity中的两种存档机制，即：**PlayerPrefs数据持久化方法**及**Serialization数据序列化方法** 。

--------------

# 一、PlayerPrefs 数据持久化方法

1. 存储原理：采用键值对(key与value)的方法，将游戏数据储存到本地，是一种Unity自带的储存方法。
2. 储存类型：仅支持int、float、string三种
3. 储存地址：对于Windows系统下，存储于
4. 读写示例：

```
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

**二进制存储（Binary Formatter）**：
```
//存档信息的类：
[System.Serializable]
public class Save
{
    public int health = 0;
    public int score = 0;
}

//存档函数：
public void SaveGame()
{
	//1. 序列化过程
    //创建save对象保存游戏信息
    Save save = CreateSaveGameObject();

    //2. 创建二进制格式化程序及文件流
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(Application.dataPath + "/gamesave.save");

    //3. 将save对象序列化到file流
    bf.Serialize(file, save);
    file.Close();
}

//读档函数：
public void LoadGame()
{
    //1. 检验目标位置是否有存档
    if (File.Exists(Application.dataPath + "/gamesave.save"))
    {
        //2. 创建二进制格式化程序，打开文件流
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/gamesave.save", FileMode.Open);

		//3. 将file流反序列化到save对象		
        Save save = (Save)bf.Deserialize(file);
        file.Close();

		//从save对象读取信息到本地
        health = save.health;
        score = save.score;
    }
    else  
        Debug.Log("No gamesaved!"); 
}
```

**JSON方法**：
```
//JSON存档函数：
private void SaveAsJson()
{	
	//1. 创建save对象保存游戏信息
    Save save = CreateSaveGameobject();
	
    //2. 利用JsonMapper将save对象转换为Json格式的字符串
    string saveJsonStr = JsonMapper.ToJson(save);

    //3. 创建StreamWriter，将Json字符串写入文件中
    StreamWriter sw = new StreamWriter(Application.dataPath + "/StreamingFile" + "/AsJson.json");
    sw.Write(saveJsonStr);
    sw.Close();
}

//JSON读档函数：
private void LoadAsJson()
{ 
	//1. 检验目标位置是否有存档
    if(File.Exists(Application.dataPath + "/StreamingFile" + "/byJson.json"))
    {
        //2. 创建一个StreamReader，用来读取流
        StreamReader sr = new StreamReader(Application.dataPath + "/StreamingFile" + "/byJson.json");

        //3. 将读取到的流赋值给jsonStr
        string jsonStr = sr.ReadToEnd();
        sr.Close();

        //4. 将字符串jsonStr转换为Save对象
        Save save = JsonMapper.ToObject<Save>(jsonStr);
    }
    else
		Debug.Log("No gamesaved!"); 
}
```

# 参考
- [PlayerPrefs - Unity Documentation](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html)
- [How to Save and Load a Game in Unity - raywenderlich](https://www.raywenderlich.com/418-how-to-save-and-load-a-game-in-unity)
- [对于PlayerPrefs学习以及储存的研究 - 果vinegar](https://blog.csdn.net/yeluo_vinager/article/details/50074461)
- [Save&Load Unity存档读档的学习总结 - JoharWong](https://www.cnblogs.com/JoharWong/p/9867394.html)
- [C#中File和FileStream的用法 - 忆汐辰](https://blog.csdn.net/qq_41209575/article/details/89178020)