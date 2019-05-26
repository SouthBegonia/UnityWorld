using System.Collections.Generic;
using UnityEngine;
using System.Collections;  //协程调用
using UnityEngine.UI;   //UI刷新
using UnityEngine.SceneManagement;  //重载场景

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject Pacman;
    public GameObject Blinky;
    public GameObject Cycle;
    public GameObject Inky;
    public GameObject Pinky;

    //持有UI面板
    public GameObject startPanel;
    public GameObject gamePanel;


    //持有开始动画
    public GameObject startCountDownPrefab;
    public GameObject gameOverPrefab;
    public GameObject winPrefab;

    //持有音乐
    public AudioClip startClip;

    //超级吃豆人状态
    public bool isSuperPacman = false;

    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3 };
    private List<GameObject> pacdotGos = new List<GameObject>();

    //当前地图中剩余豆子，已吃多少个豆子，目前分数(要在EnemyMove脚本调用)
    private int pacdotNumber = 0;
    private int nowEat = 0;
    public int score = 0;

    //数组组件，用来更新UI
    public Text remainText;
    public Text nowText;
    public Text scoreText;


    private void Awake()
    {
        _instance = this;
        int tempCount = rawIndex.Count;

        for (int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }

        //取得所有豆子
        foreach (Transform t in GameObject.Find("Maze").transform)
        {
            pacdotGos.Add(t.gameObject);
        }

        //豆子是迷宫Maze的子物体，从迷宫内取得豆子数赋给pacdotNumber
        pacdotNumber = GameObject.Find("Maze").transform.childCount;
    }

    //生成超级豆点
    private void CreateSuperPacdot()
    {
        //当剩余豆子过少，则不再产生超级豆
        if (pacdotGos.Count < 7)
            return;

        //随机取得豆子
        int tempIndex = Random.Range(0, pacdotGos.Count);

        //变大豆子为原来3倍
        pacdotGos[tempIndex].transform.localScale = new Vector3(3, 3, 3);

        //设置该豆子为超级豆子属性
        pacdotGos[tempIndex].GetComponent<Pacdot>().isSupperPacdot = true;
    }

    //初始化
    private void Start()
    {
        //开局停止状态，等待点击Start
        SetGameState(false);
    }

    //当点击开始按钮(对UI图标添加On Click->GameManager->OnStartButton启用该函数)
    public void OnStartButton()
    {
        //与点击开始按钮后同步进行的函数
        StartCoroutine(PlayStartCountDown());

        //Start声音，声音源在原点位置
        AudioSource.PlayClipAtPoint(startClip, Vector3.zero);

        //隐藏开始按钮的页面
        startPanel.SetActive(false);
    }

    //点击Exit后退出游戏
    public void OnExitButton()
    {
        Application.Quit();
    }

    //协程函数：与点下开始按钮同步进行
    IEnumerator PlayStartCountDown()
    {
        //在方法执行过程中延时，从动作播放开始计时，计时4s后销毁倒计时动作
        GameObject go = Instantiate(startCountDownPrefab);
        yield return new WaitForSeconds(4f);
        Destroy(go);

        //设置开始游戏状态
        SetGameState(true);

        //开始后，每10s产生一个超级豆子
        Invoke("CreateSuperPacdot", 10f);

        //显示积分面板
        gamePanel.SetActive(true);

        //播放bgm
        GetComponent<AudioSource>().Play();
    }

    //开局设置：吃豆人及怪物都不动，等待start()内传入布尔值启用
    private void SetGameState(bool state)
    {
        Pacman.GetComponent<PacmanMove>().enabled = state;
        Blinky.GetComponent<EnemyMove>().enabled = state;
        Cycle.GetComponent<EnemyMove>().enabled = state;
        Inky.GetComponent<EnemyMove>().enabled = state;
        Pinky.GetComponent<EnemyMove>().enabled = state;
    }

    //吃到豆子后
    public void OnEatPacdot(GameObject go)
    {
        //吃到普通豆子，从表内移除
        pacdotGos.Remove(go);

        //吃到的豆子数及得分增加
        nowEat++;
        score += 100;
    }

    //吃到超级豆子后
    public void OnEatSuperPacdot()
    {
        //得分增加
        score += 200;

        //延迟调用：延迟10s后调用创建超级豆子函数
        Invoke("CreateSuperPacdot", 10f);

        //变更为超级状态，冻结敌人
        isSuperPacman = true;
        FreezeEnemy();


        //普通延时方法：Invoke("Recover", 10f);
        //协程延时方法：
        StartCoroutine(Recover());
    }

    //恢复状态，协程延时
    IEnumerator Recover()
    {
        //等待4s：相当于持续超级吃豆人状态4s
        yield return new WaitForSeconds(4f);

        Dis_FreezeEnemy();
        isSuperPacman = false;
    }

    private void FreezeEnemy()
    {
        //冻结无法移动：禁用update方法
        Blinky.GetComponent<EnemyMove>().enabled = false;
        Cycle.GetComponent<EnemyMove>().enabled = false;
        Inky.GetComponent<EnemyMove>().enabled = false;
        Pinky.GetComponent<EnemyMove>().enabled = false;

        //变色：敌人图标变暗淡
        Blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Cycle.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    }

    private void Dis_FreezeEnemy()
    {
        //解冻
        Blinky.GetComponent<EnemyMove>().enabled = true;
        Cycle.GetComponent<EnemyMove>().enabled = true;
        Inky.GetComponent<EnemyMove>().enabled = true;
        Pinky.GetComponent<EnemyMove>().enabled = true;

        //恢复原色
        Blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Cycle.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    //按下 Win 图标，重载游戏
    public void OnWinButton()
    {
        SceneManager.LoadScene(0);

    }

    //按下 Persona 图标，跳转网页
    public void OnPersonaButton()
    {
        Application.OpenURL("https://www.cnblogs.com/SouthBegonia");
    }

    //时时更新UI
    private void Update()
    {
        //当吃完所有豆子
        if (pacdotNumber == nowEat && Pacman.GetComponent<PacmanMove>().enabled != false)
        {
            //隐藏积分面板和背景面板，显示胜利面板
            gamePanel.SetActive(false);
            winPrefab.SetActive(true);


            //我需要的是实例化Win面板，且面板上的UI图标带有按键功能；
            //但下列代码实例化只能用于物体，且无法显示再面板，也无法进行按键赋值
            //Instantiate(winPrefab);


            //取消其他所有协程
            StopAllCoroutines();

            SetGameState(false);
        }

        if (pacdotNumber == nowEat)
        {
            //
            if (Input.GetKey(KeyCode.Z))
            {
                OnPersonaButton();

                //Invoke("", 5f);
                //SceneManager.LoadScene(0);
                //Application.OpenURL("https://www.cnblogs.com/SouthBegonia");
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                OnWinButton();
            }

        }

        //如果显示得分的UI gamePanel 为显示状态
        if (gamePanel.activeInHierarchy)
        {
            //修改UI文本
            remainText.text = "Remain:\n\n" + (pacdotNumber - nowEat);
            nowText.text = "Eaten:\n\n" + nowEat;
            scoreText.text = "Score:\n\n" + score;
        }
    }

}
