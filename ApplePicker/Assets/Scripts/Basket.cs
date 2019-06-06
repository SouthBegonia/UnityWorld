using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    public Text scoreGT;
    public int scores = 0;

    private void Start()
    {
        //取得的ScoreCounter组件，初始化score的UI
        GameObject scoreGo = GameObject.Find("ScoreCounter");
        scoreGT = scoreGo.GetComponent<Text>();
        scoreGT.text = "Score: 0";
    }

    private void Update()
    {
        //获取鼠标在屏幕中位置
        Vector3 mousePos2D = Input.mousePosition;

        //摄像机的z坐标决定在三维空间中将鼠标沿z轴向前移动多远
        mousePos2D.z = -Camera.main.transform.position.z;

        //将该点从二维屏幕空间转换为三位游戏世界空间
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //移动设置
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
    }

    //碰撞检测：吃到苹果
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collideWith = collision.gameObject;
        if (collideWith.tag == "Apple")
            Destroy(collideWith);

        //吃到苹果，更新分数UI
        scores += 10;
        scoreGT.text = "Score: " + scores;

        //更新历史最高分
        if (scores > HighScore.score)
            HighScore.score = scores;
    }
}
