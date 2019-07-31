using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : PT_MonoBehaviour
{
    public string type;

    private string _tex;
    private int _height = 0;
    private Vector3 _pos;
    

    public int height
    {
        get { return (_height); }
        set
        {
            _height = value;
            AdjustHeight();
        }
    }

    //基于字符串设置墙壁砖文本
    //请求LayoutTiles
    public string tex
    {
        get { return (_tex); }
        set {
            _tex = value;

            //设置此游戏对象的名称
            name = "TilePrefab_" + _tex;
            Texture2D t2D = LayoutTiles.S.GetTileTex(_tex);
            if (t2D == null)
            {
                Utils.tr("ERROR", "Tile.type{set}=", value,
                    "No matching Texture2D in LayoutTiles.S.tileTextures!");
            }
            else GetComponent<Renderer>().material.mainTexture = t2D;                      
        }
    }
    

    //用new关键字代替从PT_MonoBehaviour中继承的pos方法
    new public Vector3 pos
    {
        get { return (_pos); }
        set
        {
            _pos = value;
            AdjustHeight();
        }
    }

    public void AdjustHeight()
    {
        //基于_height变量上下移动墙壁砖块
        Vector3 vertOffset = Vector3.back * (_height - 0.5f);

        //-0.5f使得Tile向下移动0.5个单元，当pos.z=0并且height=0时顶部接口在z=0的位置
        transform.position = _pos + vertOffset;
    }
}