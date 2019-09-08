using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    //程序功能：每帧实例化一个随机颜色的物体，坐标在某范围内随机；
    //且每帧该物体都会缩小，当缩小到一定的尺寸时，就销毁物体

    public GameObject cubePrefabVar;    //实例化物体原型
    public List<GameObject> gameObjectList; //生成物体的表
    public float scalingFactor = 0.95f;     //每次缩小的比例
    public int numCubes = 0;    //生成物体编号

    void Start()
    {   
        gameObjectList = new List<GameObject>();    //初始化
    }

    void Update()
    {
        numCubes++;
        
        //实例化一个 cubePrefabVar物体，返回类型为GameObject，接受者是gObj
        GameObject gObj = Instantiate(cubePrefabVar) as GameObject;

        //设置物体的名字，并设置随机颜色
        gObj.name = "Cube " + numCubes;
        Color c = new Color(Random.value, Random.value, Random.value);
        gObj.GetComponent<MeshRenderer>().material.color = c;

        //物体的坐标随机分布在 球心(0,0,0) * 10 内
        gObj.transform.position = Random.insideUnitSphere * 10;

        //将新创建的物体添加到 已存物体表 内
        gameObjectList.Add(gObj);

        //同时创建 待删物体表
        List<GameObject> removeList = new List<GameObject>();
  
        //遍历 已存物体表
        foreach(GameObject goTemp in gameObjectList)
        {
            //缩小每一个 已存物体的几何尺寸
            float scale = goTemp.transform.localScale.x;
            scale *= scalingFactor;
            goTemp.transform.localScale = Vector3.one * scale;

            //当尺寸达到临界值，则将物体添加到 待删物体表
            if (scale <= 0.1f)
                removeList.Add(goTemp);
        }
        
        //遍历 待删物体表
        foreach(GameObject goTemp in removeList)
        {
            //移除对象并销毁
            gameObjectList.Remove(goTemp);
            Destroy(goTemp);
        }    

        //备注：C#不允许在遍历 当前List 的 foreach()内 修改当前List 内的元素；
        //故本例需要两个 foreach()配合实现功能
    }
}
