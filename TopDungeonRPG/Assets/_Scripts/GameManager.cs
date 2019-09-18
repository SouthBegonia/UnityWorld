using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //单例
    public static GameManager instance;

    //资源:
    public List<Sprite> playerSprites;              //玩家Sprite
    public List<Sprite> weaponSprites;              //武器Sprite
    public AudioSource audioSource;
    public bool _____________________;

    //游戏数值:
    public int pesos;                               //金币
    public int experience;                          //经验
    public List<int> weaponPrices;                  //武器价格表
    public List<int> xpTable;                       //升级经验表
    public bool ______________________;

    //各类引用:
    public Player player;                           //玩家
    public Weapon weapon;                           //武器
    public CharacterMenu menu;                      //装备菜单(左下角)
    public CharacterHUD hud;                        //生命值经验值菜单(左上角)
    public Animator deathMenuAnim;                  //死亡界面动画
    public FloatingTextManager FloatingTextManager; //文本显示



    private void Awake()
    {
        //防止部分不销毁物体重复在场景重载后重复
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(FloatingTextManager.gameObject);
            Destroy(menu.gameObject);
            Destroy(hud.gameObject);
            return;
        }

        instance = this;

        //加载存档
        SceneManager.sceneLoaded += LoadState;

        //保留的物体
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.Find("FloatingTextManager").transform.parent);
        DontDestroyOnLoad(GameObject.Find("Menu"));
        DontDestroyOnLoad(GameObject.Find("HUD"));
    }

    //通用显示Text信息函数:
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //判断武器是否能够升级函数:
    public bool TryUpgradeWeapon()
    {
        //若武器等级已经达到最高,则无法再升级
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;

        //若金钱足够,则进行升级
        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();

            return true;
        }
        return false;
    }

    //XP升级系统:
    public int GetCurrentLevel()
    {
        int l = 0, add = 0;
        while (experience >= add)
        {
            add += xpTable[l];
            l++;

            if (l == xpTable.Count)
                return l;
        }
        return l;
    }
    public int GetXPToLevel(int level)
    {
        int l = 0, xp = 0;
        while (l < level)
        {
            xp += xpTable[l];
            l++;
        }
        return xp;
    }
    public void GrantXP(int xp)
    {
        int currentLevel = GetCurrentLevel();
        experience += xp;

        menu.UpdateMenu();

        if (currentLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }
    public void OnLevelUp()
    {
        ShowText("LEVEL UP!", 30, Color.yellow, player.transform.position, Vector3.up * 30, 2.0f);

        OnHitpointChange();
        player.OnLevelUp();
    }

    //更新生命值UI信息函数:
    public void OnHitpointChange()
    {
        menu.UpdateMenu();
        hud.UpdateHUD();
    }

    //复活函数:
    public void Respawn()
    {
        //隐藏死亡UI,重载主场景
        deathMenuAnim.SetTrigger("Hide");
        SceneManager.LoadScene("Main");

        //配置重生信息
        player.Respawn();
    }


    //存储存档的函数:
    public void SaveState()
    {
        Debug.Log("SaveState");

        //游戏数值载体s
        string s = "";

        //以s字符串为载体储存游戏数值信息, '|'为间隔符,区分开各类游戏数值
        s += "0" + "|";                     //data[0]
        s += pesos.ToString() + "|";        //data[1] 金币
        s += experience.ToString() + "|";   //data[2] 经验值
        s += weapon.weaponLevel.ToString(); //data[3] 武器等级

        //存储游戏信息字符串
        PlayerPrefs.SetString("SaveState", s);      
    }

    //加载存档的函数:
    public void LoadState(Scene s, LoadSceneMode sceneMode)
    {
        Debug.Log("LoadState");

        //备注:是Save不是Sava,千万别写错,否则找不到
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        //取得各游戏信息到data[]内
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');
        //    s: "10|20|30|5"   => "10" "20" "30" "5"
        //Debug.Log("data:" + data[0] + "|" + data[1] + "|" + data[2] + "|" + data[3]);

        //加载金币
        pesos = int.Parse(data[1]);

        //加载经验及玩家等级
        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        //加载武器
        weapon.SetWeaponLevel(int.Parse(data[3]));
              
        //设置场景出生地
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
