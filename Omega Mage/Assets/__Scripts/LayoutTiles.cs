using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*-----根据XML文档创建迷宫(包括其位置,贴图,Mage初生地deng)-----*/
[System.Serializable]
public class TileTex
{
    public string str;
    public Texture2D tex;
}

[System.Serializable]
public class EnemyDef
{
    //该类可定义各种敌人
    public string str;
    public GameObject go;
}

public class LayoutTiles : MonoBehaviour
{
    static public LayoutTiles S;

    public TextAsset roomsText;         //Rooms.xml文件
    public string roomNumber = "0";     //当前room #作为一个字符串
    public GameObject tilePrefab;       //
    public TileTex[] tileTextures;      //Tiles的已命名文本列表
    public GameObject portalPrefab;     //房间之间入口的预制体
    public EnemyDef[] enemyDefinitions; //各类敌人的预制体
    public Text RoomNumberText;         //所在层数UI

    public bool __________________________;

    private bool firstRoom = true;      //是否是第一个创建的房间
    public PT_XMLReader roomsXMLR;
    public PT_XMLHashList roomsXML;
    public Tile[,] tiles;
    public Transform tileAnchor;

    private void Awake()
    {
        //为LayoutTiles设置Singleton值
        S = this;

        //创建一个新的GameObject为TileAnchor
        GameObject tAnc = new GameObject("TileAnchor");
        tileAnchor = tAnc.transform;

        //读取XML
        //创建一个PT_XMLReader对象
        roomsXMLR = new PT_XMLReader();
        //解析Rooms.xml文件
        roomsXMLR.Parse(roomsText.text);
        //导出所有<room>
        roomsXML = roomsXMLR.xml["xml"][0]["room"];

        //建立第0个room
        BuildRoom(roomNumber);       
    }

    public Texture2D GetTileTex(string tStr)
    {
        //遍历所有的tileTextures查找指定字符串
        foreach (TileTex tTex in tileTextures)
        {
            if (tTex.str == tStr)
                return (tTex.tex);
        }
        return (null);
    }


    //从XML<room>入口建立一个room对象
    public void BuildRoom(PT_XMLHashtable room)
    {
        //销毁旧的Tiles
        foreach (Transform t in tileAnchor)
            Destroy(t.gameObject);

        //Mage离开
        //Mage.S.pos = Vector3.left * 1000;
        GameObject.FindWithTag("Mage").transform.position = Vector3.left * 1000;
        Mage.S.ClearInput();
        //----------------------------------------------------------------

        string rNumStr = room.att("num");

        //从<room>属性获取floors和walls的文本名
        string floorTexStr = room.att("floor");
        string wallTexStr = room.att("wall");

        //基于Rooms.xml文件中返回的carriage值将room分行
        string[] roomRows = room.text.Split('\n');

        //从每行起始修剪制表符
        for (int i = 0; i < roomRows.Length; i++)
            roomRows[i] = roomRows[i].Trim('\t');

        //清空tiles数组
        tiles = new Tile[100, 100];

        //一些局部变量
        Tile ti;
        string type, rawType, tileTexStr;
        GameObject go;
        int height;
        float maxY = roomRows.Length - 1;
        List<Portal> portals = new List<Portal>();

        //循环遍历每个room的每行中的tile
        for (int y = 0; y < roomRows.Length; y++)
        {
            for (int x = 0; x < roomRows[y].Length; x++)
            {
                //设置默认值
                height = 0;
                tileTexStr = floorTexStr;

                //获取代表tile的字符
                type = rawType = roomRows[y][x].ToString();
                switch (rawType)
                {
                    case " ":       //跳过空格
                    case "_":
                        continue;
                    case ".":       //默认floor
                        break;
                    case "|":       //默认wall
                        height = 1;
                        break;
                    default:        //其他任何都作为floor
                        type = ".";
                        break;
                }

                //基于<room>属性设置floors和walls文本
                if (type == ".")
                    tileTexStr = floorTexStr;
                else if (type == "|")
                    tileTexStr = wallTexStr;

                //初始化新的TilePrefab对象
                go = Instantiate(tilePrefab) as GameObject;
                ti = go.GetComponent<Tile>();

                //设置父Transform为tileAnchor
                ti.transform.parent = tileAnchor;

                //设置tile坐标
                ti.pos = new Vector3(x, maxY - y, 0);
                tiles[x, y] = ti;

                //设置Tile的类型，高度和文本
                ti.type = type;
                ti.height = height;
                ti.tex = tileTexStr;


                //如果类型是rawType，则继续下一个操作
                if (rawType == type)
                    continue;

                //检查room中指定的对象实例
                switch (rawType)
                {              
                    //Mage的起始位置
                    case "X":
                        //出错代码：Mage.S.pos =  ti.pos;
                        //报错信息：单例化无法设置
                        //解决措施：
                        //GameObject.FindWithTag("Mage").transform.position = ti.pos;
                        if (firstRoom)
                        {
                            //Mage.S.pos = ti.pos;
                            GameObject.FindWithTag("Mage").transform.position = ti.pos;
                            roomNumber = rNumStr;
                            firstRoom = false;
                        }
                        break;
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "A":
                    case "B":
                    case "C":
                    case "D":
                    case "E":
                    case "F":
                        //实例化Portal,设置地点,归于tileAnchor下,赋通向房间的值,
                        GameObject pGo = Instantiate(portalPrefab) as GameObject;

                        //魔法阵的z方向需为负值,否则与地面齐平被覆盖
                        //产生问题:设置魔法阵后不可单击魔法门位置产生标记
                        //问题分析:Mage.cs内关于魔法阵识别问题,即便MouseTap()内判断
                        //解决方案:将魔法阵PortalPrefab_MagicCircle的tag设置为Ground,即可在MouseTap内将其归类于Ground进行识别
                        Portal p = pGo.GetComponent<Portal>();
                        p.pos = ti.pos + new Vector3(0, 0, -0.3f);
                        p.transform.parent = tileAnchor;
                        p.toRoom = rawType;
                        portals.Add(p);
                        break;

                    default:
                        //确认是否有Enemy对应的字母
                        Enemy en = EnemyFactory(rawType);
                        if (en == null)
                            break;
                        //设置新的Enemy
                        en.pos = ti.pos;
                        en.transform.parent = tileAnchor;
                        en.typeString = rawType;
                        break;
                }
                //
            }
        }

        //定位Mage
        foreach (Portal p in portals)
        {
            //如果p.toRoom与Mage刚离开的房间号相同,则Mage应从该入口交替进入房间
            //此外,若firstroom==true且房间内无X坐标,则应该让Mage移动到该入口作为备用手段
            if (p.toRoom == roomNumber || firstRoom)
            {
                //若房间内没有X坐标,则将firstroom设置为false
                Mage.S.StopWalking();
                Mage.S.pos = p.pos;

                //Mage保持面向先前的房间
                p.justArrived = true;

                //通知Portal,Mage刚进入
                firstRoom = false;
                //
            }
        }
        //最后分配roomNumber
        roomNumber = rNumStr;
    }

    //下列代码在 book.prototools.net 上有补充说明
    public void BuildRoom(string rNumStr)
    {
        PT_XMLHashtable roomHT = null;
        for (int i = 0; i < roomsXML.Count; i++)
        {
            PT_XMLHashtable ht = roomsXML[i];
            if (ht.att("num") == rNumStr)
            {
                roomHT = ht;
                break;
            }
        }
        if (roomHT == null)
        {
            Utils.tr("ERROR", "LayoutTiles.BuildRoom()",
                     "Room not found: " + rNumStr);
            return;
        }
        BuildRoom(roomHT);
    }

    public Enemy EnemyFactory(string sType)
    {
        //查看sType是否带有EnemyDef
        GameObject prefab = null;
        foreach(EnemyDef ed in enemyDefinitions)
        {
            if (ed.str == sType)
            {
                prefab = ed.go;
                break;
            }
        }
        if (prefab == null)
        {
            Utils.tr("LayoutTiles.EnemyFactory()", "No EnemyDef for: " + sType);
            return (null);
        }
        GameObject go = Instantiate(prefab) as GameObject;

        //用于Enemy接口的GetComponent格式
        Enemy en = (Enemy)go.GetComponent(typeof(Enemy));

        return (en);
    }

    private void Update()
    {
        //更新所在层数UI
        RoomNumberText.text = "Layer: " + roomNumber;
    }
}
