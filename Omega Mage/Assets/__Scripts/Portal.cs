using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----切换房间标识点,挂在于PortalPrefab-----*/
public class Portal : PT_MonoBehaviour
{
    public string toRoom;               //通往房间
    public bool justArrived = false;
    public GameObject magicCircleMusic; //传送音乐

    private void Awake()
    {
        magicCircleMusic = GameObject.Find("MU_MagicCircle");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (justArrived)
            return;

        //获取collider对象,搜索单击父道具
        GameObject go = other.gameObject;
        GameObject goP = Utils.FindTaggedParent(go);
        if (goP != null)
            go = goP;

        //如果不是Mage,则返回
        if (go.tag != "Mage")
            return;

        //播放魔法门传送bgm
        magicCircleMusic.GetComponent<AudioSource>().Play();

        //继续创建下一个房间
        LayoutTiles.S.BuildRoom(toRoom);
    }

    private void OnTriggerExit(Collider other)
    {
        //一旦Mage离开Portal,设置ustArrived为false
        if(other.gameObject.tag=="Mage")
        {
            justArrived = false;
        }
    }
}
