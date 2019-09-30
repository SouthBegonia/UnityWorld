/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Xml;

public class Game : MonoBehaviour
{
    public List<GameObject> bullets = new List<GameObject>();
    public int shots = 0;
    public int hits = 0;

    [SerializeField]
    private Text hitsText;
    [SerializeField]
    private Text shotsText;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject[] targets;
    private bool isPaused = false;

    private void Awake()
    {
        Pause();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Pause()
    {
        menu.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Unpause()
    {
        menu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        isPaused = false;
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void NewGame()
    {
        hits = 0;
        shots = 0;
        shotsText.text = "Shots: " + shots;
        hitsText.text = "Hits: " + hits;

        ClearRobots();
        ClearBullets();
        RefreshRobots();

        Unpause();
    }

    private void ClearRobots()
    {
        foreach (GameObject target in targets)
        {
            target.GetComponent<Target>().DisableRobot();
        }
    }

    private void ClearBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }

    private void RefreshRobots()
    {
        foreach (GameObject target in targets)
        {
            target.GetComponent<Target>().RefreshTimers();
        }
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        int i = 0;
        foreach (GameObject targetGameObject in targets)
        {
            Target target = targetGameObject.GetComponent<Target>();
            if (target.activeRobot != null)
            {
                save.livingTargetPositions.Add(target.position);
                save.livingTargetsTypes.Add((int)target.activeRobot.GetComponent<Robot>().type);
                i++;
            }
        }

        save.hits = hits;
        save.shots = shots;

        return save;
    }

    public void AddShot()
    {
        shots++;
        shotsText.text = "Shots: " + shots;
    }

    public void AddHit()
    {
        hits++;
        hitsText.text = "Hits: " + hits;
    }

    //设置游戏数值
    public void SetGame(Save save)
    {
        ClearBullets();
        ClearRobots();
        RefreshRobots();

        hits = save.hits;
        shots = save.shots;
        shotsText.text = "Shots: " + save.shots;
        hitsText.text = "Hits: " + save.hits;

        for (int i = 0; i < save.livingTargetPositions.Count; i++)
        {
            int position = save.livingTargetPositions[i];
            Target target = targets[position].GetComponent<Target>();
            target.ActivateRobot((RobotTypes)save.livingTargetsTypes[i]);
            target.GetComponent<Target>().ResetDeathTimer();
        }
    }

    //-------------------------------------------
    //Serialize存储
    public void SaveGame()
    {
        //1. 创建存档信息
        Save save = CreateSaveGameObject();

        //2.创建二进制格式化程序和文件流
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/gameSaveBySerialize.save");

        //3. 将save对象序列化到file流
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved By Serialize");
    }

    //Serialize读取
    public void LoadGame()
    {
        if (File.Exists(Application.dataPath + "/gameSaveBySerialize.save"))
        {
            //1.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/gameSaveBySerialize.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            //2.
            SetGame(save);

            Debug.Log("Game Loaded By Serialize");
            Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    //-------------------------------------------
    //JSON存储
    public void SaveAsJSON()
    {

        //1. 创建存档信息
        Save save = CreateSaveGameObject();
        string path = Application.dataPath + "/gameSaveByJson.json";

        //2. 利用JsonMapper将save对象转换为Json格式的字符串
        string jsonStr = LitJson.JsonMapper.ToJson(save);

        //3. 创建一个StreamWriter，并将字符串写入
        StreamWriter sw = new StreamWriter(path);
        sw.Write(jsonStr);
        sw.Close();

        Debug.Log("Saving as JSON: " + jsonStr);
    }

    //JSON读取
    public void LoadAsJSON()
    {
        string path = Application.dataPath + "/gameSaveByJson.json";

        if (File.Exists(path))
        {
            //1. 创建StreamReader用来读取流
            StreamReader sr = new StreamReader(path);

            //2. 
            string jsonStr = sr.ReadToEnd();
            sr.Close();

            //3.
            Save save = JsonMapper.ToObject<Save>(jsonStr);
            SetGame(save);

            Debug.Log("Game Loaded By Serialize");
        }
        else
            Debug.Log("No game saved!");

        Unpause();
    }

    //-------------------------------------------
    //XML存储
    public void SaveAsXml()
    {
        Save save = CreateSaveGameObject();

        //创建XML文件的存储路径
        string filePath = Application.dataPath + "/gameSaveByXML.txt";

        //创建XML文档
        XmlDocument xmlDoc = new XmlDocument();

        //创建根节点，即最上层节点
        XmlElement root = xmlDoc.CreateElement("save");

        //设置根节点中的值
        root.SetAttribute("name", "saveFile1");

        //创建XmlElement
        XmlElement target;
        XmlElement targetPosition;
        XmlElement monsterType;

        //遍历save中存储的数据，将数据转换成XML格式
        for (int i = 0; i < save.livingTargetPositions.Count; i++)
        {
            target = xmlDoc.CreateElement("target");
            targetPosition = xmlDoc.CreateElement("targetPosition");

            //设置InnerText值
            targetPosition.InnerText = save.livingTargetPositions[i].ToString();
            monsterType = xmlDoc.CreateElement("targetType");
            monsterType.InnerText = save.livingTargetsTypes[i].ToString();

            //设置节点间的层级关系 root -- target -- (targetPosition, monsterType)
            target.AppendChild(targetPosition);
            target.AppendChild(monsterType);
            root.AppendChild(target);
        }

        //设置射击数和分数节点并设置层级关系  xmlDoc -- root --(target-- (targetPosition, monsterType), shootNum, score)
        XmlElement shots = xmlDoc.CreateElement("shoots");
        shots.InnerText = save.shots.ToString();
        root.AppendChild(shots);

        XmlElement hits = xmlDoc.CreateElement("hits");
        hits.InnerText = save.hits.ToString();
        root.AppendChild(hits);

        xmlDoc.AppendChild(root);
        xmlDoc.Save(filePath);

        if (File.Exists(Application.dataPath + "/gameSaveByXML.txt"))
        {
            Debug.Log("Saving as XML");
        }
    }

    //XML读取
    public void LoadAsXml()
    {
        string filePath = Application.dataPath + "/gameSaveByXML.txt";
        if (File.Exists(filePath))
        {
            Save save = new Save();
            //加载XML文档
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            //通过节点名称来获取元素，结果为XmlNodeList类型
            XmlNodeList targets = xmlDoc.GetElementsByTagName("target");

            //遍历所有的target节点，并获得子节点和子节点的InnerText
            if (targets.Count != 0)
            {
                foreach (XmlNode target in targets)
                {
                    XmlNode targetPosition = target.ChildNodes[0];
                    int targetPositionIndex = int.Parse(targetPosition.InnerText);
                    //把得到的值存储到save中
                    save.livingTargetPositions.Add(targetPositionIndex);

                    XmlNode monsterType = target.ChildNodes[1];
                    int monsterTypeIndex = int.Parse(monsterType.InnerText);
                    save.livingTargetsTypes.Add(monsterTypeIndex);
                }
            }

            //得到存储的射击数和分数
            XmlNodeList shoots = xmlDoc.GetElementsByTagName("shoots");
            int shootNumCount = int.Parse(shoots[0].InnerText);
            save.shots = shootNumCount;

            XmlNodeList hits = xmlDoc.GetElementsByTagName("hits");
            int hitsCount = int.Parse(hits[0].InnerText);
            save.hits = hitsCount;

            SetGame(save);
            Debug.Log("Game Loaded By Serialize");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}