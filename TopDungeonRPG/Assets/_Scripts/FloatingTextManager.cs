using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;        //FloatingTextManager自身(Panel)
    public GameObject textPrefab;           //FloatingText

    private List<FloatingText> floatingTexts = new List<FloatingText>();    //当前显示的所有FloatingText


    private void Update()
    {
        //实时刷新Text
        foreach (FloatingText txt in floatingTexts)
        {         
            txt.UpdateFloatingText();
        }          
    }

    //配置Text信息的函数:
    public void Show(string msg,int fontSize, Color color, Vector3 position,Vector3 motion,float duration)
    {
        //取得Text
        FloatingText FloatingText = GetFloatingText();

        //设置Text的各项参数
        FloatingText.text.text = msg;
        FloatingText.text.fontSize = fontSize;
        FloatingText.text.color = color;

        FloatingText.go.transform.position = Camera.main.WorldToScreenPoint(position);

        FloatingText.motion = motion;
        FloatingText.duration = duration;

        //开启显示状态
        FloatingText.Show();
    }

    //取得Text函数:
    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.text = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }
        return txt;
    }
}
