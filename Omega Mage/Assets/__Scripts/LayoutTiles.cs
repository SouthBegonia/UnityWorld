using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----根据XML文档创建迷宫(包括其位置,贴图,Mage初生地deng)-----*/
[System.Serializable]
public class TileTex
{
    public string str;
    public Texture2D tex;
}

public class LayoutTiles : MonoBehaviour
{
    static public LayoutTiles S;

    public TextAsset roomsText;         //Rooms.xml文件
    public string roomNumber = "0";    //当前room #作为一个字符串
    public GameObject tilePrefab;       //
    public TileTex[] tileTextures;      //Tiles的已命名文本列表
    public bool __________________________;

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
        foreach(TileTex tTex in tileTextures)
        {
            if (tTex.str == tStr)
                return (tTex.tex);
        }
        return (null);
    }

    //从XML<room>入口建立一个room对象
    public void BuildRoom(PT_XMLHashtable room)
    {
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

        //循环遍历每个room的每行中的tile
        for(int y = 0; y < roomRows.Length; y++)
        {
            for (int x = 0; x < roomRows[y].Length - 1; x++)
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
                    case "X":       //Mage的起始位置
                        {
                            //出错代码：Mage.S.pos =  ti.pos;
                            //报错信息：单例化无法设置
                            //解决措施：
                            GameObject.FindWithTag("Mage").transform.position = ti.pos;
                            break;
                        }
                }
            }
        }           
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
}
