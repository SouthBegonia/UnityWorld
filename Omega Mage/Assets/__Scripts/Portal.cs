using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----切换房间标识点,挂在于PortalPrefab-----*/
public class Portal : PT_MonoBehaviour
{
    public string toRoom;
    public bool justArrived = false;

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
