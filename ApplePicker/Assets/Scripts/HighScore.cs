using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    //历史最高分
    //score 为静态全局变量，可以在任何脚本中访问
    static public int score = 0;

    private void Awake()
    {
        //如果 ApplePickerHighScore 已经存在，则读取其值
        if (PlayerPrefs.HasKey("ApplePickerHighScore"))
            score = PlayerPrefs.GetInt("ApplePickerHighScore");

        //将最高分赋给PlayerPrefs 中的 ApplePickerHighScore 关键字
        //若存在该关键字，则覆写；否则创建该关键字
        PlayerPrefs.SetInt("ApplePickerHighScore", score);

        /*
        由此，Update()每帧都会检查当前的HighScore.score 是否高于 PlayPrefs 中储存的最分
        甚至重启计算机后也得以保存，类似存档功能
         */
    }

    private void Update()
    {
        //更新最高分
        Text gt = this.GetComponent<Text>();
        gt.text = "High Score: " + score;

        //更新 PlayerPrefs 中的 ApplePickerHighScore
        if (score > PlayerPrefs.GetInt("ApplePickerHighScore"))
            PlayerPrefs.SetInt("ApplePickerHighScore", score);
    }
}
