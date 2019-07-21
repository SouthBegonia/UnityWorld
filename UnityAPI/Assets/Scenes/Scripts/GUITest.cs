using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITest : MonoBehaviour
{
    private float sliderValue = 1.0f;
    private float maxSliderValue = 10.0f;

    private void OnGUI()
    {
        //在屏幕上开始一个固定大小的布局区域，原点(0,0)，大小200x60
        GUILayout.BeginArea(new Rect(0, 0, 200, 100));

        //开始一个水平控件的组
        GUILayout.BeginHorizontal();

        //创建一个重复按钮，点击按钮会立即发生事件，持续按住，按钮返回true。
        if (GUILayout.RepeatButton("Increase max\nSlider Value"))
        {
            //增加最大值
            maxSliderValue += 3.0f * Time.deltaTime;
        }

        //开始一个垂直控件的组
        GUILayout.BeginVertical();

        //创建一个自动布局的box，Mathf.Round()进行四舍五入运算，当为 ".5"时，无论奇偶一概返回偶数
        GUILayout.Box("Slider Value: " + Mathf.Round(sliderValue));

        //创建一个水平滑动条，用户可以拖动改变在最小和最大值之间的值
        //当前值sliderValue，最小值0，最大值maxSliderValue
        sliderValue = GUILayout.HorizontalSlider(sliderValue, 0.0f, maxSliderValue);

        //创建一个单次按钮，单次点击即返回true
        if (GUILayout.Button("Reset Value"))
        {
            maxSliderValue = 1.0f;
        }

        //关闭各组(必要)
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}