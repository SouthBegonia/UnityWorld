using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameMode
{
    idel,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static public MissionDemolition S;      //单例对象

    public GameObject[] castles;        //储存所有城堡的数组
    public Text gtLevel;                //GT_Level
    public Text gtScore;                //GT_Score
    public Text gtWin;                  //GT_Win
    public Vector3 castlePos;           //城堡放置位置
    public bool __________________;

    public int level;                   //当前级别
    public int levelMax;                //级别的数量
    public int shotsTaken;
    public GameObject castle;           //当前城堡
    public GameMode mode = GameMode.idel;
    public string showing = "Slingshot";//摄像机模式

    private void Start()
    {
        S = this;

        //初始化当前级别(难度)
        level = 0;
        levelMax = castles.Length;

        //隐藏胜利UI
        gtWin.gameObject.SetActive(false);

        //开始当前级别
        startLevel();
    }

    private void Update()
    {
        ShowGT();

        Debug.Log("Now LEvel : " + level);

        //检查是否已完成该级别
        if(mode == GameMode.playing && Goal.goalmet)
        {
            //级别++，准备下一关卡
            level++;

            //当完成级别时，改变mode，停止检查
            mode = GameMode.levelEnd;

            //缩小画面比例
            SwitchView("Both");

            //当完成所有级别时代表游戏胜利，否则加载下一级别
            if (level == levelMax)
            {
                //显示胜利UI
                gtWin.gameObject.SetActive(true);

                //4s后重载场景
                Invoke("ReloadGame", 4f);
            }else
                Invoke("NextLevel", 3f);
        }
    }

    //在屏幕顶端的按键，用于切换视图
    private void OnGUI()
    {
        Rect buttonRect = new Rect((Screen.width / 2) - 50, 10, 100, 24);
        switch (showing)
        {
            case "Slingshot":
                if (GUI.Button(buttonRect, "查看城堡"))
                    SwitchView("Castle");
                break;
            case "Castle":
                if (GUI.Button(buttonRect, "查看全部"))
                    SwitchView("Both");
                break;
            case "Both":
                if (GUI.Button(buttonRect, "查看弹弓"))
                    SwitchView("Slingshot");
                break;
        }
    }

    void startLevel()
    {
        //如果已经有城堡存在，则清除原有的城堡
        if (castle != null)
            Destroy(castle);

        //清除原有的弹丸
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
            Destroy(pTemp);

        //重置相机的位置
        SwitchView("Both");
        ProjectileLine.S.Clear();

        //重置目标状态
        Goal.goalmet = false;
        ShowGT();
        mode = GameMode.playing;
        
        //实例化当前级别城堡，设定坐标，清零弹丸数
        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsTaken = 0;
    }

    void NextLevel()
    {
        //重置上次遗留的计时问题
        Slingshot.S.time = 0;
        Slingshot.S.canTime = false;
        Slingshot.S.canGetGoal = false;

        //开始下一场景
        startLevel();
    }

    //重新开始游戏
    void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }

    //更新页面UI
    void ShowGT()
    {
        //发射次数及完成关卡
        gtLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        gtScore.text = "Shots Taken: " + shotsTaken;
    }

    //切换视图的静态方法，允许在代码任何位置
    static public void SwitchView(string eView)
    {
        S.showing = eView;
        switch (S.showing)
        {
            case "Slingshot":
                FollowCam.s.poi = null;
                break;
            case "Castle":
                FollowCam.s.poi = S.castle;
                break;
            case "Both":
                FollowCam.s.poi = GameObject.Find("ViewBoth");
                break;
        }
    }

    //增加发射次数的代码，允许在代码的任何位置
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}