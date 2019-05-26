using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour
{

    public float speed = 0.15f;
    private int index = 0;
    private float Magic = 0.3f;

    //数组存放四条路径的预制体
    public GameObject[] wayPointsGo;
    private List<Vector3> wayPoints = new List<Vector3>();

    private Vector3 startPos;

    private void Start()
    {
        //初始位置为自身坐标向上3个单位
        startPos = transform.position + new Vector3(0, 3, 0);


        //结构类似 Load(wayPointsGo[A_index[B_index]]) 
        //B_index 为 GetComponent<SpriteRenderer>().sortingOrder - 2 表示怪物图层sortingOrder-2的值
        //A_index[] 为 B_index 作为下标取得的值
        //wayPointsGo[A_index] 为A_index 作为下标取得的值
        //区别于 Random.Range(0,wayPointsGo.Length)，它可能取得同一随机数
        //而经过GameManager内处理的 A_index 都为不同的随机数
        LoadApath(wayPointsGo[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder - 2]]);
    }

    private void FixedUpdate()
    {
        if (transform.position != wayPoints[index])
        {
            //怪物的移动，直到抵达目标位置
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index], speed);
            GetComponent<Rigidbody2D>().MovePosition(temp);
        }
        else
        {

            index++;
            if (index >= wayPoints.Count)
            {
                index = 0;
                LoadApath(wayPointsGo[Random.Range(0, wayPointsGo.Length)]);
            }
        }

        //获取移动方向
        Vector2 dir = wayPoints[index] - transform.position;

        //把获取到的移动方向设置到状态机
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    private void LoadApath(GameObject go)
    {
        //清空List内前次路径的信息
        wayPoints.Clear();


        //将wayPointsGo数组内某一路径的子物体(路径点)的Transform组件取出，依次将其position赋值到Ways表中
        //修改为多路径后随机从5条路径走
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t.position);
        }

        //添加首末路径点到List内
        wayPoints.Insert(0, startPos);
        wayPoints.Add(startPos);
    }

    //检测Enemy触发器范围内是否有触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果有触发，检测是否为Pacman触发
        if (collision.gameObject.name == "Pacman")
        {
            if (GameManager.Instance.isSuperPacman)
            {
                //当碰到了超级吃豆人，敌人回家
                transform.position = startPos - new Vector3(0, 3, 0);
                index = 0;

                //无敌期间吃到鬼加的得分
                GameManager.Instance.score += 500;
            }
            else
            {
                //是则隐藏 Pacman(避免其他bug不采用destory)
                collision.gameObject.SetActive(false);

                //游戏结束，先隐藏积分面板
                GameManager.Instance.gamePanel.SetActive(false);
                //再实例化结束面板
                Instantiate(GameManager.Instance.gameOverPrefab);
                //延迟3秒后，重载场景
                Invoke("ReStart", 3f);
            }

        }
    }

    //重新加载场景(重新开始)
    private void ReStart()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            speed += Magic;
            Magic = -Magic;
        }
    }
}
