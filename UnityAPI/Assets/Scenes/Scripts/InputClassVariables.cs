//Input中的类变量
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputClassVariables : MonoBehaviour
{
    public Text messageText;
    public Text anyKeyText;
    public Text anyKeyDownText;
    private int anyKeyNum;
    private int anyKeyDownNum;

    private void Start()
    {
        anyKeyNum = 0;
        anyKeyDownNum = 0;
    }

    void Update()
    {
        //1. Input.anyKey:是否有某一按键或鼠标按钮此时被按住？按住的每一帧都返回true
        if (Input.anyKey)
        {
            anyKeyNum++;
            anyKeyText.text = "anyKey: " + anyKeyNum;
            Debug.Log("A key or mouse click has been detected");
        }

        //2. Input.anyKeyDown:当用户按下某一按键或鼠标按钮的第一帧，返回true
        //当松开按键/按钮后，重置下一次帧数的状态
        if (Input.anyKeyDown)
        {
            anyKeyDownNum++;
            anyKeyDownText.text = "anyKeyDown: " + anyKeyDownNum;
            Debug.Log("A key or mouse click has been detected");
        }

        //3. Input.inputString:返回这一帧的键盘输入
        //将读取的字符串赋值于message.text
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                //退格符，表示删除上个字符
                if (messageText.text.Length != 0)
                    messageText.text = messageText.text.Substring(0, messageText.text.Length - 1);
            }
            else if (c == "\n"[0] || c == "\r"[0])
            {
                //回车符，表示输入完毕
                print("User entered his name: " + messageText.text);
            }
            else
            {
                //其余为待录入的字符
                messageText.text += c;
            }
        }
    }
}
