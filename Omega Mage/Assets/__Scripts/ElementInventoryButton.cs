using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementInventoryButton : MonoBehaviour
{
    public ElementType type;

    private void Awake()
    {
        //将Gameobject名字的首个数字字符解析为int型
        char c = gameObject.name[0];
        string s = c.ToString();
        int typeNum = int.Parse(s);
        //Debug.Log(gameObject.name+"///"+gameObject.name[0]);
        
        //将int定型为ElemeType
        type = (ElementType)typeNum;
    }

    private void OnMouseUpAsButton()
    {
        //给Mage添加此类道具
        Mage.S.SelectedElement(type);
    }
}
