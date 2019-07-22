using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITest_2 : MonoBehaviour
{
    public Texture aTexture;
    public string passWord = "oldpassword";
    public bool toggleTxt = false;
    public int toolbarInt = 0;
    public string[] toolbarStrings = new string[] { "Toolbar1", "Toolbar2", "Toolbar3" };
    public float hSliderValue = 0.0F;

    private void OnGUI()
    {
        //开始一个垂直控件的组
        GUILayout.BeginVertical();

        //1.创建自动布局的box，纹理形式
        if (aTexture)
            GUILayout.Box(aTexture);
        
        //修改全局字体颜色
        GUI.contentColor = Color.yellow;

        //创建自动布局的box
        GUILayout.Box("Enter a New Password");

        //2.创建一个单行密码文本字段，用户可以输入密码
        //static function PasswordField (password : string, maskChar : char, maxLength : int)
        passWord = GUILayout.PasswordField(passWord, "*"[0], 25);

        //3.创建一个开关按钮
        toggleTxt = GUILayout.Toggle(toggleTxt, "A Toggle text");

        //4.创建一片空白，20像素
        GUILayout.Space(20);

        //5.创建一个工具栏
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);

        //6.创建一个水平滑动条
        hSliderValue = GUILayout.HorizontalSlider(hSliderValue, 0.0F, 10.0F);

        

        GUILayout.EndVertical();
    }
}
